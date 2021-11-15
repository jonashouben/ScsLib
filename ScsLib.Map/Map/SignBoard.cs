using ScsLib.Reader;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class SignBoard
	{
		[BinaryPosition(0)]
		public Token Road { get; internal set; } = default!;

		[BinaryPosition(1)]
		public Token City1 { get; internal set; } = default!;

		[BinaryPosition(2)]
		public Token City2 { get; internal set; } = default!;
	}
}
