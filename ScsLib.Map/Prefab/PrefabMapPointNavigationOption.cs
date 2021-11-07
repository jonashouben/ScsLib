using System;

namespace ScsLib.Map.Prefab
{
	[Flags]
	public enum PrefabMapPointNavigationOption
	{
		NavigationNode0 = 0x00000001,
		NavigationNode1 = 0x00000002,
		NavigationNode2 = 0x00000004,
		NavigationNode3 = 0x00000008,
		NavigationNode4 = 0x00000010,
		NavigationNode5 = 0x00000020,
		NavigationNode6 = 0x00000040,
		NavigationNodeCustomTarget = 0x00000080,
		NavigationNodeAll = 0x000000FF,
		NavigationNodeMask = 0x000000FF,
		NavigationNodeStart = 0x00000100,
		NavigationBase = 0x00000200,
		PrefabExit = 0x00000400
	}
}
