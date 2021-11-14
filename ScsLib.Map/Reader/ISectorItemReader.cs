using ScsLib.Map.SectorItem;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Reader
{
	public interface ISectorItemReader
	{
		ValueTask<AbstractSectorItem> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
