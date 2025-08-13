using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint
{
	public class EntryPointConfiguration
	{
		private ICommandRegistry commandRegistry;
		private string defaultMessage;

		internal EntryPointConfiguration(ICommandRegistry registry)
		{
			commandRegistry = registry;
			defaultMessage = Messages.DefaultMessage;
		}

		public string DefaultMessage
		{
			get
			{
				return defaultMessage;
			}
			set
			{
				defaultMessage = value;
			}
		}
		
		public void AddCommand(string[] subcommands, ICommand command)
		{
			commandRegistry.Register(subcommands, command);
		}

		internal virtual ICommandRegistry CommandRegistry
		{
			get
			{
				return commandRegistry;
			}
		}
	}
}