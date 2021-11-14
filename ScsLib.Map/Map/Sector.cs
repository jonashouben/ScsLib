using ScsLib.Map.SectorItem;
using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class Sector
	{
		[BinaryPosition(0)]
		public uint CoreMapVersion { get; internal set; }

		[BinaryPosition(1)]
		public Token GameId { get; internal set; } = default!;

		[BinaryPosition(2)]
		public uint GameMapVersion { get; internal set; }

		[BinaryPosition(3)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<AbstractSectorItem> Items { get; internal set; } = Array.Empty<AbstractSectorItem>();

		[BinaryPosition(4)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<SectorNode> Nodes { get; internal set; } = Array.Empty<SectorNode>();
	}
}
