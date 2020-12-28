using System;

namespace ScsLib.HashFileSystem
{
	[Flags]
	public enum HashEntryFlag
	{
		Directory = 1,
		Compressed = 2,
		Verify = 4,
		Encrypted = 8
	}
}
