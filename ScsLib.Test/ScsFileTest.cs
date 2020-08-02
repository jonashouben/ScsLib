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
	}
}
