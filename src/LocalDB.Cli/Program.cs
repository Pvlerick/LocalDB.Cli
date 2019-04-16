using CommandLine;
using LocalDB.Cli.Options;

namespace LocalDB.Cli
{
    partial class Program
    {
        private const string StorageFolder = ".localdb";
        private const string ConnectionStringTemplate = @"data source=(localdb)\MSSQLLocalDB;Initial Catalog={0}";

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<AddOptions, ExecOptions, RemoveOptions, InspectOptions, LsOptions>(args)
                .MapResult(
                    (AddOptions o) => Add(o.Name),
                    (ExecOptions o) => Exec(o.Name, o.Command, o.Script),
                    (RemoveOptions o) => Remove(o.Name),
                    (InspectOptions o) => Inspect(o.Name, o.MachineReadable),
                    (LsOptions o) => List(),
                    _ => 1);
        }
    }
}
