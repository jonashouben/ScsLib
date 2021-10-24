namespace ScsLib.HashFileSystem
{
	public interface IHashEntry
	{
		/// <summary>
		/// Path hashed with CityHash
		/// </summary>
		ulong Hash { get; }
	}
}
