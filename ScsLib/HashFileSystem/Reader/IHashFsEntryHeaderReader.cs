using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public interface IHashFsEntryHeaderReader
	{
		Task<IReadOnlyCollection<HashEntryHeader>> ReadAsync(FileStream fileStream, HashFsHeader hashFsHeader, CancellationToken cancellationToken = default);
	}
}
