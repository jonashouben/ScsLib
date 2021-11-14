using System;

namespace ScsLib.Reader
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class BinaryDynamicArrayAttribute : Attribute
	{
		public TypeCode LengthTypeCode { get; }
		public int FixedLength { get; }

		public BinaryDynamicArrayAttribute(TypeCode lengthTypeCode, int fixedLength = 0)
		{
			LengthTypeCode = lengthTypeCode;
			FixedLength = fixedLength;
		}
	}
}
