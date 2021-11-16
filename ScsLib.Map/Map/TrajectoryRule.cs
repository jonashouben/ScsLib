using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class TrajectoryRule
	{
		[BinaryPosition(0)]
		public uint NodeIndex { get; internal set; }

		[BinaryPosition(1)]
		public Token Rule { get; internal set; } = default!;

		[BinaryPosition(2)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<float> Parameters { get; internal set; } = Array.Empty<float>();
	}
}
