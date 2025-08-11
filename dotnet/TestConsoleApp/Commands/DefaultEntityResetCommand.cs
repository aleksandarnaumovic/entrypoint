using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint.TestConsoleApp.Commands;

internal class DefaultEntityResetCommand : AbstractCommand
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
		get { return "Resets entity to default"; }
	}

	public override string Usage
	{
		get { return "cli update entity default reset"; }
	}

	public override void Execute()
	{
		outputWriter.WriteLine("Starting entity reset.");
		outputWriter.WriteLine("Entity reset completed.");
			
		result = "Resetting entity to default";
	}
}