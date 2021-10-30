﻿using AsyncBinaryExtensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public class HashFsEntryHeaderReader : IHashFsEntryHeaderReader
	{
		public async Task<IReadOnlyCollection<HashEntryHeader>> ReadAsync(FileStream fileStream, HashFsHeader hashFsHeader, CancellationToken cancellationToken = default)
		{
			return await _ReadAsync(fileStream, hashFsHeader, cancellationToken).ToArrayAsync(cancellationToken).ConfigureAwait(false);
		}

		private static async IAsyncEnumerable<HashEntryHeader> _ReadAsync(FileStream fileStream, HashFsHeader hashFsHeader, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			fileStream.Seek(hashFsHeader.StartOffset, SeekOrigin.Begin);

			byte[] buffer = await fileStream.ReadBytesAsync(HashEntryHeader.HeaderSize * hashFsHeader.EntryCount, cancellationToken).ConfigureAwait(false);

			using (MemoryStream ms = new MemoryStream(buffer))
			{
				using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8, true))
				{
					for (int i = 0; i < hashFsHeader.EntryCount; i++)
					{
						yield return new HashEntryHeader
						{
							Hash = reader.ReadUInt64(),
							Offset = reader.ReadInt64(),
							Options = (HashEntryOption)reader.ReadUInt32(),
							Crc = reader.ReadUInt32(),
							Size = reader.ReadInt32(),
							CompressedSize = reader.ReadInt32()
						};
					}
				}
			}
		}
	}
}
