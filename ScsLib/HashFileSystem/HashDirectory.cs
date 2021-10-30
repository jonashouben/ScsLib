using System;
using System.Collections.Generic;

namespace ScsLib.HashFileSystem
{
	public class HashDirectory : HashEntry
	{
		public IReadOnlyCollection<HashDirectoryEntry> EntryNames { get; internal set; } = Array.Empty<HashDirectoryEntry>();
	}
}
