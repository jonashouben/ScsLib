using System;

namespace ScsLib.Map.Prefab
{
	[Flags]
	public enum PrefabMapPointVisualOption
	{
		RoadSizeOneWay = 0x00000000,
		RoadSize1Lane = 0x00000100,
		RoadSize2Lane = 0x00000200,
		RoadSize3Lane = 0x00000300,
		RoadSize4Lane = 0x00000400,
		RoadSize2LaneSplit = 0x00000500,
		RoadSize3LaneSplit = 0x00000600,
		RoadSize4LaneSplit = 0x00000700,
		RoadSize3LaneOneWay = 0x00000800,
		RoadSizeManual = 0x00000D00,
		RoadSizeAuto = 0x00000E00,
		RoadSizeMask = 0x00000F00,
		RoadOffset0 = 0x00000000,
		RoadOffset1 = 0x00001000,
		RoadOffset2 = 0x00002000,
		RoadOffset5 = 0x00003000,
		RoadOffset10 = 0x00004000,
		RoadOffset15 = 0x00005000,
		RoadOffset20 = 0x00006000,
		RoadOffset25 = 0x00007000,
		RoadOffsetLane = 0x00008000,
		RoadOffsetMask = 0x0000F000,
		RoadExtValueMask = 0x000000FF,
		RoadOver = 0x00010000,
		CustomColor1 = 0x00020000,
		CustomColor2 = 0x00040000,
		CustomColor3 = 0x00080000,
		NoOutline = 0x00100000,
		NoArrow = 0x00200000
	}
}
