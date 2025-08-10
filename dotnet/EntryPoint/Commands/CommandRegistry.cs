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

		public ICommand Get(string[] arguments)
		{
			foreach (CommandRegistryEntry entry in entries)
			{
				if (comparator.Begins(entry.Key, arguments)) return entry.Command;
			}

			return null;

		}

		public void WriteRegisteredDescriptions(IOutputWriter writer)
		{
			foreach (CommandRegistryEntry entry in entries)
			{
				StringBuilder builder = new StringBuilder();
				foreach (string subcommand in entry.Key) builder.Append($"{subcommand} ");
				
				writer.WriteLine($"{builder.ToString()}- {entry.Command.Description}");
			}
		}
	}
}
