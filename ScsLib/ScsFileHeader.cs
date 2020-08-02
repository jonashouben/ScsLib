namespace ScsLib
{
	public sealed class ScsFileHeader
	{
		internal const int HeaderSize = 20;

		public uint Magic { get; internal set; }
		public ushort Version { get; internal set; }
		public ushort Salt { get; internal set; }
		public uint HashMethod { get; internal set; }
		public int EntryCount { get; internal set; }
		public int StartOffset { get; internal set; }

		internal ScsFileHeader()
		{
		}
	}
}
