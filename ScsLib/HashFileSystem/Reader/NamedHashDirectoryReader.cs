using ScsLib.HashFileSystem.Named;
using ScsLib.Hashing;
using System.Collections.Generic;

namespace ScsLib.HashFileSystem.Reader
{
	public class NamedHashDirectoryReader : INamedHashDirectoryReader
	{
		private readonly ICityHash _cityHash;

		public NamedHashDirectoryReader(ICityHash cityHash)
		{
			_cityHash = cityHash;
		}

		public IEnumerable<INamedHashEntry> Read(HashFs hashFs, NamedHashDirectory namedHashDirectory)
		{
			foreach (HashDirectoryEntry entry in namedHashDirectory.EntryNames)
			{
				string path = $"{namedHashDirectory.VirtualPath}{(namedHashDirectory.VirtualPath.Length > 0 ? "/" : "")}{entry.Name}";

				if (hashFs.Entries.TryGetValue(_cityHash.CityHash64(path), out HashEntry? hashEntry))
				{
					if (hashEntry is HashDirectory hashDirectory)
					{
						yield return new NamedHashDirectory
						{
							Header = hashDirectory.Header,
							EntryNames = hashDirectory.EntryNames,
							VirtualPath = path
						};
					}
					else if (hashEntry is HashFile hashFile)
					{
						yield return new NamedHashFile
						{
							Header = hashFile.Header,
							VirtualPath = path
						};
					}
				}
			}
		}
	}
}
