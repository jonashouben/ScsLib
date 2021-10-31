using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ScsLib.Test
{
	public abstract class TestBase
	{
		public IServiceProvider ServiceProvider { get; private set; } = default!;

		[TestInitialize]
		public void TestInit()
		{
			ServiceCollection services = new ServiceCollection();

			services.AddScsLib();

			ServiceProvider = services.BuildServiceProvider();
		}
	}
}
