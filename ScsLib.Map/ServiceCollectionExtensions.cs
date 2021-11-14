using Microsoft.Extensions.DependencyInjection;
using ScsLib.Converter;
using ScsLib.Reader;

namespace ScsLib
{
	public static class ServiceCollectionExtensions
	{
		public static void AddScsMap(this IServiceCollection services)
		{
			services.AddSingleton<IBinarySerializer, BinarySerializer>();
			services.AddSingleton<ITokenConverter, TokenConverter>();
			services.AddSingleton<IPrefabReader, PrefabReader>();
			services.AddSingleton<IMbdReader, MbdReader>();
			services.AddSingleton<ISectorDescriptionReader, SectorDescriptionReader>();
			services.AddSingleton<ISectorItemReader, SectorItemReader>();
			services.AddSingleton<ISectorReader, SectorReader>();
		}
	}
}
