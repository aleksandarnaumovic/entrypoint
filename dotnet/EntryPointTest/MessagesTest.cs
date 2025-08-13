using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test
{
	[TestFixture]
	internal class MessagesTest
	{
		[Test]
		public void TestDefaultMessage()
		{
			Assert.AreEqual("EntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.", Messages.DefaultMessage);
		}

		[Test]
		public void TestIncorrectSyntax()
		{
			Assert.AreEqual("Invalid command. Please try <subcommand> <subcommand> <subcommand> arguments.... form.", Messages.IncorrectSyntax);
		}
	}
}
