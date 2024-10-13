
namespace AleksandarNaumovic.EntryPoint.Commands
{
	internal class ParametersParser : IParametersParser
	{
		public IDictionary<string, string> Parse(string[] input, IList<ParameterInfo> info)
		{
			IDictionary<string, string> parameters = new Dictionary<string, string>();

			for (int i = 0; i < input.Length; i++)
			{
				parameters.Add(input[i].Substring(2), input[++i]);
			}

			return parameters;
		}
	}
}
