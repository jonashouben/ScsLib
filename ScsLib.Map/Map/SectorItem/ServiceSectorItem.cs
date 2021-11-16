using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class ServiceSectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public ulong NodeUid { get; internal set; }

		[BinaryPosition(EndPosition + 2)]
		public ulong PrefabLinkUid { get; internal set; }

		[BinaryPosition(EndPosition + 3)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<ulong> NodeUids { get; internal set; } = Array.Empty<ulong>();
	}
}
