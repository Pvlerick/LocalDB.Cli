using CommandLine;

namespace LocalDB.Cli.Options
{
    [Verb("add", HelpText = "Create a new local database.")]
    internal class AddOptions
    {
        [Value(0, Required = true, HelpText = "The new local database's name")]
        public string Name { get; set; }
    }
}
