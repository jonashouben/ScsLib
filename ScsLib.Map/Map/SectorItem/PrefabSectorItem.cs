using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class PrefabSectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public Token Model { get; set; } = default!;

		[BinaryPosition(EndPosition + 2)]
		public Token Variant { get; set; } = default!;

		[BinaryPosition(EndPosition + 3)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<Token> AddParts { get; set; } = Array.Empty<Token>();

		[BinaryPosition(EndPosition + 4)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> NodeUids { get; set; } = Array.Empty<ulong>();

		[BinaryPosition(EndPosition + 5)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> SlaveUids { get; set; } = Array.Empty<ulong>();

		[BinaryPosition(EndPosition + 6)]
		public ulong FerryLinkUid { get; set; }

		[BinaryPosition(EndPosition + 7)]
		public short OriginIndex { get; set; }

		public IReadOnlyCollection<PrefabNode> Nodes { get; set; } = Array.Empty<PrefabNode>();
		public Token SemaphoreProfile { get; set; } = default!;
	}
}
