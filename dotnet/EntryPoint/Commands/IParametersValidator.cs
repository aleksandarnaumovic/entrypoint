namespace AleksandarNaumovic.EntryPoint.Commands
{
	internal interface IParametersValidator
	{
		public bool Validate(IList<ParameterInfo> info, IDictionary<string, string> parameters);
	}
}
