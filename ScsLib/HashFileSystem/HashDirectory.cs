using System;
using System.Collections.Generic;

namespace ScsLib.HashFileSystem
{
	public class HashDirectory : HashEntry
	{
		public IReadOnlyCollection<HashEntry> Entries { get; internal set; } = Array.Empty<HashEntry>();

		internal HashDirectory()
		{
		}
	}
}
