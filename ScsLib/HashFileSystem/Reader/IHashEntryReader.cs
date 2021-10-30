using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public interface IHashEntryReader
	{
		Task<byte[]> ReadAsync(FileStream fileStream, HashEntry hashEntry, CancellationToken cancellationToken = default);
		Task<string> ReadStringAsync(FileStream fileStream, HashEntry hashEntry, CancellationToken cancellationToken = default);
	}
}
