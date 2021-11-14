using ScsLib.Reader;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class SectorNode
	{
		[BinaryPosition(0)]
		public ulong Uid { get; internal set; }

		[BinaryPosition(1)]
		public Fixed3 Position { get; internal set; } = default!;

		[BinaryPosition(2)]
		public Quaternion Rotation { get; internal set; } = default!;

		[BinaryPosition(3)]
		public ulong BackwardItemUid { get; internal set; }

		[BinaryPosition(4)]
		public ulong ForwardItemUid { get; internal set; }

		[BinaryPosition(5)]
		public uint Flags { get; internal set; }
	}
}
