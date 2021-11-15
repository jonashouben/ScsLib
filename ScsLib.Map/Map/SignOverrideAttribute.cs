namespace ScsLib.Map
{
	public class SignOverrideAttribute
	{
		public SignOverrideAttributeType Type { get; internal set; }
		public uint Index { get; internal set; }
		public object Value { get; internal set; } = default!;
	}
}
