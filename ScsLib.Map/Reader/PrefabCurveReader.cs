using ScsLib.Map.Converter;
using ScsLib.Map.Prefab;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public class PrefabCurveReader : IPrefabCurveReader
	{
		private readonly ITokenConverter _tokenConverter;
		private readonly IFloat3Reader _float3Reader;
		private readonly IQuaternionReader _quaternionReader;

		public PrefabCurveReader(ITokenConverter tokenConverter, IFloat3Reader float3Reader, IQuaternionReader quaternionReader)
		{
			_tokenConverter = tokenConverter;
			_float3Reader = float3Reader;
			_quaternionReader = quaternionReader;
		}

		public async ValueTask<PrefabCurve> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				return new PrefabCurve
				{
					Name = _tokenConverter.FromToken(reader.ReadUInt64()),
					Flags = reader.ReadUInt32(),
					EndNode = reader.ReadByte(),
					EndLane = reader.ReadByte(),
					StartNode = reader.ReadByte(),
					StartLane = reader.ReadByte(),
					StartPosition = await _float3Reader.ReadAsync(stream, cancellationToken).ConfigureAwait(false),
					EndPosition = await _float3Reader.ReadAsync(stream, cancellationToken).ConfigureAwait(false),
					StartRotation = await _quaternionReader.ReadAsync(stream, cancellationToken).ConfigureAwait(false),
					EndRotation = await _quaternionReader.ReadAsync(stream, cancellationToken).ConfigureAwait(false),
					Length = reader.ReadSingle(),
					NextLines = Enumerable.Range(0, 4).Select(_ => reader.ReadInt32()).ToArray(),
					PreviousLines = Enumerable.Range(0, 4).Select(_ => reader.ReadInt32()).ToArray(),
					CountNext = reader.ReadUInt32(),
					CountPrevious = reader.ReadUInt32(),
					SemaphoreId = reader.ReadInt32(),
					TrafficRule = _tokenConverter.FromToken(reader.ReadUInt64()),
					NavigationNodeId = reader.ReadUInt32()
				};
			}
		}
	}
}
