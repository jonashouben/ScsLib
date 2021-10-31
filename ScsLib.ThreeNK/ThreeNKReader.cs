using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.ThreeNK
{
	public class ThreeNKReader : IThreeNKReader
	{
		internal const uint Signature = 21720627;

		private readonly IThreeNKHeaderReader _headerReader;
		private readonly IReadOnlyDictionary<byte, byte> _decodingTable;

		public ThreeNKReader(IThreeNKHeaderReader headerReader)
		{
			_headerReader = headerReader;
			_decodingTable = Enumerable.Range(byte.MinValue, byte.MaxValue - byte.MinValue + 1).ToDictionary(row => (byte)row, row => KeyTable((byte)row));
		}

		public async Task<bool> HasSignatureAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			return (await _headerReader.ReadAsync(stream, cancellationToken).ConfigureAwait(false)).Signature == Signature;
		}

		public async Task<byte[]> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			ThreeNKHeader header = await _headerReader.ReadAsync(stream, cancellationToken).ConfigureAwait(false);

			if (header.Signature != Signature) throw new NotSupportedException($"Signature {header.Signature} not supported!");

			return _Read(stream, header).ToArray();
		}

		private IEnumerable<byte> _Read(Stream stream, ThreeNKHeader header)
		{
			stream.Seek(ThreeNKHeader.HeaderSize, SeekOrigin.Begin);

			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				for (int i = 0; stream.Position < stream.Length; i++)
				{
					yield return (byte)(reader.ReadByte() ^ _decodingTable[(byte)(header.Seed + i)]);
				}
			}
		}

		private static byte KeyTable(byte i)
		{
			return (byte)((((i << 2) ^ ~i) << 3) ^ i);
		}
	}
}
