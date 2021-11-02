using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public class Float3Reader : IFloat3Reader
	{
		public ValueTask<Float3> ReadAsync(Stream stream, CancellationToken cancellationToken = default)
		{
			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				return new ValueTask<Float3>(new Float3
				{
					X = reader.ReadSingle(),
					Y = reader.ReadSingle(),
					Z = reader.ReadSingle()
				});
			}
		}
	}
}
