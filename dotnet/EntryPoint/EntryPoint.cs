using AleksandarNaumovic.EntryPoint.Commands;
using System.Text;

namespace AleksandarNaumovic.EntryPoint
{
	public class EntryPoint
	{
		private readonly EntryPointConfiguration configuration;
		private readonly IParametersParser parser;
		private readonly IParametersValidator validator;

		internal EntryPoint(EntryPointConfiguration configuration, IParametersParser parser, IParametersValidator validator)
		{
			this.configuration = configuration;
			this.parser = parser;
			this.validator = validator;
		}

		public string Execute(string[] arguments)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine();
			builder.AppendLine(configuration.DefaultMessage);
			builder.AppendLine();

			// arguments at first two positions are by convention verb and subject. That could be extended by more complex scenarios later
			if (arguments.Length < 2)
			{
				builder.AppendLine(Messages.IncorrectSyntax);
				return builder.ToString();
			}

			ICommand command = configuration.CommandRegistry.Get(arguments[0], arguments[1]);

			if (command == null)
			{
				builder.Append(configuration.CommandRegistry.GetRegisteredDescriptions());
				return builder.ToString();
			}

			IList<ParameterInfo> info = command.ParametersDefinition;

			IDictionary<string, string> parameters = parser.Parse(arguments.Skip(2).ToArray(), info);

			if (!validator.Validate(info, parameters))
			{
				builder.AppendLine(command.Usage);
				return builder.ToString();
			}

			command.AddParameters(parameters);
			command.Execute();

			builder.AppendLine(command.Result);
			return builder.ToString();
		}

		#region singleton

		private static EntryPoint instance;

		public static EntryPoint GetInstance(EntryPointConfiguration configuration)
		{
			if (instance == null)
			{
				instance = new EntryPoint(configuration, new ParametersParser(), new ParametersValidator());
			}
			return instance;
		}

		#endregion singleton
	}
}
