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
		public async Task<byte[]> ReadAsync(FileStream fileStream, HashEntry hashEntry, CancellationToken cancellationToken = default)
		{
			fileStream.Seek(hashEntry.Header.Offset, SeekOrigin.Begin);

			if (hashEntry.Header.Options.HasFlag(HashEntryOption.Compressed))
			{
				using (InflaterInputStream inflaterInputStream = new InflaterInputStream(fileStream) { IsStreamOwner = false })
				{
					return await inflaterInputStream.ReadBytesAsync(hashEntry.Header.Size, cancellationToken).ConfigureAwait(false);
				}
			}
			else
			{
				return await fileStream.ReadBytesAsync(hashEntry.Header.Size, cancellationToken).ConfigureAwait(false);
			}
		}

		public async Task<string> ReadStringAsync(FileStream fileStream, HashEntry hashEntry, CancellationToken cancellationToken = default)
		{
			return Encoding.UTF8.GetString(await ReadAsync(fileStream, hashEntry, cancellationToken).ConfigureAwait(false));
		}
	}
}
