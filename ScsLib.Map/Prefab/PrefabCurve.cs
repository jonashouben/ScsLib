using System;
using System.Collections.Generic;

namespace ScsLib.Map.Prefab
{
	public class PrefabCurve
	{
		public Token Name { get; internal set; } = default!;
		public uint Flags { get; internal set; }
		public byte EndNode { get; internal set; }
		public byte EndLane { get; internal set; }
		public byte StartNode { get; internal set; }
		public byte StartLane { get; internal set; }
		public Float3 StartPosition { get; internal set; } = default!;
		public Float3 EndPosition { get; internal set; } = default!;
		public Quaternion StartRotation { get; internal set; } = default!;
		public Quaternion EndRotation { get; internal set; } = default!;
		public float Length { get; internal set; }
		public IReadOnlyCollection<int> NextLines { get; internal set; } = Array.Empty<int>();
		public IReadOnlyCollection<int> PreviousLines { get; internal set; } = Array.Empty<int>();
		public uint CountNext { get; internal set; }
		public uint CountPrevious { get; internal set; }
		public int SemaphoreId { get; internal set; }
		public Token TrafficRule { get; internal set; } = default!;
		public uint NavigationNodeId { get; internal set; }
	}
}
