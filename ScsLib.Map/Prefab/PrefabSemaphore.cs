using ScsLib.Map.Reader;

namespace ScsLib.Map.Prefab
{
	[BinarySerializable]
	public class PrefabSemaphore
	{
		[BinaryPosition(0)]
		public Float3 Position { get; internal set; } = default!;

		[BinaryPosition(1)]
		public Quaternion Rotation { get; internal set; } = default!;

		[BinaryPosition(2)]
		public PrefabSemaphoreType Type { get; internal set; }

		[BinaryPosition(3)]
		public uint SemaphoreId { get; internal set; }

		[BinaryPosition(4)]
		public Float4 Intervals { get; internal set; } = default!;

		[BinaryPosition(5)]
		public float Cycle { get; internal set; }

		[BinaryPosition(6)]
		public Token Profile { get; internal set; } = default!;

		[BinaryPosition(7)]
		public uint Flags { get; internal set; }
	}
}
