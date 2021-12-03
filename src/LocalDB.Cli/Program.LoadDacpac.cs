using Microsoft.SqlServer.Dac;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;

namespace LocalDB.Cli;

partial class Program
{
    static Command CreateLoadDacpacCommand()
    {
        var command = new Command("load-dacpac")
        {
            new Option<string>(
                aliases: new[] { "--path", "-p" },
                description: "path to the DACPAC file"),
            new Option<string>(
                aliases: new[] { "--name", "-n" },
                description: "name of the new/target database"),
            new Option<string[]>(
                aliases: new[] { "--variables", "-v" },
                description: "variables for the DACPAC file; format is <key>=<value>"),
            new Option<bool>(
                aliases: new[] { "--dry-run", "-d" },
                description: "only ouptut the changes script, don't execute"),
        };

        command.Description = "Create/Update a databases from a DACPAC";

        command.Handler = CommandHandler.Create<string, string, string[], bool>((path, name, variables, dryRun) =>
        {
            var package = DacPackage.Load(path);
            
            var deployOptions = new DacDeployOptions { };

            foreach (var item in variables.Select(i => i.Split('=')).Select(i => new { Key = i[0], Value = i [1] }))
                deployOptions.SqlCommandVariableValues.Add(item.Key, item.Value);

            var dacService = new DacServices(string.Format(ConnectionStringTemplate, name));

            if (dryRun)
            {
                var script = dacService.GenerateDeployScript(
                    package: package,
                    targetDatabaseName: name,
                    options: deployOptions);

                System.Console.WriteLine(script);
            }
            else
            {
                dacService.Deploy(
                    package: package,
                    targetDatabaseName: name,
                    upgradeExisting: true,
                    options: deployOptions);
            }
        });

        return command;
    }
}
