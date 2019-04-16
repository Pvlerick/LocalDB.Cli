using CommandLine;

namespace LocalDB.Cli.Options
{
    [Verb("inspect", HelpText = "Inspect a local database.")]
    internal class InspectOptions
    {
        [Value(0, Required = true, HelpText = "The local database's name")]
        public string Name { get; set; }

        [Option('m', "machine-readable", Required = false, HelpText = "Machine readable format: get all the properties in a single line in comma (\",\") separated values")]
        public bool MachineReadable { get; set; }
    }
}
