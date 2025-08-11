using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint.TestConsoleApp.Commands;

internal class EntityUpdateCommand : AbstractCommand
{
	public override IList<ParameterInfo> ParametersDefinition
	{
		get
		{
			return new List<ParameterInfo>() { new ParameterInfo("definition", true) };
		}
	}

	public override string Description
	{
		get { return "Updates entity according to a new definition."; }
	}

	public override string Usage
	{
		get { return "cli update entity --definition <definition.json>"; }
	}

	public override void Execute()
	{
		outputWriter.WriteLine("Starting entity update.");
		outputWriter.WriteLine("Entity update completed.");
			
		result = "Updating entity";
	}
}