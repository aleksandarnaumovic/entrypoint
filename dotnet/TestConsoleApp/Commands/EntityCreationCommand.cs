using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint.TestConsoleApp.Commands
{
	internal class EntityCreationCommand : AbstractCommand
	{
		public override IList<ParameterInfo> ParametersDefinition
		{
			get
			{
				List<ParameterInfo> parameters = new List<ParameterInfo>();
				
				parameters.Add(new ParameterInfo("definition", true));
				
				return parameters;
			}
		}

		public override string Description
		{
			get { return "Creates entity"; }
		}

		public override string Usage
		{
			get { return "cli create entity --defintion <definition.json>"; }
		}

		public override void Execute()
		{
			outputWriter.WriteLine("Starting entity creation.");
			outputWriter.WriteLine("Entity creation completed.");
			
			result = "Creating entity";
		}
	}
}
