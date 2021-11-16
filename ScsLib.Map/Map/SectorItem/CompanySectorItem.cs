using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class CompanySectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public Token Name { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 2)]
		public Token City { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 3)]
		public ulong PrefabUid { get; internal set; }

		[BinaryPosition(EndPosition + 4)]
		public ulong NodeUid { get; internal set; }

		[BinaryPosition(EndPosition + 5)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> UnloadEasyNodeUids { get; internal set; } = Array.Empty<ulong>();

		[BinaryPosition(EndPosition + 6)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> UnloadMediumNodeUids { get; internal set; } = Array.Empty<ulong>();

		[BinaryPosition(EndPosition + 7)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> UnloadHardNodeUids { get; internal set; } = Array.Empty<ulong>();

		[BinaryPosition(EndPosition + 8)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> TrailerSpawnNodeUids { get; internal set; } = Array.Empty<ulong>();

		[BinaryPosition(EndPosition + 9)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> NodeUids { get; internal set; } = Array.Empty<ulong>();

		[BinaryPosition(EndPosition + 10)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> LongTrailerSpawnNodeUids { get; internal set; } = Array.Empty<ulong>();
	}
}
