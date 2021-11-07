using ScsLib.Map.Prefab;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public class PrefabReader : IPrefabReader
	{
		private readonly IBinarySerializer _binarySerializer;

		public PrefabReader(IBinarySerializer binarySerializer)
		{
			_binarySerializer = binarySerializer;
		}

		public async ValueTask<Prefab.Prefab> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			stream.Seek(0, SeekOrigin.Begin);

			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				Prefab.Prefab prefab = _binarySerializer.Deserialize<Prefab.Prefab>(reader);

				stream.Seek(prefab.Header.NodeOffset, SeekOrigin.Begin);
				prefab.Nodes = _binarySerializer.DeserializeMany<PrefabNode>(reader, prefab.Header.NodeCount).ToArray();

				stream.Seek(prefab.Header.NavigationCurveOffset, SeekOrigin.Begin);
				prefab.NavigationCurves = _binarySerializer.DeserializeMany<PrefabNavigationCurve>(reader, prefab.Header.NavigationCurveCount).ToArray();

				stream.Seek(prefab.Header.SignOffset, SeekOrigin.Begin);
				prefab.Signs = _binarySerializer.DeserializeMany<PrefabSign>(reader, prefab.Header.SignCount).ToArray();

				stream.Seek(prefab.Header.SemaphoreOffset, SeekOrigin.Begin);
				prefab.Semaphores = _binarySerializer.DeserializeMany<PrefabSemaphore>(reader, prefab.Header.SemaphoreCount).ToArray();

				stream.Seek(prefab.Header.SpawnPointOffset, SeekOrigin.Begin);
				prefab.SpawnPoints = _binarySerializer.DeserializeMany<PrefabSpawnPoint>(reader, prefab.Header.SpawnPointCount).ToArray();

				stream.Seek(prefab.Header.TerrainPointPosOffset, SeekOrigin.Begin);
				stream.Seek(prefab.Header.TerrainPointNormalOffset, SeekOrigin.Begin);
				stream.Seek(prefab.Header.TerrainPointVariantOffset, SeekOrigin.Begin);
				stream.Seek(prefab.Header.MapPointOffset, SeekOrigin.Begin);
				prefab.MapPoints = _binarySerializer.DeserializeMany<PrefabMapPoint>(reader, prefab.Header.MapPointCount).ToArray();

				stream.Seek(prefab.Header.TriggerPointOffset, SeekOrigin.Begin);
				prefab.TriggerPoints = _binarySerializer.DeserializeMany<PrefabTriggerPoint>(reader, prefab.Header.TriggerPointCount).ToArray();

				stream.Seek(prefab.Header.IntersectionOffset, SeekOrigin.Begin);
				prefab.Intersections = _binarySerializer.DeserializeMany<PrefabIntersection>(reader, prefab.Header.IntersectionCount).ToArray();

				stream.Seek(prefab.Header.NavigationNodeOffset, SeekOrigin.Begin);

				return prefab;
			}
		}
	}
}
