namespace ScsLib.ThreeNK
{
	public class ThreeNKHeader
	{
		public const int HeaderSize = 6;

		public uint Signature { get; internal set; }
		public byte UnknownByte { get; internal set; }
		public byte Seed { get; internal set; }
	}
}
