using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class CutsceneSectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<Token> Tags { get; internal set; } = Array.Empty<Token>();

		[BinaryPosition(EndPosition + 2)]
		public ulong NodeUid { get; internal set; }

		[BinaryPosition(EndPosition + 3)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<CutsceneAction> Actions { get; internal set; } = Array.Empty<CutsceneAction>();
	}
}
