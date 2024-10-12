namespace AleksandarNaumovic.EntryPoint
{
	public class EntryPoint
	{
		internal EntryPoint(EntryPointConfiguration configuration) 
		{
		}

		public string Execute(string[] arguments)
		{
			return "\r\nEntryPoint v1.0.0 (C) Aleksandar Naumovic 2024.";
		}

		#region singleton

		private static EntryPoint instance;

		public static EntryPoint GetInstance(EntryPointConfiguration configuration)
		{
			if (instance == null)
			{
				instance = new EntryPoint(configuration);
			}
			return instance;
		}

		#endregion singleton
	}
}
