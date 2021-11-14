using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class PrefabNavigationNodeConnection
	{
		[BinaryPosition(0)]
		public ushort TargetNode { get; internal set; }

		[BinaryPosition(1)]
		public float Length { get; internal set; }

		[BinaryPosition(2)]
		[BinaryDynamicArray(typeof(byte), 8)]
		public IReadOnlyCollection<ushort> Curves { get; internal set; } = Array.Empty<ushort>();
	}
}
