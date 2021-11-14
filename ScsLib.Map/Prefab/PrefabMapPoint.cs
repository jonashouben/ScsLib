using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class PrefabMapPoint
	{
		[BinaryPosition(0)]
		public PrefabMapPointVisualOption VisualOptions { get; internal set; }

		[BinaryPosition(1)]
		public PrefabMapPointNavigationOption NavigationOptions { get; internal set; }

		[BinaryPosition(2)]
		public Float3 Position { get; internal set; } = default!;

		[BinaryPosition(3)]
		[BinaryFixedArray(6)]
		public IReadOnlyCollection<int> Neighbours { get; internal set; } = Array.Empty<int>();

		[BinaryPosition(4)]
		public int NeighbourCount { get; internal set; }
	}
}
