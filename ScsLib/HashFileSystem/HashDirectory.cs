using System.Collections.Generic;

namespace ScsLib.HashFileSystem
{
	public class HashDirectory : HashEntry
	{
		public IReadOnlyCollection<HashEntry> Entries { get; internal set; }

		internal HashDirectory()
		{
		}
	}
}
