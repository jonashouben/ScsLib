using ScsLib.Reader;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class PrefabNode
	{
		[BinaryPosition(0)]
		public Token TerrainProfile { get; internal set; } = default!;

		[BinaryPosition(1)]
		public float TerrainProfileCoefficient { get; internal set; }
	}
}
