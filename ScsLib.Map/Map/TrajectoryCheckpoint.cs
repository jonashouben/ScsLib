using ScsLib.Reader;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class TrajectoryCheckpoint
	{
		[BinaryPosition(0)]
		public Token Route { get; internal set; } = default!;

		[BinaryPosition(1)]
		public Token Token { get; internal set; } = default!;
	}
}
