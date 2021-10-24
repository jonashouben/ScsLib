using System.Collections.Generic;

namespace ScsLib.HashFileSystem.Named
{
	public interface INamedHashDirectory : IHashDirectory, INamedHashEntry
	{
		IReadOnlyCollection<INamedHashEntry> Entries { get; }
	}
}
