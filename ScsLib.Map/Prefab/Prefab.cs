using ScsLib.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class Prefab
	{
		[BinaryPosition(0)]
		public PrefabHeader Header { get; internal set; } = default!;
		public IReadOnlyCollection<PrefabNode> Nodes { get; internal set; } = Array.Empty<PrefabNode>();
		public IReadOnlyCollection<PrefabNavigationCurve> NavigationCurves { get; internal set; } = Array.Empty<PrefabNavigationCurve>();
		public IReadOnlyCollection<PrefabSign> Signs { get; internal set; } = Array.Empty<PrefabSign>();
		public IReadOnlyCollection<PrefabSemaphore> Semaphores { get; internal set; } = Array.Empty<PrefabSemaphore>();
		public IReadOnlyCollection<PrefabSpawnPoint> SpawnPoints { get; internal set; } = Array.Empty<PrefabSpawnPoint>();
		public IReadOnlyCollection<PrefabMapPoint> MapPoints { get; internal set; } = Array.Empty<PrefabMapPoint>();
		public IReadOnlyCollection<PrefabTriggerPoint> TriggerPoints { get; internal set; } = Array.Empty<PrefabTriggerPoint>();
		public IReadOnlyCollection<PrefabIntersection> Intersections { get; internal set; } = Array.Empty<PrefabIntersection>();
		public IReadOnlyCollection<PrefabNavigationNode> NavigationNodes { get; internal set; } = Array.Empty<PrefabNavigationNode>();
	}
}
