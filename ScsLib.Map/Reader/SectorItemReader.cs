using ScsLib.Map.SectorItem;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Reader
{
	public class SectorItemReader : ISectorItemReader
	{
		private readonly IBinarySerializer _binarySerializer;

		public SectorItemReader(IBinarySerializer binarySerializer)
		{
			_binarySerializer = binarySerializer;
		}

		public async ValueTask<AbstractSectorItem> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				SectorItemType type = (SectorItemType) reader.ReadUInt32();

				switch (type)
				{
					case SectorItemType.CutPlane:
						return _binarySerializer.Deserialize<CutPlaneSectorItem>(reader);
					default:
						throw new NotSupportedException($"Type {type} not supported!");
				}
			}
		}
	}
}
