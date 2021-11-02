using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public interface IQuaternionReader
	{
		ValueTask<Quaternion> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
