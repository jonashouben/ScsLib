using Microsoft.Extensions.DependencyInjection;
using ScsLib.HashFileSystem.Reader;
using ScsLib.Hashing;

namespace ScsLib
{
	public static class ServiceCollectionExtensions
	{
		public static void AddScsLib(this IServiceCollection services)
		{
			services.AddSingleton<ICityHash, CityHash>();
			services.AddSingleton<IHashFsHeaderReader, HashFsHeaderReader>();
			services.AddSingleton<IHashFsEntryHeaderReader, HashFsEntryHeaderReader>();
			services.Configure<HashFsReaderOptions>(opt =>
			{
				opt.KnownDirectoryNames = new[]
				{
					"automat",
					"contentbrowser",
					"custom",
					"dlc",
					"def",
					"effect",
					"font",
					"locale",
					"map",
					"material",
					"matlib",
					"model",
					"model2",
					"m0d3l",
					"panorama",
					"prefab",
					"prefab2",
					"road_template",
					"sound",
					"system",
					"ui",
					"uilab",
					"unit",
					"vehicle",
					"video"
				};

				opt.KnownFileNames = new[]
				{
					"base.cfg",
					"manifest.sii",
					"description.txt",
					"mod_icon.jpg"
				};
			});
			services.AddSingleton<IHashFsReader, HashFsReader>();
			services.AddSingleton<IHashEntryReader, HashEntryReader>();
			services.AddSingleton<IHashDirectoryReader, HashDirectoryReader>();
			services.AddSingleton<INamedHashDirectoryReader, NamedHashDirectoryReader>();
		}
	}
}
