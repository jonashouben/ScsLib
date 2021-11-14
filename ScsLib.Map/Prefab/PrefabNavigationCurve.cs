using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class PrefabNavigationCurve
	{
		[BinaryPosition(0)]
		public Token Name { get; internal set; } = default!;

		[BinaryPosition(1)]
		public PrefabNavigationCurveOption Options { get; internal set; }

		[BinaryPosition(2)]
		public byte EndNode { get; internal set; }

		[BinaryPosition(3)]
		public byte EndLane { get; internal set; }

		[BinaryPosition(4)]
		public byte StartNode { get; internal set; }

		[BinaryPosition(5)]
		public byte StartLane { get; internal set; }

		[BinaryPosition(6)]
		public Float3 StartPosition { get; internal set; } = default!;

		[BinaryPosition(7)]
		public Float3 EndPosition { get; internal set; } = default!;

		[BinaryPosition(8)]
		public Quaternion StartRotation { get; internal set; } = default!;

		[BinaryPosition(9)]
		public Quaternion EndRotation { get; internal set; } = default!;

		[BinaryPosition(10)]
		public float Length { get; internal set; }

		[BinaryPosition(11)]
		[BinaryFixedArray(4)]
		public IReadOnlyCollection<int> NextLines { get; internal set; } = Array.Empty<int>();

		[BinaryPosition(12)]
		[BinaryFixedArray(4)]
		public IReadOnlyCollection<int> PreviousLines { get; internal set; } = Array.Empty<int>();

		[BinaryPosition(13)]
		public uint CountNext { get; internal set; }

		[BinaryPosition(14)]
		public uint CountPrevious { get; internal set; }

		[BinaryPosition(15)]
		public int SemaphoreId { get; internal set; }

		[BinaryPosition(16)]
		public Token TrafficRule { get; internal set; } = default!;

		[BinaryPosition(17)]
		public uint NavigationNodeId { get; internal set; }
	}
}
