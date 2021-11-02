using ScsLib.Map.Prefab;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public interface IPrefabCurveReader
	{
		ValueTask<PrefabCurve> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
