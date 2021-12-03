using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;

namespace LocalDB.Cli;

partial class Program
{
    static Command CreateInspectCommand()
    {
        var command = new Command("inspect", "Inspect a database")
        {
            new Argument("name") { Description = "The name of the database" }
        };

        command.Handler = CommandHandler.Create<string>(async name =>
        {
            using var connection = new SqlConnection(string.Format(ConnectionStringTemplate, "master"));

            using var multi = await connection.QueryMultipleAsync(
@"SELECT database_id, name, suser_sname(owner_sid) as owner FROM sys.databases
WHERE name = @Name

SELECT physical_name FROM sys.master_files mf, sys.databases db
WHERE mf.database_id = db.database_id
AND db.name = @Name", new { Name = name });

            var (id, dbname, owner) = (await multi.ReadAsync<(int, string, string)>()).Single();

            Console.WriteLine("{0,-20}{1}", "Name", dbname);
            Console.WriteLine("{0,-20}{1}", "ID", id);
            Console.WriteLine("{0,-20}{1}", "Owner", owner);

            var title = "Physical files";
            foreach (var item in (await multi.ReadAsync<string>()).ToArray())
            {
                Console.WriteLine("{0,-20}{1}", title, item);
                title = "";
            }
        });

        return command;
    }
}