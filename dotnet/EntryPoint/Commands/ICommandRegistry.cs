using AleksandarNaumovic.EntryPoint.Utilities;

namespace AleksandarNaumovic.EntryPoint.Commands
{
    internal interface ICommandRegistry
    {
        public void Register(string verb, string subject, ICommand command);

        public ICommand Get(string verb, string subject);

        public void WriteRegisteredDescriptions(IOutputWriter writer);
    }
}
