using System.Collections.Generic;
using System.IO;

namespace ScsLib.Reader
{
	public interface IBinarySerializer
	{
		T Deserialize<T>(BinaryReader reader) where T : new();
		IEnumerable<T> DeserializeMany<T>(BinaryReader reader, uint count) where T : new();
	}
}
