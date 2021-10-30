using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.ThreeNK
{
	public interface IThreeNKReader
	{
		Task<bool> HasSignatureAsync(Stream stream, CancellationToken cancellationToken = default);
		Task<byte[]> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
