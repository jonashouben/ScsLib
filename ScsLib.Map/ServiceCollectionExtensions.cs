using Microsoft.Extensions.DependencyInjection;
using ScsLib.Map.Converter;
using ScsLib.Map.Reader;

namespace ScsLib.Map
{
	public static class ServiceCollectionExtensions
	{
		public static void AddScsMap(this IServiceCollection services)
		{
			services.AddSingleton<ITokenConverter, TokenConverter>();
			services.AddSingleton<IFloat3Reader, Float3Reader>();
			services.AddSingleton<IQuaternionReader, QuaternionReader>();
			services.AddSingleton<IPrefabNodeReader, PrefabNodeReader>();
			services.AddSingleton<IPrefabCurveReader, PrefabCurveReader>();
			services.AddSingleton<IPrefabReader, PrefabReader>();
		}
	}
}
