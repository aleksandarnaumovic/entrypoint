using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint
{
	public class EntryPointConfiguration
	{
		private ICommandRegistry commandRegistry;
		private string defaultMessage;

		public EntryPointConfiguration()
		{
			commandRegistry = new CommandRegistry();
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

		public void AddCommand(string verb, string subject, ICommand command)
		{
			commandRegistry.Register(verb, subject, command);
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