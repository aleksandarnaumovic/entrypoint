using System.Runtime.CompilerServices;
using AleksandarNaumovic.EntryPoint.Utilities;

namespace AleksandarNaumovic.EntryPoint.Commands;

internal class HelpCommand : AbstractCommand
{
	private readonly IOutputWriter writer;
	private readonly ICommandRegistry registry;

	public HelpCommand(IOutputWriter writer, ICommandRegistry registry)
	{
		this.writer = writer;
		this.registry = registry;
		includeInHelp = false;
	}

	public override IList<ParameterInfo> ParametersDefinition
	{
		get
		{
			return new List<ParameterInfo>(); 
			
		}
	}

	public override void Execute()
	{
		registry.WriteRegisteredDescriptions(writer);
	}

	public override string Description
	{
		get { return "Displays help message."; }
	}
	
	public override string Usage { get { return "command help"; } }
}