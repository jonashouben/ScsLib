using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class CutPlaneSectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> NodeUids { get; internal set; } = Array.Empty<ulong>();
	}
}
