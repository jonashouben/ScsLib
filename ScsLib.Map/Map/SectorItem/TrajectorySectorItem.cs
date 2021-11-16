using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class TrajectorySectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> NodeUids { get; internal set; } = Array.Empty<ulong>();

		[BinaryPosition(EndPosition + 2)]
		public uint NavigationFlags { get; internal set; }

		[BinaryPosition(EndPosition + 3)]
		public Token AccessRule { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 4)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<TrajectoryRule> Rules { get; internal set; } = Array.Empty<TrajectoryRule>();

		[BinaryPosition(EndPosition + 5)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<TrajectoryCheckpoint> Checkpoints { get; internal set; } = Array.Empty<TrajectoryCheckpoint>();

		[BinaryPosition(EndPosition + 6)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<Token> Tags { get; internal set; } = Array.Empty<Token>();
	}
}
