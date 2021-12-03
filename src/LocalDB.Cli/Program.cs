using System.CommandLine;
using System.Threading.Tasks;

namespace LocalDB.Cli;

partial class Program
{
    private const string ConnectionStringTemplate = @"data source=(localdb)\MSSQLLocalDB;Initial Catalog={0}";
    private static readonly string[] SystemDatabases = new[] { "master", "tempdb", "model", "msdb" };

    static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand();

        rootCommand.Add(CreateListCommand());
        rootCommand.Add(CreateLoadDacpacCommand());

        await rootCommand.InvokeAsync(args);
    }
}