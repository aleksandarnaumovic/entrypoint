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
		private ICommandRegistry registry;
		private IParametersParser parser;
		private IParametersValidator validator;
		private IOutputWriter outputWriter;
		private EntryPointConfiguration config;
		private EntryPoint entryPoint;

		[SetUp]
		public void SetUp()
		{
			registry = Substitute.For<ICommandRegistry>();
			parser = Substitute.For<IParametersParser>();
			validator = Substitute.For<IParametersValidator>();
			outputWriter = Substitute.For<IOutputWriter>();

			config = EntryPoint.CreateConfiguration();

			entryPoint = new EntryPoint(config, registry, parser, validator, outputWriter);
		}

		[Test]
		public void TestCreateConfiguration()
		{
			Assert.IsNotNull(EntryPoint.CreateConfiguration());
			
			Assert.AreNotSame(EntryPoint.CreateConfiguration(), EntryPoint.CreateConfiguration());
		}

		[Test]
		public void TestCreateConfigurationWithHelpCommand()
		{
			Assert.AreEqual(typeof(HelpCommand), EntryPoint.CreateConfiguration().CommandRegistry.Get(["help"]).Command.GetType());
		}

		[Test]
		public void TestGetInstance()
		{
			EntryPointConfiguration config = EntryPoint.CreateConfiguration();

			Assert.NotNull(EntryPoint.GetInstance(config));
			Assert.AreSame(EntryPoint.GetInstance(config), EntryPoint.GetInstance(config));
		}

		[Test]
		public void TestExecute()
		{
			string[] arguments = ["do", "something", "--param1", "value1", "--param2", "value2", "--param3", "value3"];

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

			CommandRegistryEntry registryEntry = new CommandRegistryEntry(["do", "something"], command1);

			registry.Get(arguments).Returns(registryEntry);

			command1.ParametersDefinition.Returns(info);

			IDictionary<string, string> parameters = new Dictionary<string, string>();

			parameters.Add("param1", "value1");
			parameters.Add("param2", "value2");
			parameters.Add("param3", "value3");

			parser.Parse(Arg.Is<string[]>(arr => arr.SequenceEqual(new string[] { "--param1", "value1", "--param2", "value2", "--param3", "value3" })), info).Returns(parameters);

			validator.Validate(info, parameters).Returns(true);

			command1.Result.Returns("result message");

			entryPoint.Execute(arguments);

			outputWriter.Received().WriteLine();
			outputWriter.Received().WriteLine("default message");
			outputWriter.Received().WriteLine();
			
			outputWriter.Received().WriteLine("result message");
			
			registry.Received().Get(arguments);
			
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
			entryPoint.Execute([]);

			outputWriter.Received().WriteLine();
			outputWriter.Received().WriteLine("EntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.");
			outputWriter.Received().WriteLine();
			
			outputWriter.Received().WriteLine("Invalid command. Please try <subcommand> <subcommand> <subcommand> arguments.... form.");
		}

		[Test]
		public void TestExecuteSufficientArguments()
		{
			string[] arguments = ["justdo"];

			ICommand command1 = Substitute.For<ICommand>();
			ICommand command2 = Substitute.For<ICommand>();
			ICommand command3 = Substitute.For<ICommand>();

			config.AddCommand(["do", "something"], command1);
			config.AddCommand(["do", "somethingelse"], command2);
			config.AddCommand(["donotdo", "something"], command3);

			command1.Description.Returns("desc 1");
			command2.Description.Returns("desc 2");
			command3.Description.Returns("desc 3");
			
			registry.Get(arguments).Returns((CommandRegistryEntry) null);
			
			registry.WriteRegisteredDescriptions(outputWriter);

			entryPoint.Execute(arguments);

			outputWriter.Received().WriteLine();
			outputWriter.Received().WriteLine("EntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.");
			outputWriter.Received().WriteLine();
			
			registry.Received().Get(arguments);

			registry.Received().WriteRegisteredDescriptions(outputWriter);
		}

		[Test]
		public void TestExecuteCommandNotFound()
		{
			string[] arguments = ["not", "found"];

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

			registry.Get(arguments).Returns((CommandRegistryEntry) null);
			
			registry.WriteRegisteredDescriptions(outputWriter);

			entryPoint.Execute(arguments);

			outputWriter.Received().WriteLine();
			outputWriter.Received().WriteLine("default message");
			outputWriter.Received().WriteLine();

			registry.Received().Get(arguments);

			registry.Received().WriteRegisteredDescriptions(outputWriter);
		}

		[Test]
		public void TestExecuteInvalidArguments()
		{
			string[] arguments = ["do", "something", "--param1", "value1", "--param2", "value2", "--param3", "value3"];

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
			
			CommandRegistryEntry registryEntry = new CommandRegistryEntry(["do", "something"], command1);

			registry.Get(arguments).Returns(registryEntry);

			command1.ParametersDefinition.Returns(info);

			IDictionary<string, string> parameters = new Dictionary<string, string>();

			parameters.Add("param1", "value1");
			parameters.Add("param2", "value2");
			parameters.Add("param3", "value3");

			parser.Parse(Arg.Is<string[]>(arr => arr.SequenceEqual(new string[] { "--param1", "value1", "--param2", "value2", "--param3", "value3" })), info).Returns(parameters);

			validator.Validate(info, parameters).Returns(false);

			command1.Usage.Returns("usage message");

			entryPoint.Execute(arguments);

			outputWriter.Received().WriteLine();
			outputWriter.Received().WriteLine("default message");
			outputWriter.Received().WriteLine();
			
			outputWriter.Received().WriteLine("usage message");

			registry.Received().Get(arguments);

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
