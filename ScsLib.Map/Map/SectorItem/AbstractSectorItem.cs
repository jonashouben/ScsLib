using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	public abstract class AbstractSectorItem
	{
		public const int EndPosition = 4;

		[BinaryPosition(0)]
		public ulong Uid { get; internal set; }

		[BinaryPosition(1)]
		[BinaryFixedArray(5)]
		public IReadOnlyCollection<float> Minimums { get; internal set; } = Array.Empty<float>();

		[BinaryPosition(2)]
		[BinaryFixedArray(5)]
		public IReadOnlyCollection<float> Maximums { get; internal set; } = Array.Empty<float>();

		[BinaryPosition(3)]
		public uint Flags { get; internal set; }

		[BinaryPosition(4)]
		public byte ViewDistance { get; internal set; }
	}
}
