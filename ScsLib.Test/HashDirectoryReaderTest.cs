using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScsLib.HashFileSystem;
using ScsLib.HashFileSystem.Reader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScsLib.Test
{
	[TestClass]
	public class HashDirectoryReaderTest : TestBase
	{
		[TestMethod]
		public async Task Read_NoEntries()
		{
			await Read(Array.Empty<HashDirectoryEntry>()).ConfigureAwait(false);
		}

		[TestMethod]
		public async Task Read_Correct()
		{
			await Read(new[] { new HashDirectoryEntry { Name = "dir", Type = HashDirectoryEntryType.Directory } }).ConfigureAwait(false);
			await Read(new[] { new HashDirectoryEntry { Name = "file", Type = HashDirectoryEntryType.File } }).ConfigureAwait(false);
			await Read(new[] { new HashDirectoryEntry { Name = "dir", Type = HashDirectoryEntryType.Directory }, new HashDirectoryEntry { Name = "file", Type = HashDirectoryEntryType.File } }).ConfigureAwait(false);
		}

		private async Task Read(IReadOnlyCollection<HashDirectoryEntry> entries)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				HashDirectory directory = await CreateTestDirectory(ms, entries).ConfigureAwait(false);

				IHashDirectoryReader reader = ServiceProvider.GetRequiredService<IHashDirectoryReader>();

				IReadOnlyCollection<HashDirectoryEntry> resultEntries = await reader.ReadAsync(ms, directory).ConfigureAwait(false);

				Assert.AreEqual(entries.Count, resultEntries.Count);

				foreach (HashDirectoryEntry entry in entries)
				{
					HashDirectoryEntry? resultEntry = resultEntries.FirstOrDefault(row => row.Name == entry.Name);

					Assert.IsNotNull(resultEntry);
					Assert.AreEqual(entry.Type, resultEntry.Type);
				}
			}
		}

		private async Task<HashDirectory> CreateTestDirectory(Stream stream, IReadOnlyCollection<HashDirectoryEntry> entries)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(string.Join('\n', entries.Select(row =>
			{
				switch (row.Type)
				{
					case HashDirectoryEntryType.Directory:
						return $"*{row.Name}";
					default:
						return row.Name;
				}
			})));

			await stream.WriteAsync(bytes).ConfigureAwait(false);

			return new HashDirectory
			{
				Header = new HashEntryHeader
				{
					Size = bytes.Length
				}
			};
		}
	}
}
