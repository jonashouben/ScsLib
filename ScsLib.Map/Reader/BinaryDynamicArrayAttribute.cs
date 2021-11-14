using System;

namespace ScsLib.Reader
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class BinaryDynamicArrayAttribute : Attribute
	{
		public Type LengthType { get; }
		public int FixedLength { get; }

		public BinaryDynamicArrayAttribute(Type lengthType, int fixedLength = 0)
		{
			LengthType = lengthType;
			FixedLength = fixedLength;
		}
	}
}
