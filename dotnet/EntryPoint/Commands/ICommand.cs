namespace AleksandarNaumovic.EntryPoint.Commands
{
	public interface ICommand
	{
		public IList<ParameterInfo> ParametersDefinition { get; }

		public void AddParameters(IDictionary<string, string> parameters);

		public void Execute();

		public string Result { get; }

		bool IncludeInHelp { get; }

		public string Description { get; }

		public string Usage {  get; }
	}
}
