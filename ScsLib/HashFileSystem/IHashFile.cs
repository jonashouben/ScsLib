namespace ScsLib.HashFileSystem
{
	public interface IHashFile : IHashEntry
	{
		/// <summary>
		/// Size in bytes
		/// </summary>
		int Size { get; }
		bool IsCompressed { get; }
	}
}
