using System;

namespace ScsLib.Map.Prefab
{
	[Flags]
	public enum PrefabIntersectionOption
	{
		SiblingCountMask = 0x000000F0,
		SiblingCountShift = 4,
		TypeStart = 0x00010000,
		TypeEnd = 0x00020000,
		TypeCrossSharp = 0x00040000
	}
}
