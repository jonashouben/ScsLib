using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public interface IHashEntryReader
	{
		Task<byte[]> ReadAsync(Stream stream, HashEntry hashEntry, CancellationToken cancellationToken = default);
		Task<string> ReadStringAsync(Stream stream, HashEntry hashEntry, CancellationToken cancellationToken = default);
	}
}
