using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace LocalDB.Cli;

partial class Program
{
    static Command CreateListCommand()
    {
        var command = new Command("list", "List databases (excluding system databases)")
        {
            new Option<string>(
                aliases: new[] { "--filter", "-f" },
                description: "filter based on database name",
                getDefaultValue: () => "*")
        };

        command.Handler = CommandHandler.Create<string>(async filter =>
        {
            using var connection = new SqlConnection(string.Format(ConnectionStringTemplate, "master"));

            var databases = await connection.QueryAsync<string>(
                "SELECT DISTINCT name FROM sys.databases" +
                " WHERE suser_sname(owner_sid) <> 'sa'" + //Exclude system databases
                " AND name LIKE @Filter",
                new { Filter = filter.Replace('*', '%') });

            foreach (var database in databases)
                Console.WriteLine(database);
        });

        return command;
    }
}