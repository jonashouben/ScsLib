using System;
using System.Collections.Generic;

namespace ScsLib.Map.Prefab
{
	public class Prefab
	{
		public int Version { get; internal set; }
		public IReadOnlyCollection<object> Nodes { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> NavigationCurves { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> Signs { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> Semaphores { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> SpawnPoints { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> MapPoints { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> TriggerPoints { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> Intersections { get; internal set; } = Array.Empty<object>();
		public IReadOnlyCollection<object> NavigationNodes { get; internal set; } = Array.Empty<object>();
	}
}
