using AleksandarNaumovic.EntryPoint;

namespace TestConsoleApp
{
	internal class Program
	{
		static void Main(string[] arguments)
		{
			EntryPointConfiguration config = new EntryPointConfiguration();
			config.DefaultMessage = "Program koji radi nesto";

			

			Console.WriteLine(EntryPoint.GetInstance(config).Execute(arguments));
		}
	}
}