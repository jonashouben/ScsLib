using ScsLib.Reader;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class CutsceneAction
	{
		[BinaryPosition(0)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<uint> NumberParameters { get; internal set; } = Array.Empty<uint>();

		[BinaryPosition(1)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<string> StringParameters { get; internal set; } = Array.Empty<string>();

		[BinaryPosition(2)]
		[BinaryDynamicArray(typeof(uint))]
		public IReadOnlyCollection<Token> TargetTags { get; internal set; } = Array.Empty<Token>();

		[BinaryPosition(3)]
		public float TargetRange { get; internal set; }

		[BinaryPosition(4)]
		public uint Flags { get; internal set; }
	}
}
