using AsyncBinaryExtensions;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem
{
	public abstract class HashEntry : IEquatable<HashEntry>
	{
		public virtual HashEntryHeader Header { get; internal set; }
		public string VirtualPath { get; internal set; }
		public string Path => VirtualPath.Replace('/', System.IO.Path.DirectorySeparatorChar);

		internal HashEntry()
		{
		}

		public bool Equals(HashEntry other)
		{
			return other?.Header?.Equals(Header) == true;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as HashEntry);
		}

		public override int GetHashCode()
		{
			return Header.GetHashCode();
		}

		internal virtual async ValueTask<byte[]> ReadBytes(Stream stream, CancellationToken cancellationToken = default)
		{
			stream.Seek(Header.Offset, SeekOrigin.Begin);

			if (Header.IsCompressed)
			{
				using (InflaterInputStream inflater = new InflaterInputStream(stream) { IsStreamOwner = false })
				{
					return await inflater.ReadBytesAsync(Header.Size, cancellationToken).ConfigureAwait(false);
				}
			}
			else
			{
				return await stream.ReadBytesAsync(Header.Size, cancellationToken).ConfigureAwait(false);
			}
		}

		internal async ValueTask<string> ReadString(Stream stream, CancellationToken cancellationToken = default)
		{
			return Encoding.UTF8.GetString(await ReadBytes(stream, cancellationToken).ConfigureAwait(false));
		}
	}
}
