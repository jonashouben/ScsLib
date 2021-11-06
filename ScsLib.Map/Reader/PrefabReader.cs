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
		private readonly BinarySerializer _binarySerializer;

		public PrefabReader(BinarySerializer binarySerializer)
		{
			_binarySerializer = binarySerializer;
		}

		public async ValueTask<Prefab.Prefab> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			stream.Seek(0, SeekOrigin.Begin);

			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				Prefab.Prefab prefab = await _binarySerializer.DeserializeAsync<Prefab.Prefab>(reader, cancellationToken).ConfigureAwait(false);

				uint nodeCount = reader.ReadUInt32();
				uint navigationCurveCount = reader.ReadUInt32();
				uint signCount = reader.ReadUInt32();
				uint semaphoreCount = reader.ReadUInt32();
				uint spawnPointCount = reader.ReadUInt32();
				uint terrainPointCount = reader.ReadUInt32();
				uint terrainPointVariantCount = reader.ReadUInt32();
				uint mapPointCount = reader.ReadUInt32();
				uint triggerPointCount = reader.ReadUInt32();
				uint intersectionCount = reader.ReadUInt32();
				uint navigationNodeCount = reader.ReadUInt32();
				uint nodeOffset = reader.ReadUInt32();
				uint navigationCurveOffset = reader.ReadUInt32();
				uint signOffset = reader.ReadUInt32();
				uint semaphoreOffset = reader.ReadUInt32();
				uint spawnPointOffset = reader.ReadUInt32();
				uint terrainPointPosOffset = reader.ReadUInt32();
				uint terrainPointNormalOffset = reader.ReadUInt32();
				uint terrainPointVariantOffset = reader.ReadUInt32();
				uint mapPointOffset = reader.ReadUInt32();
				uint triggerPointOffset = reader.ReadUInt32();
				uint intersectionOffset = reader.ReadUInt32();
				uint navigationNodeOffset = reader.ReadUInt32();

				stream.Seek(nodeOffset, SeekOrigin.Begin);
				List<PrefabNode> nodes = new List<PrefabNode>();
				for (uint i = 0; i < nodeCount; i++)
				{
					nodes.Add(await _binarySerializer.DeserializeAsync<PrefabNode>(reader, cancellationToken).ConfigureAwait(false));
				}
				prefab.Nodes = nodes;

				stream.Seek(navigationCurveOffset, SeekOrigin.Begin);
				List<PrefabCurve> curves = new List<PrefabCurve>();
				for (uint i = 0; i < navigationCurveCount; i++)
				{
					curves.Add(await _binarySerializer.DeserializeAsync<PrefabCurve>(reader, cancellationToken).ConfigureAwait(false));
				}
				prefab.NavigationCurves = curves;

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

				return prefab;
			}
		}
	}
}
