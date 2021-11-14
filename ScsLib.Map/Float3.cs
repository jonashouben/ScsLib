using ScsLib.Map.Reader;

namespace ScsLib
{
	[BinarySerializable]
	public class Float3
	{
		[BinaryPosition(0)]
		public float X { get; internal set; }

		[BinaryPosition(1)]
		public float Y { get; internal set; }

		[BinaryPosition(2)]
		public float Z { get; internal set; }
	}
}
