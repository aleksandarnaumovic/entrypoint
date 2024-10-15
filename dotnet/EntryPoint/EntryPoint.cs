using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint
{
	public class EntryPoint
	{
		private readonly EntryPointConfiguration configuration;
		private readonly IParametersParser parser;

		internal EntryPoint(EntryPointConfiguration configuration, IParametersParser parser)
		{
			this.configuration = configuration;
			this.parser = parser;
		}

		public string Execute(string[] arguments)
		{
			// arguments at first two positions are by convention verb and subject. That could be extended by more complex scenarios later
			ICommand command = configuration.CommandRegistry.Get(arguments[0], arguments[1]);
			IList<ParameterInfo> info = command.ParametersDefinition;

			IDictionary<string, string> parameters = parser.Parse(arguments.Skip(2).ToArray(), info);

			command.AddParameters(parameters);
			command.Execute();

			return configuration.DefaultMessage + "\r\n" + command.Result;
		}

		#region singleton

		private static EntryPoint instance;

		public static EntryPoint GetInstance(EntryPointConfiguration configuration)
		{
			if (instance == null)
			{
				instance = new EntryPoint(configuration, new ParametersParser());
			}
			return instance;
		}

		#endregion singleton
	}
}
