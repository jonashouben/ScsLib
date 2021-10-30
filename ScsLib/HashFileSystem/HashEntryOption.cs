using System;

namespace ScsLib.HashFileSystem
{
	[Flags]
	public enum HashEntryOption
	{
		Directory = 1,
		Compressed = 2,
		Verify = 4,
		Encrypted = 8
	}
}
