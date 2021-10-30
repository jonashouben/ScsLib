using ScsLib.HashFileSystem.Named;
using ScsLib.Hashing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public class HashFsReader : IHashFsReader
	{
		private static readonly string RootEntryName = string.Empty;

		private static readonly IReadOnlyCollection<string> KnownDirectoryNames = new string[]
		{
			"automat",
			"contentbrowser",
			"custom",
			"dlc",
			"def",
			"effect",
			"font",
			"locale",
			"map",
			"material",
			"matlib",
			"model",
			"model2",
			"panorama",
			"prefab",
			"prefab2",
			"road_template",
			"sound",
			"system",
			"ui",
			"uilab",
			"unit",
			"vehicle",
			"video"
		};

		private static readonly IReadOnlyCollection<string> KnownFileNames = new string[]
		{
			"base.cfg"
		};

		private readonly ulong _rootEntryHash;
		private readonly IReadOnlyDictionary<ulong, string> _knownHashes;
		private readonly IHashFsHeaderReader _hashFsHeaderReader;
		private readonly IHashFsEntryHeaderReader _hashFsEntryHeaderReader;
		private readonly IHashDirectoryReader _hashDirectoryReader;

		public HashFsReader(ICityHash cityHash, IHashFsHeaderReader hashFsHeaderReader, IHashFsEntryHeaderReader hashFsEntryHeaderReader, IHashDirectoryReader hashDirectoryReader)
		{
			_rootEntryHash = cityHash.CityHash64(RootEntryName);
			_knownHashes = KnownDirectoryNames.Concat(KnownFileNames).ToDictionary(row => cityHash.CityHash64(row));
			_hashFsHeaderReader = hashFsHeaderReader;
			_hashFsEntryHeaderReader = hashFsEntryHeaderReader;
			_hashDirectoryReader = hashDirectoryReader;
		}

		public FileStream Open(string path)
		{
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan | FileOptions.Asynchronous);
		}

		public async Task<HashFs> ReadAsync(FileStream fileStream, CancellationToken cancellationToken = default)
		{
			HashFsHeader header = await _hashFsHeaderReader.ReadAsync(fileStream, cancellationToken).ConfigureAwait(false);

			if (header.Magic != 592659283) throw new NotSupportedException($"Magic {header.Magic} not supported!");
			if (header.HashMethod != 1498696003) throw new NotSupportedException($"HashMethod {header.HashMethod} not supported!");
			if (header.Version != 1) throw new NotSupportedException($"HashVersion {header.Version} not supported!");

			IReadOnlyCollection<HashEntryHeader> entryHeaders = await _hashFsEntryHeaderReader.ReadAsync(fileStream, header, cancellationToken).ConfigureAwait(false);

			IReadOnlyDictionary<ulong, HashEntry> entries = await ReadEntries(fileStream, entryHeaders, cancellationToken).ToDictionaryAsync(row => row.Header.Hash, cancellationToken).ConfigureAwait(false);

			NamedHashDirectory rootDirectory = GetRootDirectory(entries);

			return new HashFs
			{
				Header = header,
				Entries = entries,
				RootDirectory = rootDirectory
			};
		}

		internal async IAsyncEnumerable<HashEntry> ReadEntries(FileStream fileStream, IReadOnlyCollection<HashEntryHeader> entryHeaders, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			foreach (HashEntryHeader entryHeader in entryHeaders)
			{
				if (entryHeader.Options.HasFlag(HashEntryOption.Directory))
				{
					HashDirectory hashDirectory = new HashDirectory
					{
						Header = entryHeader
					};

					hashDirectory.EntryNames = await _hashDirectoryReader.ReadAsync(fileStream, hashDirectory, cancellationToken).ConfigureAwait(false);

					yield return hashDirectory;
				}
				else
				{
					yield return new HashFile
					{
						Header = entryHeader
					};
				}
			}
		}

		internal NamedHashDirectory GetRootDirectory(IReadOnlyDictionary<ulong, HashEntry> entries)
		{
			if (entries.TryGetValue(_rootEntryHash, out HashEntry? hashEntry) && hashEntry is HashDirectory directory)
			{
				if (directory is NamedHashDirectory namedHashDirectory)
				{
					return namedHashDirectory;
				}
				else
				{
					return new NamedHashDirectory
					{
						Header = directory.Header,
						EntryNames = directory.EntryNames,
						VirtualPath = RootEntryName
					};
				}
			}
			else
			{
				return new NamedHashDirectory
				{
					VirtualPath = string.Empty,
					Header = new HashEntryHeader
					{
						Hash = _rootEntryHash
					},
					EntryNames = _knownHashes
						.Select(row => new
						{
							Key = row.Value,
							Value = entries.FirstOrDefault(r => r.Key == row.Key).Value
						})
						.Where(row => row.Value != null)
						.Select(row =>
						{
							if (row.Value is HashDirectory)
							{
								return new HashDirectoryEntry
								{
									Name = row.Key,
									Type = HashDirectoryEntryType.Directory
								};
							}
							else if (row.Value is HashFile)
							{
								return new HashDirectoryEntry
								{
									Name = row.Key,
									Type = HashDirectoryEntryType.File
								};
							}
							else
							{
								return new HashDirectoryEntry
								{
									Name = row.Key,
									Type = HashDirectoryEntryType.None
								};
							}
						})
						.ToArray()
				};
			}
		}
	}
}
