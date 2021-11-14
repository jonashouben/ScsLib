using ScsLib.Reader;

namespace ScsLib
{
	[BinarySerializable]
	public class Placement
	{
		[BinaryPosition(0)]
		public Float4 Position { get; internal set; } = default!;

		[BinaryPosition(1)]
		public Quaternion Rotation { get; internal set; } = default!;
	}
}
