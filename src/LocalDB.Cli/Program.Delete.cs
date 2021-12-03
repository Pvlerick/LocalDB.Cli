using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;

namespace LocalDB.Cli;

partial class Program
{
    static Command CreateDeleteCommand()
    {
        var command = new Command("delete", "Delete a database")
        {
            new Argument("name") { Description = "The name of the database" }
        };

        command.Handler = CommandHandler.Create<string>(async name =>
        {
            using var connection = new SqlConnection(string.Format(ConnectionStringTemplate, "master"));

            var owner = (await connection
                .QueryAsync<string>("SELECT suser_sname(owner_sid) as owner FROM sys.databases WHERE name = @Name", new { Name = name }))
                .Single();

            if (owner == "sa")
            {
                Console.WriteLine("Cannot delete a system database");
                return;
            }

            await connection.ExecuteAsync("EXEC sp_detach_db @Name", new { Name = name });
        });

        return command;
    }
}
