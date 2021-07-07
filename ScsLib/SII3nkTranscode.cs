using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ScsLib
{
	internal static class SII3nkTranscode
	{
		private static byte KeyTable(byte i)
		{
			return (byte) ((((i << 2) ^ ~i) << 3) ^ i);
		}

		private static IEnumerable<byte> _Transcode(Stream stream, byte seed)
		{
			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				for (int i = 0; stream.Position < stream.Length; i++)
				{
					yield return (byte) (reader.ReadByte() ^ KeyTable((byte)(seed + i)));
				}
			}
		}

		public static byte[] Transcode(Stream stream)
		{
			if (!stream.CanSeek) throw new InvalidOperationException("Stream not seekable!");

			SII3nkHeader header;

			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				header = new SII3nkHeader
				{
					Signature = reader.ReadUInt32(),
					UnkByte = reader.ReadByte(),
					Seed = reader.ReadByte()
				};
			}

			return _Transcode(stream, header.Seed).ToArray();
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
