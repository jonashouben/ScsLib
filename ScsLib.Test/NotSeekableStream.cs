using System.IO;

namespace ScsLib.Test
{
	public class NotSeekableStream : MemoryStream
	{
		public override bool CanSeek => false;
	}
}
