namespace AleksandarNaumovic.EntryPoint.Commands
{
	public interface ICommand
	{
		public IList<ParameterInfo> ParametersDefinition { get; }

		public void AddParameters(IDictionary<string, string> parameterss);

		public void Execute();

		public string Result { get; }
	}
}
