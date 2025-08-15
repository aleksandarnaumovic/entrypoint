using System.Text;
using AleksandarNaumovic.EntryPoint.Utilities;

namespace AleksandarNaumovic.EntryPoint.Commands
{
	internal class CommandRegistry : ICommandRegistry
	{
		private readonly IArrayComparator comparator;
		private readonly IList<CommandRegistryEntry> entries;

		public CommandRegistry(IArrayComparator comparator)
		{
			this.comparator = comparator;
			
			entries = new List<CommandRegistryEntry>();
		}

		public void Register(string[] subcommands, ICommand command)
		{
			entries.Add(new CommandRegistryEntry(subcommands, command));
		}

		public CommandRegistryEntry Get(string[] arguments)
		{
			foreach (CommandRegistryEntry entry in entries)
			{
				if (comparator.Begins(arguments, entry.Key)) return entry;
			}

			return null;
		}

		public ICommand GetCommand(string[] arguments)
		{
			foreach (CommandRegistryEntry entry in entries)
			{
				if (comparator.Begins(arguments, entry.Key)) return entry.Command;
			}

			return null;

		}

		public void WriteRegisteredDescriptions(IOutputWriter writer)
		{
			foreach (CommandRegistryEntry entry in entries)
			{
				if (!entry.Command.IncludeInHelp) continue;
				
				StringBuilder builder = new StringBuilder();
				foreach (string subcommand in entry.Key) builder.Append($"{subcommand} ");
				
				writer.WriteLine($"{builder.ToString()}- {entry.Command.Description}");
			}
		}
	}
}
