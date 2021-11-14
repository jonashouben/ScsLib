namespace ScsLib.Converter
{
	public interface ITokenConverter
	{
		Token FromToken(ulong token);
		Token FromString(string stringValue);
	}
}
