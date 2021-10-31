using ScsLib.HashFileSystem;
using ScsLib.HashFileSystem.Named;
using System.Collections.Generic;
using System.Linq;

namespace ScsLib.Wpf
{
	public class NamedHashDirectoryTree : INamedHashEntry
	{
		public HashEntryHeader Header { get; internal set; } = default!;
		public string VirtualPath { get; internal set; } = default!;
		public bool IsManual { get; internal set; }
		public bool IsExpanded { get; internal set; }
		public IEnumerable<INamedHashEntry> Entries { get; internal set; } = Enumerable.Empty<INamedHashEntry>();
	}
}
