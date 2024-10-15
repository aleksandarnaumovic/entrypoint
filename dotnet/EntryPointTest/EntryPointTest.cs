using AleksandarNaumovic.EntryPoint.Commands;
using NSubstitute;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test
{
	[TestFixture]
	public class EntryPointTest
	{
		private IParametersParser parser;
		private EntryPointConfiguration config;
		private EntryPoint entryPoint;

		[SetUp]
		public void SetUp()
		{
			parser = Substitute.For<IParametersParser>();

			config = new EntryPointConfiguration();

			entryPoint = new EntryPoint(config, parser);
		}

		[Test]
		public void TestGetInstance()
		{
			EntryPointConfiguration config = new EntryPointConfiguration();

			Assert.NotNull(EntryPoint.GetInstance(config));
			Assert.AreSame(EntryPoint.GetInstance(config), EntryPoint.GetInstance(config));
		}

		[Test]
		public void TestExecute()
		{
			config.DefaultMessage = "default message";

			ICommand command1 = Substitute.For<ICommand>();
			ICommand command2 = Substitute.For<ICommand>();
			ICommand command3 = Substitute.For<ICommand>();

			config.AddCommand("do", "something", command1);
			config.AddCommand("do", "somethingelse", command2);
			config.AddCommand("donotdo", "something", command3);

			IList<ParameterInfo> info = new List<ParameterInfo>();

			info.Add(new ParameterInfo("param1", true));
			info.Add(new ParameterInfo("param2", false));
			info.Add(new ParameterInfo("param3", true));

			command1.ParametersDefinition.Returns(info);

			IDictionary<string, string> parameters = new Dictionary<string, string>();

			parameters.Add("param1", "value1");
			parameters.Add("param2", "value2");
			parameters.Add("param3", "value3");

			parser.Parse(Arg.Is<string[]>(arr => arr.SequenceEqual(new string[] { "--param1", "value1", "--param2", "value2", "--param3", "value3" })), info).Returns(parameters);

			command1.Result.Returns("result message");

			Assert.AreEqual("default message\r\nresult message", entryPoint.Execute(new string[] { "do", "something", "--param1", "value1", "--param2", "value2", "--param3", "value3" }));

			_ = command1.Received().ParametersDefinition;

			parser.Received().Parse(Arg.Is<string[]>( arr => arr.SequenceEqual(new string[] { "--param1", "value1", "--param2", "value2", "--param3", "value3" })), info);

			command1.Received().AddParameters(Arg.Is<IDictionary<string, string>>( dict => AreEqualDictionaries(parameters, dict)));
			command1.Received().Execute();
			_ = command1.Received().Result;
		}

		private bool AreEqualDictionaries(IDictionary<string, string> expected, IDictionary<string, string> actual)
		{
			if (expected == null || actual == null) return false;
			if (expected.Count != actual.Count) return false;

			foreach (KeyValuePair<string, string> item in expected)
			{
				if (!actual.ContainsKey(item.Key) || actual[item.Key] != item.Value) return false;
			}

			return true;
		}
	}
}
