using ScsLib.Reader;

namespace ScsLib
{
	[BinarySerializable]
	public class Fixed2
	{
		[BinaryPosition(0)]
		public int X { get; internal set; }

		[BinaryPosition(1)]
		public int Z { get; internal set; }
	}
}
