namespace AleksandarNaumovic.EntryPoint.Commands
{
	public class ParameterInfo
	{
		private string name;
		private bool isRequired;
		private ParameterType type;
		private int? endPosition;

		public ParameterInfo(string name, bool isRequired)
		{
			this.name = name;
			this.isRequired = isRequired;
			type = ParameterType.NamedOption;
		}

		public ParameterInfo(string name, bool isRequired, int endPosition)
			: this(name, isRequired)
		{
			type = ParameterType.PositionedFromTheEnd;
			this.endPosition = endPosition;
		}

		public string Name
		{
			get { return name; }
		}

		public bool IsRequired
		{
			get { return isRequired; }
		}

		public ParameterType Type
		{
			get { return type; }
		}

		public int? EndPosition
		{
			get { return endPosition; }
		}

		public enum ParameterType
		{
			NamedOption,
			PositionedFromTheEnd
		}
	}
}
