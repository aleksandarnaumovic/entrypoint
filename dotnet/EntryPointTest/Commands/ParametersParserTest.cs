using AleksandarNaumovic.EntryPoint.Commands;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test.Commands
{
	[TestFixture]
	internal class ParametersParserTest
	{
		private IParametersParser parser;

		[SetUp]
		public void SetUp()
		{
			parser = new ParametersParser();
		}

		[Test]
		public void TestParse()
		{
			IList<ParameterInfo> info = new List<ParameterInfo>();

			info.Add(new ParameterInfo("username", true));
			info.Add(new ParameterInfo("password", true));
			info.Add(new ParameterInfo("database", true));

			string[] input = { "--username", "root", "--password", "changeit", "--database", "somedb" };

			IDictionary<string, string>	parameters = parser.Parse(input, info);

			Assert.AreEqual(3, parameters.Count);

			Assert.IsTrue(parameters.ContainsKey("username"));
			Assert.IsTrue(parameters.ContainsKey("password"));
			Assert.IsTrue(parameters.ContainsKey("database"));

			Assert.AreEqual("root", parameters["username"]);
			Assert.AreEqual("changeit", parameters["password"]);
			Assert.AreEqual("somedb", parameters["database"]);
		}
	}
}
