using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public interface IHashFsHeaderReader
	{
		Task<HashFsHeader> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
