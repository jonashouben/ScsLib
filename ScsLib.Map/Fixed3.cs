using ScsLib.Reader;

namespace ScsLib
{
	[BinarySerializable]
	public class Fixed3
	{
		[BinaryPosition(0)]
		public int X { get; internal set; }

		[BinaryPosition(1)]
		public int Y { get; internal set; }

		[BinaryPosition(2)]
		public int Z { get; internal set; }
	}
}
