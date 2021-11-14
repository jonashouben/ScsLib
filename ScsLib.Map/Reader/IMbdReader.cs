using ScsLib.Map;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Reader
{
	public interface IMbdReader
	{
		ValueTask<Mbd> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
