using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.ThreeNK
{
	public interface IThreeNKHeaderReader
	{
		Task<ThreeNKHeader> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
