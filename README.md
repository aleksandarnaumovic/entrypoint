# Entrypoint


Entrypoint is a framework that enables building of custom command line interfaces. A framework parses and validates command line arguments, selects the particular command, passes parsed arguments and executes the command.

A project is intended to be implemented both in .NET and Java, but for now there is only a .NET variance.

Dot net solution has three projects:

1. EntryPoint - actual framework
2. EntryPointTest - framework unit tests
3. TestConsoleApp - example of test console app

In current implementation, CLI could look like:

`cli <subcommand> <subcommand> <subcommand> --argument value --another-argument value`

In particular project using this framework, executable assembly can have it's own name, and variable-lenghted subcommands arrays can be associated with each implemented command. Each command needs to be conformant with an interface which defines command parameters definition, execution logic, and associated usage / description messages. A main message appearing at the top is customizable too. A framework adds also a help subcommand which displays a list of available commands.

## Custom CLI implementation

A basic example appliction can be found in TestConsoleApp project. To implement your own CLI, do follow the next steps:

1. Create a console project

Pay attention to the assembly name. Executable representing the actual command will be named this way.

2. Add reference to EntryPoint

For now, add a reference to the manually built EntryPoint assembly. When a NuGet package will be released, add a reference to the package. 

3. Implement one or more commands

Use ICommand interface or inherit AbstractCommand, and implement abstract parts.

A part of the ICommand interface concerning basic functionality of any command is implemented withitn AbstractCommand. This includes handling input parameters, providing them within inherited class so they can be read during execution. Handling result is also supported, and outputWriter is available during command execution to provide a means of progressive output writing.

When using AbstractCommand, you need to implement the following:

- ParametersDefinition - a list of parameters metadata. For now, only mandatory named parameters have been implemented.
- Command execution
- Command description which is displayed as command help or when a command has not been found on execution.
- Command usage, which is displayed when a command is found, but parameters do not match.

6. In Program entry point, add the code with commands registration and endpoint execution.

```
    EntryPointConfiguration config = EntryPoint.CreateConfiguration(); 
    config.DefaultMessage = "Software which should do something.";
    
    config.AddCommand(["create", "entity"], new EntityCreationCommand());
    config.AddCommand(["create", "default", "entity"], new DefaultEntityCreationCommand());
    config.AddCommand(["update", "entity", "default", "reset"], new DefaultEntityResetCommand());
    config.AddCommand(["update", "entity"], new EntityUpdateCommand());

    EntryPoint.GetInstance(config).Execute(arguments);
```
