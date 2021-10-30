using ScsLib.HashFileSystem.Named;
using System.Collections.Generic;

namespace ScsLib.HashFileSystem
{
	public class HashFs
	{
		public HashFsHeader Header { get; internal set; } = default!;
		public IReadOnlyDictionary<ulong, HashEntry> Entries { get; internal set; } = new Dictionary<ulong, HashEntry>();
		public NamedHashDirectory RootDirectory { get; internal set; } = default!;
	}
}
