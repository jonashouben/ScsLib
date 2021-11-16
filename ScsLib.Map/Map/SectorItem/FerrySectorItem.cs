using ScsLib.Reader;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class FerrySectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public Token Name { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 2)]
		public ulong PrefabLinkUid { get; internal set; }

		[BinaryPosition(EndPosition + 3)]
		public ulong NodeUid { get; internal set; }

		[BinaryPosition(EndPosition + 4)]
		public Float3 UnloadOffset { get; internal set; } = default!;
	}
}
