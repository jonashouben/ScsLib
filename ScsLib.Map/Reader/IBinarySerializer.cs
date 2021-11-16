using System.Collections.Generic;
using System.IO;

namespace ScsLib.Reader
{
	public interface IBinarySerializer
	{
		T Deserialize<T>(BinaryReader reader);
		IEnumerable<T> DeserializeMany<T>(BinaryReader reader, uint count);
	}
}
