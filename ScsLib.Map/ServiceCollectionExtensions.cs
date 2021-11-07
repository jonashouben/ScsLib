using Microsoft.Extensions.DependencyInjection;
using ScsLib.Map.Converter;
using ScsLib.Map.Reader;

namespace ScsLib.Map
{
	public static class ServiceCollectionExtensions
	{
		public static void AddScsMap(this IServiceCollection services)
		{
			services.AddSingleton<IBinarySerializer, BinarySerializer>();
			services.AddSingleton<ITokenConverter, TokenConverter>();
			services.AddSingleton<IPrefabReader, PrefabReader>();
		}
	}
}
