namespace AleksandarNaumovic.EntryPoint.Commands
{
	internal class CommandRegistry : ICommandRegistry
	{
		private IDictionary<string, IDictionary<string, ICommand>> commands;

		public CommandRegistry()
		{
			commands = new Dictionary<string, IDictionary<string, ICommand>>();
		}

		public void Register(string verb, string subject, ICommand command)
		{
			GetCommand(verb).Add(subject, command);
		}

		private IDictionary<string, ICommand> GetCommand(string verb)
		{
			if (!commands.ContainsKey(verb)) 
			{
				commands.Add(verb, new Dictionary<string, ICommand>());
			}
			return commands[verb];
		}

		public ICommand Get(string verb, string subject)
		{
			if (!commands.ContainsKey(verb) || !commands[verb].ContainsKey(subject))
			{
				return null;
			}
			return commands[verb][subject];
		}
	}
}
