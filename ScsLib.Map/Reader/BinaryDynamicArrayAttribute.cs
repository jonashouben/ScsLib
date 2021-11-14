using System;

namespace ScsLib.Map.Reader
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class BinaryDynamicArrayAttribute : Attribute
	{
		public TypeCode LengthTypeCode { get; }
		public int IgnoreLength { get; }

		public BinaryDynamicArrayAttribute(TypeCode lengthTypeCode, int ignoreLength = 0)
		{
			LengthTypeCode = lengthTypeCode;
			IgnoreLength = ignoreLength;
		}
	}
}
