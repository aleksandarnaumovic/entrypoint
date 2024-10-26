
namespace AleksandarNaumovic.EntryPoint.Commands
{
	internal class ParametersValidator : IParametersValidator
	{
		public bool Validate(IList<ParameterInfo> info, IDictionary<string, string> parameters)
		{
			foreach (ParameterInfo parameter in info)
			{
				if (parameter.IsRequired && !parameters.ContainsKey(parameter.Name)) return false;
			}
			return true;
		}
	}
}