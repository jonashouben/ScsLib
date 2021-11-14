using ScsLib.Map;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Reader
{
	public class MbdReader : IMbdReader
	{
		private readonly IBinarySerializer _binarySerializer;

		public MbdReader(IBinarySerializer binarySerializer)
		{
			_binarySerializer = binarySerializer;
		}

		public async ValueTask<Mbd> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			stream.Seek(0, SeekOrigin.Begin);

			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				return _binarySerializer.Deserialize<Mbd>(reader);
			}
		}
	}
}
