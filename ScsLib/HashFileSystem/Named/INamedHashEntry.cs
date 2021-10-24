namespace ScsLib.HashFileSystem.Named
{
	public interface INamedHashEntry : IHashEntry
	{
		string VirtualPath { get; }
	}
}
