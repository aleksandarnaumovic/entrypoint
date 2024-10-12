using AleksandarNaumovic.EntryPoint.Commands;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test.Commands
{
    [TestFixture]
    public class CommandRegistryTest
    {
        private CommandRegistry registry;

        [SetUp]
        public void SetUp()
        {
            registry = new CommandRegistry();
        }

		[Test]
		public void TestRegisterVerbTwice()
		{
			registry.Register("test", "command", new TestCommand());
			registry.Register("test", "other", new TestCommand());
		}
		
        [Test]
        public void TestGetWithoutRegistration()
        {
            Assert.IsNull(registry.Get("not", "existing"));
        }

        [Test]
        public void TestGetWithRegisteredNotMatchingSubject()
        {
            TestCommand command = new TestCommand();

            registry.Register("test", "command", command);

            Assert.IsNull(registry.Get("test", "non-existing"));
        }

        [Test]
        public void TestGetAfterRegistration()
        {
            TestCommand command = new TestCommand();

            registry.Register("test", "command", command);

            Assert.AreSame(command, registry.Get("test", "command"));
        }
    }
}
