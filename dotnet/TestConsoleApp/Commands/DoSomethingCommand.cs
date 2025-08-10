using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint.TestConsoleApp.Commands
{
	internal class DoSomethingCommand : AbstractCommand
	{
		public override IList<ParameterInfo> ParametersDefinition
		{
			get
			{
				return new List<ParameterInfo>();
			}
		}

		public override string Description
		{
			get { return "Do something"; }
		}

		public override string Usage
		{
			get { return ""; }
		}

		public override void Execute()
		{
			outputWriter.WriteLine("Write for the first time from do something command.");
			outputWriter.WriteLine("Write for the second time from do something command.");
			
			result = "Doing something";
		}
	}
}
