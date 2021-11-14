using ScsLib.Map;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Reader
{
	public class SectorReader : ISectorReader
	{
		private readonly IBinarySerializer _binarySerializer;

		public SectorReader(IBinarySerializer binarySerializer)
		{
			_binarySerializer = binarySerializer;
		}

		public async ValueTask<Sector> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				return _binarySerializer.Deserialize<Sector>(reader);
			}
		}
	}
}
