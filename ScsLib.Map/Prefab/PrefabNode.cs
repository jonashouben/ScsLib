using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class PrefabNode
	{
		[BinaryPosition(0)]
		public int TerrainPointIndex { get; internal set; }

		[BinaryPosition(1)]
		public int TerrainPointCount { get; internal set; }

		[BinaryPosition(2)]
		public int VariantIndex { get; internal set; }

		[BinaryPosition(3)]
		public int VariantCount { get; internal set; }

		[BinaryPosition(4)]
		public Float3 Position { get; internal set; } = default!;

		[BinaryPosition(5)]
		public Float3 Direction { get; internal set; } = default!;

		[BinaryPosition(6)]
		[BinaryFixedArray(8)]
		public IReadOnlyCollection<int> InputLines { get; internal set; } = Array.Empty<int>();

		[BinaryPosition(7)]
		[BinaryFixedArray(8)]
		public IReadOnlyCollection<int> OutputLines { get; internal set; } = Array.Empty<int>();
	}
}
