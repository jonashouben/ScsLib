namespace ScsLib.HashFileSystem
{
	public interface IHashDirectoryEntry
	{
		string Name { get; }
		HashDirectoryEntryType Type { get; }
	}
}
