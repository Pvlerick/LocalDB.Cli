using CommandLine;

namespace LocalDB.Cli.Options
{
    [Verb("remove", HelpText = "Delete a local database.")]
    internal class RemoveOptions
    {
        [Value(0, Required = true, HelpText = "The local database's name")]
        public string Name { get; set; }
    }
}
