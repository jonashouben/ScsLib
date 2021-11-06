using ScsLib.Map.Reader;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class Quaternion
	{
		[BinaryPosition(0)]
		public float Heading { get; internal set; }

		[BinaryPosition(1)]
		public float Pitch { get; internal set; }

		[BinaryPosition(2)]
		public float Roll { get; internal set; }

		[BinaryPosition(3)]
		public int Padding { get; internal set; }
	}
}
