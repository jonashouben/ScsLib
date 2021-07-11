using AsyncBinaryExtensions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ScsLib.HashFileSystem;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using ScsLib.Hashing;
using System.Diagnostics.CodeAnalysis;

namespace ScsLib
{
	public sealed class ScsFile : IDisposable
	{
		internal const uint Magic = 592659283;
		internal const uint CityHashMethod = 1498696003;
		internal const ushort HashVersion = 1;

		private readonly Stream _stream;
		private bool _disposed;

		public ScsFileHeader Header { get; internal set; } = default!;
		public IReadOnlyDictionary<ulong, HashEntry> Entries { get; internal set; } = new Dictionary<ulong, HashEntry>();
		public HashDirectory RootDirectory { get; internal set; } = default!;

		private ScsFile(Stream stream)
		{
			_stream = stream;
		}

		public bool TryGetEntry(ulong hash, [NotNullWhen(true)] out HashEntry? entry)
		{
			return Entries.TryGetValue(hash, out entry);
		}

		public bool TryGetEntry(string path, [NotNullWhen(true)] out HashEntry? entry)
		{
			if (TryGetEntry(CityHash.CityHash64(path), out entry))
			{
				return true;
			}

			return false;
		}

		public bool TryGetDirectory(ulong hash, [NotNullWhen(true)] out HashDirectory? directory)
		{
			if (TryGetEntry(hash, out HashEntry? entry))
			{
				if (entry is HashDirectory directoryEntry)
				{
					directory = directoryEntry;
					return true;
				}
			}

			directory = default!;
			return false;
		}

		public bool TryGetDirectory(string path, [NotNullWhen(true)] out HashDirectory? directory)
		{
			return TryGetDirectory(CityHash.CityHash64(path), out directory);
		}

		public bool TryGetFile(ulong hash, [NotNullWhen(true)] out HashFile? file)
		{
			if (TryGetEntry(hash, out HashEntry? entry))
			{
				if (entry is HashFile fileEntry)
				{
					file = fileEntry;
					return true;
				}
			}

			file = default!;
			return false;
		}

		public bool TryGetFile(string filePath, [NotNullWhen(true)] out HashFile? file)
		{
			return TryGetFile(CityHash.CityHash64(filePath), out file);
		}

		public async ValueTask<byte[]> ReadFile(HashFile hashFile, CancellationToken cancellationToken = default)
		{
			byte[] data = await hashFile.ReadBytes(_stream, cancellationToken).ConfigureAwait(false);

			if (data.Length >= 4 && BitConverter.ToUInt32(data, 0) == 0x014B6E33) //3nk
			{
				using (MemoryStream ms = new MemoryStream(data))
				{
					return SII3nkTranscode.Transcode(ms);
				}
			}

			return data;
		}

		public static async Task<ScsFile> Read(Stream stream, CancellationToken cancellationToken = default)
		{
			if (!stream.CanRead) throw new InvalidOperationException();
			if (!stream.CanSeek) throw new InvalidOperationException();
			if (stream.Length < ScsFileHeader.HeaderSize) throw new FormatException($"Stream has less than {ScsFileHeader.HeaderSize} bytes (no header) !");

			// Read File Header

			ScsFileHeader header;
			using (MemoryStream ms = new MemoryStream(await stream.ReadBytesAsync(ScsFileHeader.HeaderSize, cancellationToken).ConfigureAwait(false)))
			{
				using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8, true))
				{
					header = new ScsFileHeader
					{
						Magic = reader.ReadUInt32(),
						Version = reader.ReadUInt16(),
						Salt = reader.ReadUInt16(),
						HashMethod = reader.ReadUInt32(),
						EntryCount = reader.ReadInt32(),
						StartOffset = reader.ReadInt32()
					};
				}
			}

			if (header.Magic != Magic) throw new NotSupportedException($"Magic {header.Magic} not supported!");
			if (header.HashMethod != CityHashMethod) throw new NotSupportedException($"HashMethod {header.HashMethod} not supported!");
			if (header.Version != HashVersion) throw new NotSupportedException($"HashVersion {header.Version} not supported!");

