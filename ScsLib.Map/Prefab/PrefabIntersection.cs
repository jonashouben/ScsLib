using ScsLib.Reader;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class PrefabIntersection
	{
		[BinaryPosition(0)]
		public uint CurveId { get; internal set; }

		[BinaryPosition(1)]
		public float Position { get; internal set; }

		[BinaryPosition(2)]
		public float Radius { get; internal set; }

		[BinaryPosition(3)]
		public PrefabIntersectionOption Options { get; internal set; }
	}
}
