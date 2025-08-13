using AleksandarNaumovic.EntryPoint.Commands;
using NSubstitute;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test
{
	[TestFixture]
	internal class EntryPointConfigurationTest
	{
		private ICommandRegistry registry;
		private EntryPointConfiguration config;

		[SetUp]
		public void SetUp()
		{
			registry = Substitute.For<ICommandRegistry>();
			
			config = new EntryPointConfiguration(registry);
		}

		[Test]
		public void TestConstructor()
		{
			Assert.AreEqual("EntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.", config.DefaultMessage);
			Assert.AreSame(registry, config.CommandRegistry);
		}
		
		[Test]
		public void TestDefaultMessage()
		{
			Assert.AreEqual("EntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.", config.DefaultMessage);
		}

		[Test]
		public void TestAddCommandsToTheRegistry()
		{
			// EntryPointConfiguration config = new EntryPointConfiguration();

			ICommand command1 = Substitute.For<ICommand>();
			ICommand command2 = Substitute.For<ICommand>();
			ICommand command3 = Substitute.For<ICommand>();
			
			string[] subcommands1 = ["do", "something"];
			string[] subcommands2 = ["do", "somethingelse"];
			string[] subcommands3 = ["dontdo", "something"];

			config.AddCommand(subcommands1, command1);
			config.AddCommand(subcommands2, command2);
			config.AddCommand(subcommands3, command3);
			
			registry.Received().Register(subcommands1, command1);
			registry.Received().Register(subcommands2, command2);
			registry.Received().Register(subcommands3, command3);
		}
	}
}
