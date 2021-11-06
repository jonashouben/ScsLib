using System;

namespace ScsLib.Map.Reader
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
