using System;

namespace ScsLib.HashFileSystem
{
	public sealed class HashEntryHeader : IEquatable<HashEntryHeader>
	{
		internal const int HeaderSize = 32;

		public ulong Hash { get; internal set; }
		public long Offset { get; internal set; }
		public HashEntryFlag Flags { get; internal set; }
		public uint Crc { get; internal set; }
		public int Size { get; internal set; }
		public int CompressedSize { get; internal set; }

		internal HashEntryHeader()
		{
		}

		public bool Equals(HashEntryHeader? other)
		{
			return other?.Hash == Hash;
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as HashEntryHeader);
		}

		public override int GetHashCode()
		{
			return Hash.GetHashCode();
		}
	}
}
