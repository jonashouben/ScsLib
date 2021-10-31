using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public interface IHashDirectoryReader
	{
		Task<IReadOnlyCollection<HashDirectoryEntry>> ReadAsync(Stream stream, HashDirectory hashDirectory, CancellationToken cancellationToken = default);
	}
}
