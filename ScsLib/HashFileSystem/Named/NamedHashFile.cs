namespace ScsLib.HashFileSystem.Named
{
	public class NamedHashFile : HashFile, INamedHashEntry
	{
		public string VirtualPath { get; internal set; } = default!;
	}
}
