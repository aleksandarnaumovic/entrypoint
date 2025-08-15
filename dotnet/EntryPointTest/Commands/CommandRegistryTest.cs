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
        public void TestGetWithoutRegistration()
        {
            Assert.IsNull(registry.GetCommand(["not", "existing"]));
        }

        [Test]
        public void TestGetWithRegisteredNotMatchingSubject()
        {
            TestCommand command = new TestCommand();

            registry.Register(["test", "command"], command);

            comparator.Begins(
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] { "test", "non-existing" })),
				Arg.Is<string[]>(arr => arr.SequenceEqual(new[] { "test", "command" })))
	            .Returns(false);

            Assert.IsNull(registry.Get(["test", "non-existing"]));

            comparator.Received().Begins(
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "non-existing"})),
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command"}))
			);
        }

        [Test]
        public void TestGetAfterRegistration()
        {
            TestCommand command = new TestCommand();

            registry.Register(["test", "command"], command);

            comparator.Begins(
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command", "--param1", "value1"})),
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command"}))
	            )
	            .Returns(true);

            Assert.AreSame(command, registry.Get(["test", "command", "--param1", "value1"]).Command);

            comparator.Received().Begins(
	            
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command", "--param1", "value1"})),
	            Arg.Is<string[]>(arr => arr.SequenceEqual(new[] {"test", "command"}))
            );
        }

        [Test]
        public void TestWriteRegisteredDescription()
        {
	        IOutputWriter writer = Substitute.For<IOutputWriter>();

	        ICommand command1 = Substitute.For<ICommand>();
            ICommand command2 = Substitute.For<ICommand>();
            ICommand command3 = Substitute.For<ICommand>();
            ICommand skipCommand = Substitute.For<ICommand>();

            command1.Description.Returns("Cmd 1 description.");
			command2.Description.Returns("Cmd 2 description.");
			command3.Description.Returns("Cmd 3 description.");
			skipCommand.Description.Returns("Skip description.");
			
			command1.IncludeInHelp.Returns(true);
			command2.IncludeInHelp.Returns(true);
			command3.IncludeInHelp.Returns(true);
			skipCommand.IncludeInHelp.Returns(false);

            registry.Register(["do", "something"], command1);
            registry.Register(["do", "somethingelse"], command2);
            registry.Register(["doelse", "something"], command3);
            registry.Register(["skip"], skipCommand);

            // Assert.AreEqual("do something - Cmd 1 description.\r\ndo somethingelse - Cmd 2 description.\r\ndoelse something - Cmd 3 description.\r\n", registry.GetRegisteredDescriptions());
            registry.WriteRegisteredDescriptions(writer);

			_ = command1.Received().Description;
			_ = command2.Received().Description;
			_ = command3.Received().Description;
			
			writer.Received().WriteLine("do something - Cmd 1 description.");
			writer.Received().WriteLine("do somethingelse - Cmd 2 description.");
			writer.Received().WriteLine("doelse something - Cmd 3 description.");
			writer.DidNotReceive().WriteLine("skip - Skip description.");
        }
    }
}
