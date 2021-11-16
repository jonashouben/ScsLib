using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class TriggerSectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<Token> Tags { get; internal set; } = Array.Empty<Token>();

		[BinaryPosition(EndPosition + 2)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> NodeUids { get; internal set; } = Array.Empty<ulong>();

		[BinaryPosition(EndPosition + 3)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<TriggerAction> Actions { get; internal set; } = Array.Empty<TriggerAction>();

		public float Range { get; internal set; }
	}
}
