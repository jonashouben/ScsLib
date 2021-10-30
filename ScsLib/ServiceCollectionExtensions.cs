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
			services.AddSingleton<IHashFsReader, HashFsReader>();
			services.AddSingleton<IHashEntryReader, HashEntryReader>();
			services.AddSingleton<IHashDirectoryReader, HashDirectoryReader>();
		}
	}
}
