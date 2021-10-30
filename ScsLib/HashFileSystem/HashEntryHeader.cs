namespace ScsLib.HashFileSystem
{
	public class HashEntryHeader
	{
		public const int HeaderSize = 32;

		public ulong Hash { get; internal set; }
		public long Offset { get; internal set; }
		public HashEntryOption Options { get; internal set; }
		public uint Crc { get; internal set; }
		public int Size { get; internal set; }
		public int CompressedSize { get; internal set; }
	}
}
