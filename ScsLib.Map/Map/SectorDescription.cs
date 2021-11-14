using ScsLib.Reader;

namespace ScsLib.Map
{
	[BinarySerializable]
	public class SectorDescription
	{
		[BinaryPosition(0)]
		public uint Version { get; internal set; }

		[BinaryPosition(1)]
		public Fixed2 MinBoundary { get; internal set; } = default!;

		[BinaryPosition(2)]
		public Fixed2 MaxBoundary { get; internal set; } = default!;

		[BinaryPosition(3)]
		public uint Flags { get; internal set; }

		[BinaryPosition(4)]
		public Token ClimateProfile { get; internal set; } = default!;
	}
}
