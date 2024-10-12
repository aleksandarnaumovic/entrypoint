namespace AleksandarNaumovic.EntryPoint.Commands
{
	internal interface IParametersParser
	{
		public IDictionary<string, string> Parse(string[] input);
	}
}
