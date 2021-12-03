using Dapper;
using Microsoft.Data.SqlClient;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace LocalDB.Cli;

partial class Program
{
    static Command CreateListCommand()
    {
        var command = new Command("list")
        {
            new Option<string>(
                aliases: new[] { "--filter", "-f" },
                description: "filter based on database name",
                getDefaultValue: () => "*")
        };

        command.Description = "List databases";

        command.Handler = CommandHandler.Create<string>(filter =>
        {
            using var connection = new SqlConnection(string.Format(ConnectionStringTemplate, "master"));

            var databases = connection.Query<string>(
                "SELECT DISTINCT name FROM sys.databases" +
                " WHERE name NOT IN @SystemDatabases" +
                " AND name LIKE @Filter",
                new { Program.SystemDatabases, Filter = filter.Replace('*', '%') });

            foreach (var item in databases)
                System.Console.WriteLine(item);

        });

        return command;
    }
}
