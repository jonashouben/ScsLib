using System;
using System.Collections.Generic;

namespace ScsLib.Map.Prefab
{
	public class PrefabNode
	{
		public int TerrainPointIndex { get; internal set; }
		public int TerrainPointCount { get; internal set; }
		public int VariantIndex { get; internal set; }
		public int VariantCount { get; internal set; }
		public Float3 Position { get; internal set; } = default!;
		public Float3 Direction { get; internal set; } = default!;
		public IReadOnlyCollection<int> InputLines { get; internal set; } = Array.Empty<int>();
		public IReadOnlyCollection<int> OutputLines { get; internal set; } = Array.Empty<int>();
	}
}
