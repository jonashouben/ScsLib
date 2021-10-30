using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public interface IHashFsReader
	{
		FileStream Open(string path);
		Task<HashFs> ReadAsync(FileStream fileStream, CancellationToken cancellationToken = default);
	}
}
