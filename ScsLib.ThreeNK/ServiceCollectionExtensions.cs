using Microsoft.Extensions.DependencyInjection;

namespace ScsLib.ThreeNK
{
	public static class ServiceCollectionExtensions
	{
		public static void AddThreeNK(this IServiceCollection services)
		{
			services.AddSingleton<IThreeNKHeaderReader, ThreeNKHeaderReader>();
			services.AddSingleton<IThreeNKReader, ThreeNKReader>();
		}
	}
}
