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
		private IParametersValidator validator;
		private EntryPointConfiguration config;
		private EntryPoint entryPoint;

		[SetUp]
		public void SetUp()
		{
			parser = Substitute.For<IParametersParser>();
			validator = Substitute.For<IParametersValidator>();

			config = new EntryPointConfiguration();

			entryPoint = new EntryPoint(config, parser, validator);
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

			validator.Validate(info, parameters).Returns(true);

			command1.Result.Returns("result message");

			Assert.AreEqual("\r\ndefault message\r\n\r\nresult message\r\n", entryPoint.Execute(new string[] { "do", "something", "--param1", "value1", "--param2", "value2", "--param3", "value3" }));

			_ = command1.Received().ParametersDefinition;

			parser.Received().Parse(Arg.Is<string[]>( arr => arr.SequenceEqual(new string[] { "--param1", "value1", "--param2", "value2", "--param3", "value3" })), info);

			validator.Received().Validate(info, parameters);

			command1.Received().AddParameters(Arg.Is<IDictionary<string, string>>( dict => AreEqualDictionaries(parameters, dict)));
			command1.Received().Execute();
			_ = command1.Received().Result;
		}

		[Test]
		public void TestExecuteInsufficientArguments()
		{
			Assert.AreEqual("\r\nEntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.\r\n\r\nInvalid command. Please try <verb> <subject> arguments.... form.\r\n", entryPoint.Execute(new string[] { "justdo" }));
		}

		[Test]
		public void TestExecuteCommandNotFound()
		{
			config.DefaultMessage = "default message";

			ICommand command1 = Substitute.For<ICommand>();
			ICommand command2 = Substitute.For<ICommand>();
			ICommand command3 = Substitute.For<ICommand>();

			config.AddCommand("do", "something", command1);
			config.AddCommand("do", "somethingelse", command2);
			config.AddCommand("donotdo", "something", command3);

			command1.Description.Returns("desc 1");
			command2.Description.Returns("desc 2");
			command3.Description.Returns("desc 3");

			Assert.AreEqual("\r\ndefault message\r\n\r\ndo something - desc 1\r\ndo somethingelse - desc 2\r\ndonotdo something - desc 3\r\n", entryPoint.Execute(new string[] { "not", "found"}));
		}

		[Test]
		public void TestExecuteInvalidArguments()
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

			validator.Validate(info, parameters).Returns(false);

			command1.Usage.Returns("usage message");

			Assert.AreEqual("\r\ndefault message\r\n\r\nusage message\r\n", entryPoint.Execute(new string[] { "do", "something", "--param1", "value1", "--param2", "value2", "--param3", "value3" }));

			_ = command1.Received().ParametersDefinition;

			parser.Received().Parse(Arg.Is<string[]>( arr => arr.SequenceEqual(new string[] { "--param1", "value1", "--param2", "value2", "--param3", "value3" })), info);

			validator.Received().Validate(info, parameters);


			_ = command1.Received().Usage;
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
