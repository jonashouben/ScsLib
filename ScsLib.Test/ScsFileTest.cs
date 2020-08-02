using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScsLib.Test
{
	[TestClass]
	public class ScsFileTest
	{
		[TestMethod]
		[Timeout(1000)]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task Read_ParameterNull()
		{
			Stream stream = null;
			await ScsFile.Read(stream).ConfigureAwait(false);
			Assert.Fail();
		}

		[TestMethod]
		[Timeout(1000)]
		[ExpectedException(typeof(InvalidOperationException))]
		public async Task Read_StreamNotReadable()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				ms.Close();

				using (ScsFile scsFile = await ScsFile.Read(ms).ConfigureAwait(false))
				{
					Assert.Fail();
				}
			}
		}

		[TestMethod]
		[Timeout(1000)]
		[ExpectedException(typeof(InvalidOperationException))]
		public async Task Read_StreamNotSeekable()
		{
			using (NotSeekableStream stream = new NotSeekableStream())
			{
				using (ScsFile scsFile = await ScsFile.Read(stream).ConfigureAwait(false))
				{
					Assert.Fail();
				}
			}
		}

		[TestMethod]
		[Timeout(1000)]
		[ExpectedException(typeof(FormatException))]
		public async Task Read_NoHeader()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (ScsFile scsFile = await ScsFile.Read(ms).ConfigureAwait(false))
				{
					Assert.Fail();
				}
			}
		}

		[TestMethod]
		[Timeout(1000)]
		[ExpectedException(typeof(NotSupportedException))]
		public async Task Read_InvalidMagic()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				await WriteHeader(ms, 0, 0, 0, 0, 0, 0).ConfigureAwait(false);
				ms.Seek(0, SeekOrigin.Begin);
				using (ScsFile scsFile = await ScsFile.Read(ms).ConfigureAwait(false))
				{
					Assert.Fail();
				}
			}
		}

		[TestMethod]
		[Timeout(1000)]
		[ExpectedException(typeof(NotSupportedException))]
		public async Task Read_InvalidHashMethod()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				await WriteHeader(ms, ScsFile.Magic, 0, 0, 0, 0, 0).ConfigureAwait(false);
				ms.Seek(0, SeekOrigin.Begin);
				using (ScsFile scsFile = await ScsFile.Read(ms).ConfigureAwait(false))
				{
					Assert.Fail();
				}
			}
		}

		[TestMethod]
		[Timeout(1000)]
		[ExpectedException(typeof(NotSupportedException))]
		public async Task Read_InvalidHashVersion()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				await WriteHeader(ms, ScsFile.Magic, 0, 0, ScsFile.CityHashMethod, 0, 0).ConfigureAwait(false);
				ms.Seek(0, SeekOrigin.Begin);
				using (ScsFile scsFile = await ScsFile.Read(ms).ConfigureAwait(false))
				{
					Assert.Fail();
				}
			}
		}

		[TestMethod]
		[Timeout(1000)]
		public async Task Read_NoEntries()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				await WriteHeader(ms, ScsFile.Magic, ScsFile.HashVersion, 0, ScsFile.CityHashMethod, 0, ScsFileHeader.HeaderSize).ConfigureAwait(false);
				ms.Seek(0, SeekOrigin.Begin);
				using (ScsFile scsFile = await ScsFile.Read(ms).ConfigureAwait(false))
				{
					Assert.AreEqual(0, scsFile.Entries.Count);
					Assert.IsNotNull(scsFile.RootDirectory);
					Assert.AreEqual(0, scsFile.RootDirectory.Entries.Count);
				}
			}
		}

		private static async Task WriteHeader(Stream stream, uint magic, ushort version, ushort salt, uint hashMethod, int entryCount, int startOffset)
		{
			await stream.WriteAsync(BitConverter.GetBytes(magic)).ConfigureAwait(false);
			await stream.WriteAsync(BitConverter.GetBytes(version)).ConfigureAwait(false);
			await stream.WriteAsync(BitConverter.GetBytes(salt)).ConfigureAwait(false);
			await stream.WriteAsync(BitConverter.GetBytes(hashMethod)).ConfigureAwait(false);
			await stream.WriteAsync(BitConverter.GetBytes(entryCount)).ConfigureAwait(false);
			await stream.WriteAsync(BitConverter.GetBytes(startOffset)).ConfigureAwait(false);
		}
	}
}
