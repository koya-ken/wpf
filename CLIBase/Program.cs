using McMaster.Extensions.CommandLineUtils;

namespace CLIBase;

[Command]
internal class TestCommand
{
    // FileExistsがついてるやつだけとおる
    [Option(ShortName = "n", Description = "help2")]
    [FileExists]
    public string? Name { get; set; } = "";
    [Option(ShortName = "i")]
    [AllowedValues("low", "normal", "high", IgnoreCase = true)]
    public string Importance { get; } = "normal";
    // 文字列ならいける
    [Option(ShortName = "a")]
    public string _age { get; } = "1";
    private int Age => int.Parse(_age);
    public void OnExecute(CommandLineApplication app, IConsole console)
    {

        console.WriteLine($"名前: {Name ?? "名無し"} {Importance} age:{Age}");
        // return 0;
    }

}

public class Program
{

    public static void Main(string[] args)
    {
        // var a = new CommandLineApplication<TestCommand>();
        // a.Conventions.UseAttributes();
        // Console.WriteLine("test");
        // a.Parse();
        // a.Execute();
        // Console.WriteLine("test");
        CommandLineApplication.Execute<TestCommand>(args);
        // return await CommandLineApplication.ExecuteAsync<TestCommand>();
    }
}