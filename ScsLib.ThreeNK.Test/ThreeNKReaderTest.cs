using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScsLib.ThreeNK.Test
{
	[TestClass]
	public class ThreeNKReaderTest : TestBase
	{
		[TestMethod]
		public async Task Read()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				await CreateTestFile(ms, new ThreeNKHeader
				{
					Signature = ThreeNKReader.Signature,
					UnknownByte = 0,
					Seed = 150
				}, new byte[] { 77, 94, 201, 199, 135, 181, 109, 89, 92, 4 }).ConfigureAwait(false);

				IThreeNKReader reader = ServiceProvider.GetRequiredService<IThreeNKReader>();

				byte[] data = await reader.ReadAsync(ms).ConfigureAwait(false);

				Assert.IsTrue(new byte[] { 83, 105, 105, 78, 117, 110, 105, 116, 10, 123 }.SequenceEqual(data));
			}
		}

		private async Task CreateTestFile(Stream stream, ThreeNKHeader header, byte[] data)
		{
			await stream.WriteAsync(BitConverter.GetBytes(header.Signature)).ConfigureAwait(false);
			await stream.WriteAsync(new byte[] { header.UnknownByte, header.Seed }).ConfigureAwait(false);
			await stream.WriteAsync(data).ConfigureAwait(false);
		}
	}
}
