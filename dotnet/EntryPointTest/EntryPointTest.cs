using AleksandarNaumovic.EntryPoint.Commands;
using AleksandarNaumovic.EntryPoint.Utilities;
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
		private IOutputWriter outputWriter;
		private EntryPointConfiguration config;
		private EntryPoint entryPoint;

		[SetUp]
		public void SetUp()
		{
			parser = Substitute.For<IParametersParser>();
			validator = Substitute.For<IParametersValidator>();
			outputWriter = Substitute.For<IOutputWriter>();

			config = new EntryPointConfiguration();

			entryPoint = new EntryPoint(config, parser, validator, outputWriter);
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

			ICommand command1 = Substitute.For<ICommand, IInternalCommand>();
			ICommand command2 = Substitute.For<ICommand, IInternalCommand>();
			ICommand command3 = Substitute.For<ICommand, IInternalCommand>();

			config.AddCommand(["do", "something"], command1);
			config.AddCommand(["do", "somethingelse"], command2);
			config.AddCommand(["donotdo", "something"], command3);

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

			entryPoint.Execute(["do", "something", "--param1", "value1", "--param2", "value2", "--param3", "value3"]);

			outputWriter.Received().WriteLine();
			outputWriter.Received().WriteLine("default message");
			outputWriter.Received().WriteLine();
			
			outputWriter.Received().WriteLine("result message");
			
			_ = command1.Received().ParametersDefinition;

			parser.Received().Parse(Arg.Is<string[]>( arr => arr.SequenceEqual(new string[] { "--param1", "value1", "--param2", "value2", "--param3", "value3" })), info);

			validator.Received().Validate(info, parameters);

			((IInternalCommand) command1.Received()).OutputWriter = outputWriter;
			command1.Received().AddParameters(Arg.Is<IDictionary<string, string>>( dict => AreEqualDictionaries(parameters, dict)));
			command1.Received().Execute();
			_ = command1.Received().Result;
		}

		[Test]
		public void TestExecuteInsufficientArguments()
		{
			entryPoint.Execute(["justdo"]);

			outputWriter.Received().WriteLine();
			outputWriter.Received().WriteLine("EntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.");
			outputWriter.Received().WriteLine();
			
			outputWriter.Received().WriteLine("Invalid command. Please try <verb> <subject> arguments.... form.");
		}

		[Test]
		public void TestExecuteCommandNotFound()
		{
			config.DefaultMessage = "default message";

			ICommand command1 = Substitute.For<ICommand>();
			ICommand command2 = Substitute.For<ICommand>();
			ICommand command3 = Substitute.For<ICommand>();

			config.AddCommand(["do", "something"], command1);
			config.AddCommand(["do", "somethingelse"], command2);
			config.AddCommand(["donotdo", "something"], command3);

			command1.Description.Returns("desc 1");
			command2.Description.Returns("desc 2");
			command3.Description.Returns("desc 3");

			entryPoint.Execute(["not", "found"]);

			outputWriter.Received().WriteLine();
			outputWriter.Received().WriteLine("default message");
			outputWriter.Received().WriteLine();
			
			outputWriter.Received().WriteLine("do something - desc 1");
			outputWriter.Received().WriteLine("do somethingelse - desc 2");
			outputWriter.Received().WriteLine("donotdo something - desc 3");
		}

		[Test]
		public void TestExecuteInvalidArguments()
		{
			config.DefaultMessage = "default message";

			ICommand command1 = Substitute.For<ICommand>();
			ICommand command2 = Substitute.For<ICommand>();
			ICommand command3 = Substitute.For<ICommand>();

			config.AddCommand(["do", "something"], command1);
			config.AddCommand(["do", "somethingelse"], command2);
			config.AddCommand(["donotdo", "something"], command3);

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

			entryPoint.Execute(["do", "something", "--param1", "value1", "--param2", "value2", "--param3", "value3"]);

			outputWriter.Received().WriteLine();
			outputWriter.Received().WriteLine("default message");
			outputWriter.Received().WriteLine();
			
			outputWriter.Received().WriteLine("usage message");

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
