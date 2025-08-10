using AleksandarNaumovic.EntryPoint.Commands;
using System.Text;
using AleksandarNaumovic.EntryPoint.Utilities;

namespace AleksandarNaumovic.EntryPoint
{
	public class EntryPoint
	{
		private readonly EntryPointConfiguration configuration;
		private readonly IParametersParser parser;
		private readonly IParametersValidator validator;
		private readonly IOutputWriter outputWriter;

		internal EntryPoint(EntryPointConfiguration configuration, IParametersParser parser, IParametersValidator validator, IOutputWriter outputWriter)
		{
			this.configuration = configuration;
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
			if (arguments.Length < 2)
			{
				outputWriter.WriteLine(Messages.IncorrectSyntax);
				return;
			}

			ICommand command = configuration.CommandRegistry.Get(arguments[0], arguments[1]);

			if (command == null)
			{
				configuration.CommandRegistry.WriteRegisteredDescriptions(outputWriter);
				return;
			}

			IList<ParameterInfo> info = command.ParametersDefinition;

			IDictionary<string, string> parameters = parser.Parse(arguments.Skip(2).ToArray(), info);

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

		#region singleton

		private static EntryPoint instance;

		public static EntryPoint GetInstance(EntryPointConfiguration configuration)
		{
			if (instance == null)
			{
				instance = new EntryPoint(configuration, new ParametersParser(), new ParametersValidator(), new ConsoleDirectOutputWriter());
			}
			return instance;
		}

		#endregion singleton
	}
}
