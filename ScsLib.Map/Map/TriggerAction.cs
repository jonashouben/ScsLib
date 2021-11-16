using System;
using System.Collections.Generic;

namespace ScsLib.Map
{
	public class TriggerAction
	{
		public Token Name { get; internal set; } = default!;
		public IReadOnlyCollection<float> NumberParameters { get; internal set; } = Array.Empty<float>();
		public IReadOnlyCollection<string> StringParameters { get; internal set; } = Array.Empty<string>();
		public IReadOnlyCollection<Token> TargetTags { get; internal set; } = Array.Empty<Token>();
		public float TargetRange { get; internal set; }
		public uint Flags { get; internal set; }
	}
}
