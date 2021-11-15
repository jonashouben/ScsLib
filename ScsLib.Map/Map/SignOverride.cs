using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class SignOverride
	{
		[BinaryPosition(0)]
		public uint Id { get; internal set; }

		[BinaryPosition(1)]
		public Token AreaName { get; internal set; } = default!;

		[BinaryPosition(2)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<SignOverrideAttribute> Attributes { get; internal set; } = Array.Empty<SignOverrideAttribute>();
	}
}
