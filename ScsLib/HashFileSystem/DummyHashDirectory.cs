using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem
{
	public sealed class DummyHashDirectory : HashDirectory
	{
		private readonly ICollection<string> _entries = new List<string>();
		public override HashEntryHeader Header { get => throw new NotSupportedException(); internal set => throw new NotSupportedException(); }

		internal DummyHashDirectory()
		{
		}

		internal void AddDirectory(string directoryName)
		{
			_entries.Add("*" + directoryName);
		}

		internal void AddFile(string fileName)
		{
			_entries.Add(fileName);
		}

		internal override ValueTask<byte[]> ReadBytes(Stream stream, CancellationToken cancellationToken = default)
		{
			return new ValueTask<byte[]>(Encoding.UTF8.GetBytes(string.Join('\n', _entries)));
		}
	}
}
