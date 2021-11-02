using ScsLib.Map.Prefab;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public class PrefabReader : IPrefabReader
	{
		private readonly IPrefabNodeReader _prefabNodeReader;
		private readonly IPrefabCurveReader _prefabCurveReader;

		public PrefabReader(IPrefabNodeReader prefabNodeReader, IPrefabCurveReader prefabCurveReader)
		{
			_prefabNodeReader = prefabNodeReader;
			_prefabCurveReader = prefabCurveReader;
		}

		public async ValueTask<Prefab.Prefab> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			stream.Seek(0, SeekOrigin.Begin);

			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				int version = reader.ReadInt32();
				int nodeCount = reader.ReadInt32();
				int navigationCurveCount = reader.ReadInt32();
				int signCount = reader.ReadInt32();
				int semaphoreCount = reader.ReadInt32();
				int spawnPointCount = reader.ReadInt32();
				int terrainPointCount = reader.ReadInt32();
				int terrainPointVariantCount = reader.ReadInt32();
				int mapPointCount = reader.ReadInt32();
				int triggerPointCount = reader.ReadInt32();
				int intersectionCount = reader.ReadInt32();
				int navigationNodeCount = reader.ReadInt32();
				int nodeOffset = reader.ReadInt32();
				int navigationCurveOffset = reader.ReadInt32();
				int signOffset = reader.ReadInt32();
				int semaphoreOffset = reader.ReadInt32();
				int spawnPointOffset = reader.ReadInt32();
				int terrainPointPosOffset = reader.ReadInt32();
				int terrainPointNormalOffset = reader.ReadInt32();
				int terrainPointVariantOffset = reader.ReadInt32();
				int mapPointOffset = reader.ReadInt32();
				int triggerPointOffset = reader.ReadInt32();
				int intersectionOffset = reader.ReadInt32();
				int navigationNodeOffset = reader.ReadInt32();

				stream.Seek(nodeOffset, SeekOrigin.Begin);
				List<PrefabNode> nodes = new List<PrefabNode>();
				for (int i = 0; i < nodeCount; i++)
				{
					nodes.Add(await _prefabNodeReader.ReadAsync(stream, cancellationToken).ConfigureAwait(false));
				}
				stream.Seek(navigationCurveOffset, SeekOrigin.Begin);
				List<PrefabCurve> curves = new List<PrefabCurve>();
				for (int i = 0; i < navigationCurveCount; i++)
				{
					curves.Add(await _prefabCurveReader.ReadAsync(stream, cancellationToken).ConfigureAwait(false));
				}
				stream.Seek(signOffset, SeekOrigin.Begin);
				stream.Seek(semaphoreOffset, SeekOrigin.Begin);
				stream.Seek(spawnPointOffset, SeekOrigin.Begin);
				stream.Seek(terrainPointPosOffset, SeekOrigin.Begin);
				stream.Seek(terrainPointNormalOffset, SeekOrigin.Begin);
				stream.Seek(terrainPointVariantOffset, SeekOrigin.Begin);
				stream.Seek(mapPointOffset, SeekOrigin.Begin);
				stream.Seek(triggerPointOffset, SeekOrigin.Begin);
				stream.Seek(intersectionOffset, SeekOrigin.Begin);
				stream.Seek(navigationNodeOffset, SeekOrigin.Begin);

				return new Prefab.Prefab
				{
					Version = version,
					Nodes = nodes,
					NavigationCurves = curves
				};
			}
		}
	}
}
