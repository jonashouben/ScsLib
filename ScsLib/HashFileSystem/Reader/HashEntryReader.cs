using AsyncBinaryExtensions;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public class HashEntryReader : IHashEntryReader
	{
		public async Task<byte[]> ReadAsync(Stream stream, HashEntry hashEntry, CancellationToken cancellationToken = default)
		{
			stream.Seek(hashEntry.Header.Offset, SeekOrigin.Begin);

			if (hashEntry.Header.Options.HasFlag(HashEntryOption.Compressed))
			{
				using (InflaterInputStream inflaterInputStream = new InflaterInputStream(stream) { IsStreamOwner = false })
				{
					return await inflaterInputStream.ReadBytesAsync(hashEntry.Header.Size, cancellationToken).ConfigureAwait(false);
				}
			}
			else
			{
				return await stream.ReadBytesAsync(hashEntry.Header.Size, cancellationToken).ConfigureAwait(false);
			}
		}

		public async Task<string> ReadStringAsync(Stream stream, HashEntry hashEntry, CancellationToken cancellationToken = default)
		{
			return Encoding.UTF8.GetString(await ReadAsync(stream, hashEntry, cancellationToken).ConfigureAwait(false));
		}
	}
}
