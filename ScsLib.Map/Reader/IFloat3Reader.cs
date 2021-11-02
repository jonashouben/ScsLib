using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScsLib.Map.Reader
{
	public interface IFloat3Reader
	{
		ValueTask<Float3> ReadAsync(Stream stream, CancellationToken cancellationToken = default);
	}
}
