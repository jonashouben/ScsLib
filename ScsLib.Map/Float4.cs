using ScsLib.Map.Reader;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class Float4 : Float3
	{
		[BinaryPosition(3)]
		public float W { get; internal set; }
	}
}
