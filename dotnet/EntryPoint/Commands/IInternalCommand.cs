using AleksandarNaumovic.EntryPoint.Utilities;

namespace AleksandarNaumovic.EntryPoint.Commands;

internal interface IInternalCommand
{
	public IOutputWriter OutputWriter { set; } 
}