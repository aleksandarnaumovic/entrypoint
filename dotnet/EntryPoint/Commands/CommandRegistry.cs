using AleksandarNaumovic.EntryPoint.Utilities;

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
			GetCommandsForVerb(verb).Add(subject, command);
		}

		private IDictionary<string, ICommand> GetCommandsForVerb(string verb)
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

		public void WriteRegisteredDescriptions(IOutputWriter writer)
		{
			foreach (string verb in commands.Keys)
			{
				foreach (string subject in commands[verb].Keys)
				{
					writer.WriteLine($"{verb} {subject} - {commands[verb][subject].Description}");
				}
			}
		}
	}
}
