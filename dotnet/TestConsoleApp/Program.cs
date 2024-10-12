using AleksandarNaumovic.EntryPoint;

namespace TestConsoleApp
{
	internal class Program
	{
		static void Main(string[] arguments)
		{
			EntryPointConfiguration config = new EntryPointConfiguration();

			Console.WriteLine(EntryPoint.GetInstance(config).Execute(arguments));
		}
	}
}