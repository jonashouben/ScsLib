using System;

namespace ScsLib.Map.Prefab
{
	[Flags]
	public enum PrefabNavigationCurveOption
	{
		ForceNoBlinker = 0x00000004,
		RightBlinker = 0x00000008,
		LeftBlinker = 0x00000010,
		SmallVehicles = 0x00000020,
		LargeVehicles = 0x00000040,
		AllowedVehiclesMask = SmallVehicles | LargeVehicles,
		PriorityMask = 0x000F0000,
		PriorityShift = 16,
		LowProbability = 0x00002000,
		LimitDisplacement = 0x00004000,
		AdditivePriority = 0x00008000,
		StartNavigationPoint = ForceNoBlinker | RightBlinker | LeftBlinker | PriorityMask | AdditivePriority | LimitDisplacement,
		EndNavigationPoint = AllowedVehiclesMask | LowProbability
	}
}
