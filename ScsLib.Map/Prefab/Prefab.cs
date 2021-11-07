using ScsLib.Map.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.Prefab
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
		public IReadOnlyCollection<object> TriggerPoints { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> Intersections { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> NavigationNodes { get; internal set; } = Array.Empty<object>();
	}
}
