using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public interface IHashFsReader
	{
		FileStream Open(string path);
		Task<bool> HasSignatureAsync(Stream stream, CancellationToken cancellationToken = default);
		Task<HashFs> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
