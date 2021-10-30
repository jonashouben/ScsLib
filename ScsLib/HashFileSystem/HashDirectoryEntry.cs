namespace ScsLib.HashFileSystem
{
	public class HashDirectoryEntry
	{
		public string Name { get; internal set; } = default!;
		public HashDirectoryEntryType Type { get; internal set; }
	}
}
