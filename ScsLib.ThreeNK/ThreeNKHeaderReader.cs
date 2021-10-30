using AsyncBinaryExtensions;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.ThreeNK
{
	public class ThreeNKHeaderReader : IThreeNKHeaderReader
	{
		public async Task<ThreeNKHeader> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			stream.Seek(0, SeekOrigin.Begin);

			byte[] buffer = await stream.ReadBytesAsync(ThreeNKHeader.HeaderSize, cancellationToken).ConfigureAwait(false);

			using (MemoryStream ms = new MemoryStream(buffer))
			{
				using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8, true))
				{
					return new ThreeNKHeader
					{
						Signature = reader.ReadUInt32(),
						UnknownByte = reader.ReadByte(),
						Seed = reader.ReadByte()
					};
				}
			}
		}
	}
}
