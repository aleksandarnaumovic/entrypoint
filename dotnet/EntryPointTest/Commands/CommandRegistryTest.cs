using AleksandarNaumovic.EntryPoint.Commands;
using AleksandarNaumovic.EntryPoint.Utilities;
using NSubstitute;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test.Commands
{
    [TestFixture]
    public class CommandRegistryTest
    {
	    private IArrayComparator comparator;
        private CommandRegistry registry;
        
        [SetUp]
        public void SetUp()
        {
	        comparator = Substitute.For<IArrayComparator>();
	        
	        registry = new CommandRegistry(comparator);
        }

		[Test]
		public void TestRegisterVerbTwice()
		{
			registry.Register(["test", "command"], new TestCommand());
			registry.Register(["test", "other"], new TestCommand());
		}
		
        [Test]
        public void TestGetWithoutRegistration()
        {
            Assert.IsNull(registry.Get(["not", "existing"]));
        }

        [Test]
        public void TestGetWithRegisteredNotMatchingSubject()
        {
            TestCommand command = new TestCommand();

            registry.Register(["test", "command"], command);

            comparator.Begins(
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] { "test", "command" })),
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] { "test", "non-existing" })))
	            .Returns(false);

            Assert.IsNull(registry.Get(["test", "non-existing"]));

            comparator.Received().Begins(
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command"})),
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "non-existing"}))
			);
        }

        [Test]
        public void TestGetAfterRegistration()
        {
            TestCommand command = new TestCommand();

            registry.Register(["test", "command"], command);

            comparator.Begins(
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command"})),
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command", "--param1", "value1"})))
	            .Returns(true);

            Assert.AreSame(command, registry.Get(["test", "command", "--param1", "value1"]));

            comparator.Received().Begins(
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command"})),
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command", "--param1", "value1"}))
            );
        }

        [Test]
        public void TestWriteRegisteredDescription()
        {
	        IOutputWriter writer = Substitute.For<IOutputWriter>();

	        ICommand command1 = Substitute.For<ICommand>();
            ICommand command2 = Substitute.For<ICommand>();
            ICommand command3 = Substitute.For<ICommand>();

            command1.Description.Returns("Cmd 1 description.");
			command2.Description.Returns("Cmd 2 description.");
			command3.Description.Returns("Cmd 3 description.");

            registry.Register(["do", "something"], command1);
            registry.Register(["do", "somethingelse"], command2);
            registry.Register(["doelse", "something"], command3);

            // Assert.AreEqual("do something - Cmd 1 description.\r\ndo somethingelse - Cmd 2 description.\r\ndoelse something - Cmd 3 description.\r\n", registry.GetRegisteredDescriptions());
            registry.WriteRegisteredDescriptions(writer);

			_ = command1.Received().Description;
			_ = command2.Received().Description;
			_ = command3.Received().Description;
			
			writer.WriteLine("do something - Cmd 1 description.");
			writer.WriteLine("do somethingelse - Cmd 2 description.");
			writer.WriteLine("doelse something - Cmd 3 description.");
        }
    }
}
