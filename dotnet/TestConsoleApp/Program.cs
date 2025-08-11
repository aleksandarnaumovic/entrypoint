using AleksandarNaumovic.EntryPoint.TestConsoleApp.Commands;

namespace AleksandarNaumovic.EntryPoint.TestConsoleApp
{
	internal class Program
	{
		public static void Main(string[] arguments)
		{
			EntryPointConfiguration config = new EntryPointConfiguration();
			//config.DefaultMessage = "Software which should do something.";

			config.AddCommand(["create", "entity"], new EntityCreationCommand());
			config.AddCommand(["create", "default", "entity"], new DefaultEntityCreationCommand());
			config.AddCommand(["update", "entity", "default", "reset"], new DefaultEntityResetCommand());
			config.AddCommand(["update", "entity"], new EntityUpdateCommand());

			EntryPoint.GetInstance(config).Execute(arguments);
		}
	}
}