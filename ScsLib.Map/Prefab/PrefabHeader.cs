using ScsLib.Map.Reader;

namespace ScsLib.Map.Prefab
{
	[BinarySerializable]
	public class PrefabHeader
	{
		[BinaryPosition(0)]
		public uint Version { get; internal set; }

		[BinaryPosition(1)]
		public uint NodeCount { get; internal set; }

		[BinaryPosition(2)]
		public uint NavigationCurveCount { get; internal set; }

		[BinaryPosition(3)]
		public uint SignCount { get; internal set; }

		[BinaryPosition(4)]
		public uint SemaphoreCount { get; internal set; }

		[BinaryPosition(5)]
		public uint SpawnPointCount { get; internal set; }

		[BinaryPosition(6)]
		public uint TerrainPointCount { get; internal set; }

		[BinaryPosition(7)]
		public uint TerrainPointVariantCount { get; internal set; }

		[BinaryPosition(8)]
		public uint MapPointCount { get; internal set; }

		[BinaryPosition(9)]
		public uint TriggerPointCount { get; internal set; }

		[BinaryPosition(10)]
		public uint IntersectionCount { get; internal set; }

		[BinaryPosition(11)]
		public uint NavigationNodeCount { get; internal set; }

		[BinaryPosition(12)]
		public uint NodeOffset { get; internal set; }

		[BinaryPosition(13)]
		public uint NavigationCurveOffset { get; internal set; }

		[BinaryPosition(14)]
		public uint SignOffset { get; internal set; }

		[BinaryPosition(15)]
		public uint SemaphoreOffset { get; internal set; }

		[BinaryPosition(16)]
		public uint SpawnPointOffset { get; internal set; }

		[BinaryPosition(17)]
		public uint TerrainPointPosOffset { get; internal set; }

		[BinaryPosition(18)]
		public uint TerrainPointNormalOffset { get; internal set; }

		[BinaryPosition(19)]
		public uint TerrainPointVariantOffset { get; internal set; }

		[BinaryPosition(20)]
		public uint MapPointOffset { get; internal set; }

		[BinaryPosition(21)]
		public uint TriggerPointOffset { get; internal set; }

		[BinaryPosition(22)]
		public uint IntersectionOffset { get; internal set; }

		[BinaryPosition(23)]
		public uint NavigationNodeOffset { get; internal set; }
	}
}
