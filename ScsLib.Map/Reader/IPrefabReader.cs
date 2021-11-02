using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public interface IPrefabReader
	{
		ValueTask<Prefab.Prefab> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
