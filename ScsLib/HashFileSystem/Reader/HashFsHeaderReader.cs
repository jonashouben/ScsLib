using AsyncBinaryExtensions;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public class HashFsHeaderReader : IHashFsHeaderReader
	{
		public async Task<HashFsHeader> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			stream.Seek(0, SeekOrigin.Begin);

			byte[] buffer = await stream.ReadBytesAsync(HashFsHeader.HeaderSize, cancellationToken).ConfigureAwait(false);

			using (MemoryStream ms = new MemoryStream(buffer))
			{
				using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8, true))
				{
					return new HashFsHeader
					{
						Signature = reader.ReadUInt32(),
						Version = reader.ReadUInt16(),
						Salt = reader.ReadUInt16(),
						HashMethod = reader.ReadUInt32(),
						EntryCount = reader.ReadInt32(),
						StartOffset = reader.ReadInt32()
					};
				}
			}
		}
	}
}
