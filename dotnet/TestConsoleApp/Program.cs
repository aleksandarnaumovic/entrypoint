using AleksandarNaumovic.EntryPoint;
using AleksandarNaumovic.EntryPoint.TestConsoleApp.Commands;

namespace TestConsoleApp
{
	internal class Program
	{
		static void Main(string[] arguments)
		{
			EntryPointConfiguration config = new EntryPointConfiguration();
			//config.DefaultMessage = "Software which should do something.";

			config.AddCommand("do", "something", new DoSomethingCommand());
			config.AddCommand("do", "another", new DoAnotherCommand());

			Console.WriteLine(EntryPoint.GetInstance(config).Execute(arguments));
		}
	}
}