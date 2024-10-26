using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint.TestConsoleApp.Commands
{
	internal class DoAnotherCommand : AbstractCommand
	{
		public override IList<ParameterInfo> ParametersDefinition
		{
			get
			{
				return new List<ParameterInfo>() { new ParameterInfo("required", true) };
			}
		}

		public override string Description
		{
			get { return "Do something else"; }
		}

		public override string Usage
		{
			get { return "Incorrect usage. It should be do another --required something"; }
		}

		public override void Execute()
		{
			result = "Doing another";
		}
	}
}
