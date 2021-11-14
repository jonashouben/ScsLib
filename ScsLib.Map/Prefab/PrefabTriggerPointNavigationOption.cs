using System;

namespace ScsLib.Prefab
{
	[Flags]
	public enum PrefabTriggerPointNavigationOption
	{
		Manual = 0x0001,
		Sphere = 0x0002,
		Partial = 0x0004,
		OneTime = 0x0008
	}
}
