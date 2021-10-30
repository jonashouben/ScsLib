namespace ScsLib.HashFileSystem
{
	public class HashEntry : IHashEntry
	{
		public HashEntryHeader Header { get; internal set; } = default!;
	}
}
