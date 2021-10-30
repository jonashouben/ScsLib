using ScsLib.HashFileSystem.Named;
using System.Collections.Generic;

namespace ScsLib.HashFileSystem.Reader
{
	public interface INamedHashDirectoryReader
	{
		IEnumerable<INamedHashEntry> Read(HashFs hashFs, NamedHashDirectory namedHashDirectory);
	}
}
