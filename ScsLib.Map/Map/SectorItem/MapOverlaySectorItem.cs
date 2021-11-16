using ScsLib.Reader;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class MapOverlaySectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public Token Look { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 2)]
		public ulong NodeUid { get; internal set; }
	}
}
