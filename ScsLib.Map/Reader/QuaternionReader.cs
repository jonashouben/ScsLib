using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public class QuaternionReader : IQuaternionReader
	{
		public ValueTask<Quaternion> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				return new ValueTask<Quaternion>(new Quaternion
				{
					Heading = reader.ReadSingle(),
					Pitch = reader.ReadSingle(),
					Roll = reader.ReadSingle(),
					Padding = reader.ReadInt32()
				});
			}
		}
	}
}
