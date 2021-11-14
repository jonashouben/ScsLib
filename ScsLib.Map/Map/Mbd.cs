using ScsLib.Reader;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class Mbd
	{
		[BinaryPosition(0)]
		public uint CoreMapVersion { get; internal set; }

		[BinaryPosition(1)]
		public Token GameId { get; internal set; } = default!;

		[BinaryPosition(2)]
		public uint GameMapVersion { get; internal set; }

		[BinaryPosition(3)]
		public ulong EditorMapId { get; internal set; }

		[BinaryPosition(4)]
		public Placement StartPlacement { get; internal set; } = default!;

		[BinaryPosition(5)]
		public uint GameTag { get; internal set; }

		[BinaryPosition(6)]
		public float TimeCompressionNormal { get; internal set; }

		[BinaryPosition(7)]
		public float TimeCompressionCity { get; internal set; }

		[BinaryPosition(8)]
		public byte EuropeMapUiCorrections { get; internal set; }
	}
}
