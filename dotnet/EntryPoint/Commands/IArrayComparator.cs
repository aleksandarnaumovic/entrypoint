namespace AleksandarNaumovic.EntryPoint.Commands;

internal interface IArrayComparator
{
	public bool Begins(string[] compared, string[] expectedBeginning);
}