			// Read Entry Headers

			stream.Seek(header.StartOffset, SeekOrigin.Begin);

			List<HashEntryHeader> entryHeaders = new List<HashEntryHeader>();

			using (MemoryStream ms = new MemoryStream(await stream.ReadBytesAsync(HashEntryHeader.HeaderSize * header.EntryCount, cancellationToken).ConfigureAwait(false)))
			{
				using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8, true))
				{
					for (int i = 0; i < header.EntryCount; i++)
					{
						entryHeaders.Add(new HashEntryHeader
						{
							Hash = reader.ReadUInt64(),
							Offset = reader.ReadInt64(),
							Options = (HashEntryOption) reader.ReadUInt32(),
							Crc = reader.ReadUInt32(),
							Size = reader.ReadInt32(),
							CompressedSize = reader.ReadInt32()
						});
					}
				}
			}

			// Create Entries from Headers

			Dictionary<ulong, HashEntry> entries = new Dictionary<ulong, HashEntry>();

			foreach (HashEntryHeader entryHeader in entryHeaders)
			{
				HashEntry entry;

				if (entryHeader.Options.HasFlag(HashEntryOption.Directory))
				{
					entry = new HashDirectory { Header = entryHeader };
				}
				else
				{
					entry = new HashFile { Header = entryHeader };
				}

				entries.Add(entryHeader.Hash, entry);
			}

			ScsFile scsFile = new ScsFile(stream) { Header = header, Entries = entries };

			// Create Root Directory

			if (scsFile.TryGetDirectory(CityHashes.RootEntry, out HashDirectory? rootDirectory))
			{
				rootDirectory.VirtualPath = "";
				scsFile.RootDirectory = rootDirectory;
			}
			else
			{
				// Create a dummy root directory, and add known directories and files

				DummyHashDirectory dummyRootDirectory = new DummyHashDirectory { VirtualPath = "" };

				foreach (string knownDirectory in CityHashes.KnownDirectoryNames)
				{
					if (scsFile.TryGetDirectory(knownDirectory, out _))
					{
						dummyRootDirectory.AddDirectory(knownDirectory);
					}
				}

				foreach (string knownFile in CityHashes.KnownFileNames)
				{
					if (scsFile.TryGetFile(knownFile, out _))
					{
						dummyRootDirectory.AddFile(knownFile);
					}
				}

				scsFile.RootDirectory = dummyRootDirectory;
			}

			scsFile.RootDirectory.Entries = await GetEntries(scsFile, scsFile.RootDirectory, stream, cancellationToken).ToArrayAsync(cancellationToken).ConfigureAwait(false);

			return scsFile;
		}

		public static async Task<ScsFile> Read(string path, CancellationToken cancellationToken = default)
		{
			return await Read(new FileStream(path, FileMode.Open, FileAccess.Read), cancellationToken).ConfigureAwait(false);
		}

		private static async IAsyncEnumerable<HashEntry> GetEntries(ScsFile scsFile, HashDirectory directory, Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			foreach (string str in (await directory.ReadString(stream, cancellationToken).ConfigureAwait(false)).Split('\n'))
			{
				string path = directory.VirtualPath + (directory.VirtualPath.Length > 0 ? "/" : "");

				if (str.StartsWith("*", StringComparison.Ordinal))
				{
					path += str.Substring(1);
					if (scsFile.TryGetDirectory(path, out HashDirectory? nextDirectory))
					{
						nextDirectory.VirtualPath = path;
						nextDirectory.Entries = await GetEntries(scsFile, nextDirectory, stream, cancellationToken).ToArrayAsync(cancellationToken).ConfigureAwait(false);
						yield return nextDirectory;
					}
				}
				else
				{
					path += str;
					if (scsFile.TryGetFile(path, out HashFile? file))
					{
						file.VirtualPath = path;
						yield return file;
					}
				}
			}
		}

		#region IDisposable
		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_stream.Dispose();
				}

				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}