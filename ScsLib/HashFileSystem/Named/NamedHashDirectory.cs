namespace ScsLib.HashFileSystem.Named
{
	public class NamedHashDirectory : HashDirectory, INamedHashEntry
	{
		public string VirtualPath { get; internal set; } = default!;
		public bool IsManual { get; internal set; }
	}
}
