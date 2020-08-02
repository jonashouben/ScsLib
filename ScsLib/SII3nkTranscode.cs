using AsyncBinaryExtensions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib
{
	internal static class SII3nkTranscode
	{
		private static byte KeyTable(byte i)
		{
			return (byte) ((((i << 2) ^ ~i) << 3) ^ i);
		}

		public static async Task<byte[]> Transcode(Stream stream, CancellationToken cancellationToken = default)
		{
			SII3nkHeader header = new SII3nkHeader
			{
				Signature = await stream.ReadUIntAsync(cancellationToken).ConfigureAwait(false),
				UnkByte = await stream.ReadByteAsync(cancellationToken).ConfigureAwait(false),
				Seed = await stream.ReadByteAsync(cancellationToken).ConfigureAwait(false)
			};

			byte[] data = await stream.ReadToEndAsync(1024, cancellationToken).ConfigureAwait(false);

			for (int i = 0; i < data.Length; i++)
			{
				data[i] = (byte) (data[i] ^ KeyTable((byte)(header.Seed + i)));
			}

			return data;
		}

		private class SII3nkHeader
		{
			internal const int HeaderSize = 6;

			public uint Signature { get; set; }
			public byte UnkByte { get; set; }
			public byte Seed { get; set; }
		}
	}
}
