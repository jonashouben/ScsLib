using System;

namespace ScsLib.Reader
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class BinaryPositionAttribute : Attribute
	{
		public int Position { get; }

		public BinaryPositionAttribute(int position)
		{
			Position = position;
		}
	}
}
