using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class GarageSectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public Token City { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 2)]
		public uint BuyMode { get; internal set; }

		[BinaryPosition(EndPosition + 3)]
		public ulong NodeUid { get; internal set; }

		[BinaryPosition(EndPosition + 4)]
		public ulong PrefabLinkUid { get; internal set; }

		[BinaryPosition(EndPosition + 5)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> NodeUids { get; internal set; } = Array.Empty<ulong>();
	}
}
