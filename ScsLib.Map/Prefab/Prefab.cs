using ScsLib.Map.Reader;
using System;
using System.Collections.Generic;

namespace ScsLib.Map.Prefab
{
	[BinarySerializable]
	public class Prefab
	{
		[BinaryPosition(0)]
		public uint Version { get; internal set; }

		public IReadOnlyCollection<PrefabNode> Nodes { get; internal set; } = Array.Empty<PrefabNode>();
		public IReadOnlyCollection<PrefabCurve> NavigationCurves { get; internal set; } = Array.Empty<PrefabCurve>();
		public IReadOnlyCollection<object> Signs { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> Semaphores { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> SpawnPoints { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> MapPoints { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> TriggerPoints { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> Intersections { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> NavigationNodes { get; internal set; } = Array.Empty<object>();
	}
}
