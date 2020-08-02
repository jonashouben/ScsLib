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
		private readonly FileStream _fileStream;
		private bool _disposed;

		public ScsFileHeader Header { get; internal set; }
		public IReadOnlyDictionary<ulong, HashEntry> Entries { get; internal set; }
		public HashDirectory RootDirectory { get; internal set; }

		private ScsFile(FileStream fileStream)
		{
			_fileStream = fileStream;
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

			return await hashFile.ReadBytes(_fileStream, cancellationToken).ConfigureAwait(false);
		}

		public static async Task<ScsFile> Read(string path, CancellationToken cancellationToken = default)
		{
			if (path == null) throw new ArgumentNullException(nameof(path));

			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

			// Read File Header

			ScsFileHeader header;
			using (MemoryStream ms = new MemoryStream(await fs.ReadBytesAsync(ScsFileHeader.HeaderSize, cancellationToken).ConfigureAwait(false)))
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

			// Read Entry Headers

			fs.Seek(header.StartOffset, SeekOrigin.Begin);

			List<HashEntryHeader> entryHeaders = new List<HashEntryHeader>();

			using (MemoryStream ms = new MemoryStream(await fs.ReadBytesAsync(HashEntryHeader.HeaderSize * header.EntryCount, cancellationToken).ConfigureAwait(false)))
			{
				for (int i = 0; i < header.EntryCount; i++)
				{
					entryHeaders.Add(new HashEntryHeader
					{
						Hash = await ms.ReadULongAsync(cancellationToken).ConfigureAwait(false),
						Offset = await ms.ReadLongAsync(cancellationToken).ConfigureAwait(false),
						Flags = await ms.ReadUIntAsync(cancellationToken).ConfigureAwait(false),
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

				switch (entryHeader.Flags)
				{
					case 1:
					case 3:
					case 5:
					case 7:
						entry = new HashDirectory { Header = entryHeader };
						break;
					default:
						entry = new HashFile { Header = entryHeader };
						break;
				}
				entries.Add(entryHeader.Hash, entry);
			}

			ScsFile scsFile = new ScsFile(fs) { Header = header, Entries = entries };

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

			scsFile.RootDirectory.Entries = await GetEntries(scsFile, scsFile.RootDirectory, fs, cancellationToken).ToArrayAsync(cancellationToken).ConfigureAwait(false);

			return scsFile;
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
					_fileStream.Dispose();
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