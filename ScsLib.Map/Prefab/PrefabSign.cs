using ScsLib.Map.Reader;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class PrefabSign
	{
		[BinaryPosition(0)]
		public Token Name { get; internal set; } = default!;

		[BinaryPosition(1)]
		public Float3 Position { get; internal set; } = default!;

		[BinaryPosition(2)]
		public Quaternion Rotation { get; internal set; } = default!;

		[BinaryPosition(3)]
		public Token Model { get; internal set; } = default!;

		[BinaryPosition(4)]
		public Token Part { get; internal set; } = default!;
	}
}
