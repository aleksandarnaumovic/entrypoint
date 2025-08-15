using AleksandarNaumovic.EntryPoint.Commands;
using AleksandarNaumovic.EntryPoint.Utilities;
using NSubstitute;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace AleksandarNaumovic.EntryPoint.Test.Commands;

[TestFixture]
public class HelpCommandTest
{
	private ICommandRegistry registry;
	private HelpCommand command;
	private IOutputWriter writer;

	[SetUp]
	public void SetUp()
	{
		writer = Substitute.For<IOutputWriter>();
		registry = Substitute.For<ICommandRegistry>();
		
		command = new HelpCommand(writer, registry);
	}

	[Test]
	public void TestIncludeInHelp()
	{
		Assert.False(command.IncludeInHelp);
	}

	[Test]
	public void TestParametersDefinition()
	{
		Assert.AreEqual(0, command.ParametersDefinition.Count);
	}

	[Test]
	public void TestDescription()
	{
		Assert.AreEqual("Displays help message.", command.Description);
	}

	[Test]
	public void TestUsage()
	{
		Assert.AreEqual("command help", command.Usage);
	}

	[Test]
	public void TestExecute()
	{
		command.Execute();
		
		registry.Received().WriteRegisteredDescriptions(writer);
	}
}