﻿using ScsLib.Reader;

namespace ScsLib.Map.SectorItem
{
	[BinarySerializable]
	public class RoadSectorItem : AbstractSectorItem
	{
		[BinaryPosition(EndPosition + 1)]
		public uint RoadFlags { get; set; }

		[BinaryPosition(EndPosition + 2)]
		public Token Look { get; set; } = default!;

		[BinaryPosition(EndPosition + 3)]
		public Token RightTemplateVariant { get; set; } = default!;

		[BinaryPosition(EndPosition + 4)]
		public Token LeftTemplateVariant { get; set; } = default!;

		[BinaryPosition(EndPosition + 5)]
		public Token RightEdgeRight { get; set; } = default!;

		[BinaryPosition(EndPosition + 6)]
		public Token RightEdgeLeft { get; set; } = default!;

		[BinaryPosition(EndPosition + 7)]
		public Token LeftEdgeRight { get; set; } = default!;

		[BinaryPosition(EndPosition + 8)]
		public Token LeftEdgeLeft { get; set; } = default!;

		[BinaryPosition(EndPosition + 9)]
		public Token RightProfile { get; set; } = default!;

		[BinaryPosition(EndPosition + 10)]
		public float RightProfileCoefficient { get; set; }

		[BinaryPosition(EndPosition + 11)]
		public Token LeftProfile { get; set; } = default!;

		[BinaryPosition(EndPosition + 12)]
		public float LeftProfileCoefficient { get; set; }

		[BinaryPosition(EndPosition + 13)]
		public Token RightTemplateLook { get; set; } = default!;

		[BinaryPosition(EndPosition + 14)]
		public Token LeftTemplateLook { get; set; } = default!;

		[BinaryPosition(EndPosition + 15)]
		public Token Material { get; set; } = default!;

		[BinaryPosition(EndPosition + 16)]
		public Token RightRailing { get; set; } = default!;

		[BinaryPosition(EndPosition + 17)]
		public short RightRailingOffset { get; set; }

		[BinaryPosition(EndPosition + 18)]
		public Token LeftRailing { get; set; } = default!;

		[BinaryPosition(EndPosition + 19)]
		public short LeftRailingOffset { get; set; }

		[BinaryPosition(EndPosition + 20)]
		public Token RightRailing1 { get; set; } = default!;

		[BinaryPosition(EndPosition + 21)]
		public short RightRailingOffset1 { get; set; }

		[BinaryPosition(EndPosition + 22)]
		public Token LeftRailing1 { get; set; } = default!;

		[BinaryPosition(EndPosition + 23)]
		public short LeftRailingOffset1 { get; set; }

		[BinaryPosition(EndPosition + 24)]
		public Token RightRailing2 { get; set; } = default!;

		[BinaryPosition(EndPosition + 25)]
		public short RightRailingOffset2 { get; set; }

		[BinaryPosition(EndPosition + 26)]
		public Token LeftRailing2 { get; set; } = default!;

		[BinaryPosition(EndPosition + 27)]
		public short LeftRailingOffset2 { get; set; }

		[BinaryPosition(EndPosition + 28)]
		public int RightRoadHeight { get; set; }

		[BinaryPosition(EndPosition + 29)]
		public int LeftRoadHeight { get; set; }

		[BinaryPosition(EndPosition + 30)]
		public ulong Node0Uid { get; set; }

		[BinaryPosition(EndPosition + 31)]
		public ulong Node1Uid { get; set; }

		[BinaryPosition(EndPosition + 32)]
		public float Length { get; set; }
	}
}
