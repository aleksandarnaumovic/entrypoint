namespace AleksandarNaumovic.EntryPoint.Utilities;

internal class ConsoleDirectOutputWriter : IOutputWriter
{
    public void WriteLine()
    {
        WriteLine(string.Empty);
    }

    public void WriteLine(string line)
    {
        Console.WriteLine(line);
    }
}