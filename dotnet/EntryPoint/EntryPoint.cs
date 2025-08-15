using AleksandarNaumovic.EntryPoint.Commands;
using AleksandarNaumovic.EntryPoint.Utilities;

namespace AleksandarNaumovic.EntryPoint
{
	public class EntryPoint
	{
		private readonly EntryPointConfiguration configuration;
		private readonly ICommandRegistry registry;
		private readonly IParametersParser parser;
		private readonly IParametersValidator validator;
		private readonly IOutputWriter outputWriter;

		internal EntryPoint(EntryPointConfiguration configuration, ICommandRegistry registry, IParametersParser parser, IParametersValidator validator, IOutputWriter outputWriter)
		{
			this.configuration = configuration;
			this.registry = registry;
			this.parser = parser;
			this.validator = validator;
			this.outputWriter = outputWriter;
		}

		public void Execute(string[] arguments)
		{
			outputWriter.WriteLine();
			outputWriter.WriteLine(configuration.DefaultMessage);
			outputWriter.WriteLine();

			// arguments at first two positions are by convention verb and subject. That could be extended by more complex scenarios later
			if (arguments.Length < 1)
			{
				outputWriter.WriteLine(Messages.IncorrectSyntax);
				return;
			}

			CommandRegistryEntry registryEntry = registry.Get(arguments);
			if (registryEntry == null)
			{
				registry.WriteRegisteredDescriptions(outputWriter);
				return;
			}
			
			ICommand command = registryEntry.Command;
			
			string[] parametersArray = new string[arguments.Length - registryEntry.Key.Length];
			Array.Copy(arguments, registryEntry.Key.Length, parametersArray, 0, arguments.Length - registryEntry.Key.Length);

			IList<ParameterInfo> info = command.ParametersDefinition;

			IDictionary<string, string> parameters = parser.Parse(parametersArray, info);

			if (!validator.Validate(info, parameters))
			{
				outputWriter.WriteLine(command.Usage);
				return;
			}

			((IInternalCommand) command).OutputWriter = outputWriter;
			command.AddParameters(parameters);
			command.Execute();

			outputWriter.WriteLine(command.Result);
		}

		#region creation

		public static EntryPointConfiguration CreateConfiguration()
		{
			CommandRegistry registry = new CommandRegistry(new ArrayComparator());
			
			registry.Register(["help"], new HelpCommand(new ConsoleDirectOutputWriter(), registry));
			
			return new EntryPointConfiguration(registry);
		}

		private static EntryPoint instance;

		public static EntryPoint GetInstance(EntryPointConfiguration configuration)
		{
			if (instance == null)
			{
				instance = new EntryPoint(configuration, configuration.CommandRegistry, new ParametersParser(), new ParametersValidator(), new ConsoleDirectOutputWriter());
			}
			return instance;
		}

		#endregion creation
	}
}
