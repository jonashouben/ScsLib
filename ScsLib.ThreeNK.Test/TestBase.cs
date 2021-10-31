using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ScsLib.ThreeNK.Test
{
	public abstract class TestBase
	{
		public IServiceProvider ServiceProvider { get; private set; } = default!;

		[TestInitialize]
		public void TestInit()
		{
			ServiceCollection services = new ServiceCollection();

			services.AddThreeNK();

			ServiceProvider = services.BuildServiceProvider();
		}
	}
}
