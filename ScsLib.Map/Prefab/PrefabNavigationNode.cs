using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class PrefabNavigationNode
	{
		[BinaryPosition(0)]
		public PrefabNavigationNodeType Type { get; internal set; }

		[BinaryPosition(1)]
		public ushort Index { get; internal set; }

		[BinaryDynamicArray(TypeCode.Byte, 255)]
		[BinaryPosition(2)]
		public IReadOnlyCollection<PrefabNavigationNodeConnection> Connections { get; internal set; } = Array.Empty<PrefabNavigationNodeConnection>();
	}
}
