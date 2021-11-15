using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class SignSectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public Token Model { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 2)]
		public ulong NodeUid { get; internal set; }

		[BinaryPosition(EndPosition + 3)]
		public Token Look { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 4)]
		public Token Variant { get; internal set; } = default!;

		[BinaryPosition(EndPosition + 5)]
		[BinaryDynamicArray(typeof(byte))]
		public IReadOnlyCollection<SignBoard> Boards { get; internal set; } = Array.Empty<SignBoard>();

		[BinaryPosition(EndPosition + 6)]
		public string OverrideTemplate { get; internal set; } = default!;

		public IReadOnlyCollection<SignOverride> Overrides { get; internal set; } = Array.Empty<SignOverride>();
	}
}
