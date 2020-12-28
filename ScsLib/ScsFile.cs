using AsyncBinaryExtensions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ScsLib.HashFileSystem;
using System.Runtime.CompilerServices;
using System.Linq;

namespace ScsLib
{
	public sealed class ScsFile : IDisposable
	{
		internal const uint Magic = 592659283;
		internal const uint CityHashMethod = 1498696003;
		internal const ushort HashVersion = 1;

		private readonly Stream _stream;
		private bool _disposed;

		public ScsFileHeader Header { get; internal set; }
		public IReadOnlyDictionary<ulong, HashEntry> Entries { get; internal set; }
		public HashDirectory RootDirectory { get; internal set; }

		private ScsFile(Stream stream)
		{
			_stream = stream;
		}

		public bool TryGetEntry(ulong hash, out HashEntry entry)
		{
			return Entries.TryGetValue(hash, out entry);
		}

		public bool TryGetEntry(string path, out HashEntry entry)
		{
			if (path == null) throw new ArgumentNullException(nameof(path));

			if (TryGetEntry(CityHash.CityHash64(path), out entry))
			{
				return true;
			}

			entry = null;
			return false;
		}

		public bool TryGetDirectory(ulong hash, out HashDirectory directory)
		{
			if (TryGetEntry(hash, out HashEntry entry))
			{
				directory = entry as HashDirectory;
				return directory != null;
			}

			directory = null;
			return false;
		}

		public bool TryGetDirectory(string path, out HashDirectory directory)
		{
			if (path == null) throw new ArgumentNullException(nameof(path));

			return TryGetDirectory(CityHash.CityHash64(path), out directory);
		}

		public bool TryGetFile(ulong hash, out HashFile file)
		{
			if (TryGetEntry(hash, out HashEntry entry))
			{
				file = entry as HashFile;
				return file != null;
			}

			file = null;
			return false;
		}

		public bool TryGetFile(string filePath, out HashFile file)
		{
			if (filePath == null) throw new ArgumentNullException(nameof(filePath));

			return TryGetFile(CityHash.CityHash64(filePath), out file);
		}

		public async ValueTask<byte[]> ReadFile(HashFile hashFile, CancellationToken cancellationToken = default)
		{
			if (hashFile == null) throw new ArgumentNullException(nameof(hashFile));

			byte[] data = await hashFile.ReadBytes(_stream, cancellationToken).ConfigureAwait(false);

			if (data.Length >= 4 && BitConverter.ToUInt32(data, 0) == 0x014B6E33) //3nk
			{
				using (MemoryStream ms = new MemoryStream(data))
				{
					return await SII3nkTranscode.Transcode(ms, cancellationToken).ConfigureAwait(false);
				}
			}

			return data;
		}

		public static async Task<ScsFile> Read(Stream stream, CancellationToken cancellationToken = default)
		{
			if (stream == null) throw new ArgumentNullException(nameof(stream));
			if (!stream.CanRead) throw new InvalidOperationException();
			if (!stream.CanSeek) throw new InvalidOperationException();
			if (stream.Length < ScsFileHeader.HeaderSize) throw new FormatException($"Stream has less than {ScsFileHeader.HeaderSize} bytes (no header) !");

			// Read File Header

			ScsFileHeader header;
			using (MemoryStream ms = new MemoryStream(await stream.ReadBytesAsync(ScsFileHeader.HeaderSize, cancellationToken).ConfigureAwait(false)))
			{
				header = new ScsFileHeader
				{
					Magic = await ms.ReadUIntAsync(cancellationToken).ConfigureAwait(false),
					Version = await ms.ReadUShortAsync(cancellationToken).ConfigureAwait(false),
					Salt = await ms.ReadUShortAsync(cancellationToken).ConfigureAwait(false),
					HashMethod = await ms.ReadUIntAsync(cancellationToken).ConfigureAwait(false),
					EntryCount = await ms.ReadIntAsync(cancellationToken).ConfigureAwait(false),
					StartOffset = await ms.ReadIntAsync(cancellationToken).ConfigureAwait(false)
				};
			}

			if (header.Magic != Magic) throw new NotSupportedException($"Magic {header.Magic} not supported!");
			if (header.HashMethod != CityHashMethod) throw new NotSupportedException($"HashMethod {header.HashMethod} not supported!");
			if (header.Version != HashVersion) throw new NotSupportedException($"HashVersion {header.Version} not supported!");

			// Read Entry Headers

			stream.Seek(header.StartOffset, SeekOrigin.Begin);

			List<HashEntryHeader> entryHeaders = new List<HashEntryHeader>();

			using (MemoryStream ms = new MemoryStream(await stream.ReadBytesAsync(HashEntryHeader.HeaderSize * header.EntryCount, cancellationToken).ConfigureAwait(false)))
			{
				for (int i = 0; i < header.EntryCount; i++)
				{
					entryHeaders.Add(new HashEntryHeader
					{
						Hash = await ms.ReadULongAsync(cancellationToken).ConfigureAwait(false),
						Offset = await ms.ReadLongAsync(cancellationToken).ConfigureAwait(false),
						Flags = (HashEntryFlag) await ms.ReadUIntAsync(cancellationToken).ConfigureAwait(false),
						Crc = await ms.ReadUIntAsync(cancellationToken).ConfigureAwait(false),
						Size = await ms.ReadIntAsync(cancellationToken).ConfigureAwait(false),
						CompressedSize = await ms.ReadIntAsync(cancellationToken).ConfigureAwait(false)
					});
				}
			}

			// Create Entries from Headers

			Dictionary<ulong, HashEntry> entries = new Dictionary<ulong, HashEntry>();

			foreach (HashEntryHeader entryHeader in entryHeaders)
			{
				HashEntry entry;

				if (entryHeader.Flags.HasFlag(HashEntryFlag.Directory))
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

			if (scsFile.TryGetDirectory(CityHashes.RootEntry, out HashDirectory rootDirectory))
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
			if (path == null) throw new ArgumentNullException(nameof(path));

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
					if (scsFile.TryGetDirectory(path, out HashDirectory nextDirectory))
					{
						nextDirectory.VirtualPath = path;
						nextDirectory.Entries = await GetEntries(scsFile, nextDirectory, stream, cancellationToken).ToArrayAsync(cancellationToken).ConfigureAwait(false);
						yield return nextDirectory;
					}
				}
				else
				{
					path += str;
					if (scsFile.TryGetFile(path, out HashFile file))
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