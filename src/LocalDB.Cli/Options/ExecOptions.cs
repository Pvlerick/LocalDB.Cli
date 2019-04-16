using CommandLine;

namespace LocalDB.Cli.Options
{
    [Verb("exec", HelpText = "Execute a command of a script on the local database.")]
    internal class ExecOptions
    {
        [Value(0, Required = true, HelpText = "The new local database's name")]
        public string Name { get; set; }

        [Option('c', "command", HelpText = "The command to execute on the local database", SetName = "command")]
        public string Command { get; set; }

        [Option('s', "script", HelpText = "The script to execute on the local database", SetName = "script")]
        public string Script { get; set; }
    }
}
