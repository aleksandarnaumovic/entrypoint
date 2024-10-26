using AleksandarNaumovic.EntryPoint.Commands;
using NSubstitute;
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

        [Test]
        public void TestGetRegisteredDescriptionWithoutRegistered()
        {
            Assert.AreEqual(string.Empty, registry.GetRegisteredDescriptions());
        }

        [Test]
        public void TestGetRegisteredDescription()
        {
            ICommand command1 = Substitute.For<ICommand>();
            ICommand command2 = Substitute.For<ICommand>();
            ICommand command3 = Substitute.For<ICommand>();

            command1.Description.Returns("Cmd 1 description.");
			command2.Description.Returns("Cmd 2 description.");
			command3.Description.Returns("Cmd 3 description.");

            registry.Register("do", "something", command1);
            registry.Register("do", "somethingelse", command2);
            registry.Register("doelse", "something", command3);

            Assert.AreEqual("do something - Cmd 1 description.\r\ndo somethingelse - Cmd 2 description.\r\ndoelse something - Cmd 3 description.\r\n", registry.GetRegisteredDescriptions());

			_ = command1.Received().Description;
			_ = command2.Received().Description;
			_ = command3.Received().Description;
        }
    }
}
