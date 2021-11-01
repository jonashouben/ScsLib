using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.HashFileSystem.Reader
{
	public class HashDirectoryReader : IHashDirectoryReader
	{
		private readonly IHashEntryReader _hashEntryReader;

		public HashDirectoryReader(IHashEntryReader hashEntryReader)
		{
			_hashEntryReader = hashEntryReader;
		}

		public async Task<IReadOnlyCollection<HashDirectoryEntry>> ReadAsync(Stream stream, HashDirectory hashDirectory, CancellationToken cancellationToken = default)
		{
			string directoryString = await _hashEntryReader.ReadStringAsync(stream, hashDirectory, cancellationToken).ConfigureAwait(false);

			return _Read(directoryString);
		}

		private IReadOnlyCollection<HashDirectoryEntry> _Read(ReadOnlySpan<char> directoryString)
		{
			List<HashDirectoryEntry> entries = new List<HashDirectoryEntry>();

			foreach (ReadOnlySpan<char> content in new LineSplitEnumerator(directoryString))
			{
				if (content.StartsWith("*", StringComparison.Ordinal))
				{
					entries.Add(new HashDirectoryEntry
					{
						Type = HashDirectoryEntryType.Directory,
						Name = content.Slice(1).ToString()
					});
				}
				else
				{
					entries.Add(new HashDirectoryEntry
					{
						Type = HashDirectoryEntryType.File,
						Name = content.ToString()
					});
				}
			}

			return entries;
		}

		private ref struct LineSplitEnumerator
		{
			private readonly ReadOnlySpan<char> _input;
			private int _pos;

			public ReadOnlySpan<char> Current { get; private set; }

			public LineSplitEnumerator(ReadOnlySpan<char> input)
			{
				_input = input;
				_pos = 0;
				Current = default;
			}

			public LineSplitEnumerator GetEnumerator() => this;

			public bool MoveNext()
			{
				if (_pos >= _input.Length) return false;

				int startPos = _pos;

				while (_pos < _input.Length && _input[_pos] != '\n')
				{
					_pos++;
				}

				Current = _input.Slice(startPos, _pos - startPos);
				_pos++;
				return true;
			}
		}
	}
}
