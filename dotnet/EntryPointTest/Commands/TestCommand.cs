using AleksandarNaumovic.EntryPoint.Commands;

namespace AleksandarNaumovic.EntryPoint.Test.Commands
{
	internal class TestCommand : AbstractCommand
	{
		override public IList<ParameterInfo> ParametersDefinition
		{
			get
			{
				return new List<ParameterInfo>();
			}
		}

		public IDictionary<string, string> GetParameters()
		{
			return parameters;
		}

		override public void Execute()
		{
		}

		public void SetResult(string result)
		{
			this.result = result;
		}
	}
}
