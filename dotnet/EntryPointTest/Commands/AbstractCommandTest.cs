using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test.Commands
{
	[TestFixture]
	internal class AbstractCommandTest
	{
		private TestCommand command;

		[SetUp]
		public void SetUp()
		{
			command = new TestCommand();
		}

		[Test]
		public void TestAddParameters()
		{
			Assert.IsNull(command.GetParameters());

			IDictionary<string, string> parameters = new Dictionary<string, string>();

			command.AddParameters(parameters);

			Assert.AreSame(parameters, command.GetParameters());
		}

		[Test]
		public void TestResult()
		{
			Assert.IsNull(command.Result);

			command.SetResult("new result");

			Assert.AreEqual("new result", command.Result);
		}
	}
}
