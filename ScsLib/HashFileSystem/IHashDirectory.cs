using System.Collections.Generic;

namespace ScsLib.HashFileSystem
{
	public interface IHashDirectory : IHashEntry
	{
		IReadOnlyCollection<IHashDirectoryEntry> EntryNames { get; }
	}
}
