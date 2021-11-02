using ScsLib.Map.Prefab;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public class PrefabNodeReader : IPrefabNodeReader
	{
		private readonly IFloat3Reader _float3Reader;

		public PrefabNodeReader(IFloat3Reader float3Reader)
		{
			_float3Reader = float3Reader;
		}

		public async ValueTask<PrefabNode> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				return new PrefabNode
				{
					TerrainPointIndex = reader.ReadInt32(),
					TerrainPointCount = reader.ReadInt32(),
					VariantIndex = reader.ReadInt32(),
					VariantCount = reader.ReadInt32(),
					Position = await _float3Reader.ReadAsync(stream, cancellationToken).ConfigureAwait(false),
					Direction = await _float3Reader.ReadAsync(stream, cancellationToken).ConfigureAwait(false),
					InputLines = Enumerable.Range(0, 8).Select(_ => reader.ReadInt32()).ToArray(),
					OutputLines = Enumerable.Range(0, 8).Select(_ => reader.ReadInt32()).ToArray()
				};
			}
		}
	}
}
