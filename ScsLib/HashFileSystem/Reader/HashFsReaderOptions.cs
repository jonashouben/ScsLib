using System;
using System.Collections.Generic;

namespace ScsLib.HashFileSystem.Reader
{
	public class HashFsReaderOptions
	{
		public IReadOnlyCollection<string> KnownDirectoryNames { get; set; } = Array.Empty<string>();
		public IReadOnlyCollection<string> KnownFileNames { get; set; } = Array.Empty<string>();
	}
}
