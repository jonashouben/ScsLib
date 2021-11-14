using ScsLib.Map.Reader;

namespace ScsLib.Prefab
{
	[BinarySerializable]
	public class PrefabSpawnPoint
	{
		[BinaryPosition(0)]
		public Float3 Position { get; internal set; } = default!;

		[BinaryPosition(1)]
		public Quaternion Rotation { get; internal set; } = default!;

		[BinaryPosition(2)]
		public PrefabSpawnPointType Type { get; internal set; }
	}
}
