namespace AleksandarNaumovic.EntryPoint.Commands;

internal class CommandRegistryEntry
{
	private string[] key;
	private ICommand command;
	
	public CommandRegistryEntry(string[] key, ICommand command)
	{
		this.key = key;
		this.command = command;
	}

	public string[] Key
	{
		get { return key; }
	}

	public ICommand Command
	{
		get { return command; }
	}
}