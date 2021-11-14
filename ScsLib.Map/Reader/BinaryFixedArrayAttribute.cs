using System;

namespace ScsLib.Reader
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class BinaryFixedArrayAttribute : Attribute
	{
		public int Length { get; }

		public BinaryFixedArrayAttribute(int length)
		{
			Length = length;
		}
	}
}
