namespace AleksandarNaumovic.EntryPoint.Commands;

internal class ArrayComparator : IArrayComparator
{
	public bool Begins(string[] compared, string[] expectedBeginning)
	{
		if (compared.Length < expectedBeginning.Length) return false;

		for (int i = 0; i < expectedBeginning.Length; i++)
		{
			if (compared[i] != expectedBeginning[i]) return false;
		}
		
		return true;
	}
}