using AleksandarNaumovic.EntryPoint.Utilities;

namespace AleksandarNaumovic.EntryPoint.Commands
{
	internal interface ICommandRegistry
	{
		public void Register(string[] subcommands, ICommand command);
		
		public CommandRegistryEntry Get(string[] arguments);
		
		public ICommand GetCommand(string[] arguments);

		public void WriteRegisteredDescriptions(IOutputWriter writer);
	}
}