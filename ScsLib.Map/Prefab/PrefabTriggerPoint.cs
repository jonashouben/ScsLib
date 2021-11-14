using ScsLib.Map.Reader;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class PrefabTriggerPoint
	{
		[BinaryPosition(0)]
		public uint TriggerId { get; internal set; }

		[BinaryPosition(1)]
		public Token Action { get; internal set; } = default!;

		[BinaryPosition(2)]
		public float Range { get; internal set; }

		[BinaryPosition(3)]
		public float ResetDelay { get; internal set; }

		[BinaryPosition(4)]
		public float ResetDistance { get; internal set; }

		[BinaryPosition(5)]
		public PrefabTriggerPointNavigationOption NavigationOptions { get; internal set; }

		[BinaryPosition(6)]
		public Float3 Position { get; internal set; } = default!;

		[BinaryPosition(7)]
		[BinaryFixedArray(2)]
		public IReadOnlyCollection<int> Neighbours { get; internal set; } = Array.Empty<int>();
	}
}
