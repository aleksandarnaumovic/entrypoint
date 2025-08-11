using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint.TestConsoleApp.Commands
{
	internal class DefaultEntityCreationCommand : AbstractCommand
	{
		public override IList<ParameterInfo> ParametersDefinition
		{
			get
			{
				return new List<ParameterInfo>() { new ParameterInfo("name", true) };
			}
		}

		public override string Description
		{
			get { return "Creates default entity"; }
		}

		public override string Usage
		{
			get { return "cli create default entity --name <entity-name>"; }
		}

		public override void Execute()
		{
			outputWriter.WriteLine("Starting entity creation.");
			outputWriter.WriteLine("Entity creation completed.");
			
			result = "Creating default entity";
		}
	}
}
