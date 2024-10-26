namespace AleksandarNaumovic.EntryPoint.Commands
{
	public abstract class AbstractCommand : ICommand
	{
		public abstract IList<ParameterInfo> ParametersDefinition { get; }

		protected IDictionary<string, string> parameters;

		public void AddParameters(IDictionary<string, string> parameters)
		{
			this.parameters = parameters;
		}

		public abstract void Execute();

		protected string result;

		public string Result
		{
			get
			{
				return result;
			}
		}

		public abstract string Description { get; }

		public abstract string Usage { get; }
	}
}
