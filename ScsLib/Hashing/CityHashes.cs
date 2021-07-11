using System.Collections.Generic;

namespace ScsLib.Hashing
{
	public static class CityHashes
	{
		public static readonly ulong RootEntry = CityHash.CityHash64("");

		internal static readonly IReadOnlyCollection<string> KnownDirectoryNames = new string[]
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

		internal static readonly IReadOnlyCollection<string> KnownFileNames = new string[]
		{
			"base.cfg"
		};
	}
}
