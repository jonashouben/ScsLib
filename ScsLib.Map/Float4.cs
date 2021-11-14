using ScsLib.Reader;

namespace ScsLib
{
	[BinarySerializable]
	public class Float4 : Float3
	{
		[BinaryPosition(3)]
		public float W { get; internal set; }
	}
}
