using ScsLib.HashFileSystem.Named;
using System.Collections.Generic;

namespace ScsLib.HashFileSystem
{
	public interface IHashFs
	{
		IReadOnlyDictionary<ulong, IHashEntry> Entries { get; }
		INamedHashDirectory RootDirectory { get; }
	}
}