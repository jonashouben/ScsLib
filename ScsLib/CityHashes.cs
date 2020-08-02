using System.Collections.Generic;

namespace ScsLib
{
	public static class CityHashes
	{
		public static readonly ulong RootEntry = CityHash.CityHash64("");

		public static readonly IReadOnlyCollection<string> KnownDirectoryNames = new string[]
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

		public static readonly IReadOnlyCollection<string> KnownFileNames = new string[]
		{
			"base.cfg"
		};
	}
}
