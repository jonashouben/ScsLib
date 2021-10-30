using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

		public async Task<IReadOnlyCollection<HashDirectoryEntry>> ReadAsync(FileStream fileStream, HashDirectory hashDirectory, CancellationToken cancellationToken = default)
		{
			return await _ReadAsync(fileStream, hashDirectory, cancellationToken).ToArrayAsync(cancellationToken).ConfigureAwait(false);
		}

		private async IAsyncEnumerable<HashDirectoryEntry> _ReadAsync(FileStream fileStream, HashDirectory hashDirectory, [EnumeratorCancellation] CancellationToken cancellationToken)
		{
			foreach (ReadOnlySpan<char> content in (await _hashEntryReader.ReadStringAsync(fileStream, hashDirectory, cancellationToken).ConfigureAwait(false)).Split('\n'))
			{
				if (content.StartsWith("*", StringComparison.Ordinal))
				{
					yield return new HashDirectoryEntry
					{
						Type = HashDirectoryEntryType.Directory,
						Name = content.Slice(1).ToString()
					};
				}
			}
		}
	}
}
