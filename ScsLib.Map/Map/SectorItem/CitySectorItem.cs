using ScsLib.Reader;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class CitySectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public Token Name { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 2)]
		public float Width { get; internal set; }

		[BinaryPosition(EndPosition + 3)]
		public float Height { get; internal set; }

		[BinaryPosition(EndPosition + 4)]
		public ulong NodeUid { get; internal set; }
	}
}
