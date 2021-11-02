using Microsoft.Extensions.DependencyInjection;
using ScsLib.Map.Reader;

namespace ScsLib.Map
{
	public static class ServiceCollectionExtensions
	{
		public static void AddScsMap(this IServiceCollection services)
		{
			services.AddSingleton<IFloat3Reader, Float3Reader>();
			services.AddSingleton<IPrefabNodeReader, PrefabNodeReader>();
			services.AddSingleton<IPrefabReader, PrefabReader>();
		}
	}
}
