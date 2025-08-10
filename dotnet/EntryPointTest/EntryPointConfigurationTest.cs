using AleksandarNaumovic.EntryPoint.Commands;
using NSubstitute;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test
{
	[TestFixture]
	internal class EntryPointConfigurationTest
	{
		private EntryPointConfiguration config;

		[SetUp]
		public void SetUp()
		{
			config = new EntryPointConfiguration();
		}
		
		[Test]
		public void TestDefaultMessage()
		{
			Assert.AreEqual("EntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.", config.DefaultMessage);
		}

		[Test]
		public void TestAddCommandsToTheRegistry()
		{
			EntryPointConfiguration config = new EntryPointConfiguration();

			ICommand command1 = Substitute.For<ICommand>();
			ICommand command2 = Substitute.For<ICommand>();
			ICommand command3 = Substitute.For<ICommand>();

			config.AddCommand(["do", "something"], command1);
			config.AddCommand(["do", "somethingelse"], command2);
			config.AddCommand(["dontdo", "something"], command3);

			Assert.AreSame(command1, config.CommandRegistry.Get(["do", "something"]));
			Assert.AreSame(command2, config.CommandRegistry.Get(["do", "somethingelse"]));
			Assert.AreSame(command3, config.CommandRegistry.Get(["dontdo", "something"]));
		}
	}
}
