using ScsLib.Reader;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class BusStopSectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public Token City { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 2)]
		public ulong PrefabUid { get; internal set; }

		[BinaryPosition(EndPosition + 3)]
		public ulong NodeUid { get; internal set; }
	}
}
