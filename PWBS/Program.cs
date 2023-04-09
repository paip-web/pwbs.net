using System.CommandLine;
using System.CommandLine.IO;
using PWBS.ConfigFile;
using PWBS.NetFrameworkUtils;
using Spectre.Console;

namespace PWBS;

public static class Program
{

    private static void MainHandler(FileInfo? configFile, string[] tasks)
    {
        PWBSLogger.AppBanner();
        
        PWBSConfigFileManager configFileManager = new();
        var configFileString = configFile is null ? "" : configFile.FullName;
        if (!File.Exists(configFileString))
            configFileManager.NewConfigurationFile(new PWBSConfigurationFile(), configFileString);
        var configFileData = configFileManager.LoadConfigurationFile(configFileString);
        if (configFileData is null)
            throw new Exception("Configuration file is not valid.");
        foreach (var task in tasks)
        {
            if (configFileData.commands.TryGetValue(task, out var cmd))
            {
                foreach (var cmdCommandString in cmd.CommandStrings)
                {
                    CommandExecute.Execute(cmdCommandString);
                }
            }
            else
            {
                AnsiConsole.WriteLine($"[red]Task {Markup.Escape(task)} found.[/]");
            }
        }
    }
    
    public static int Main(string[] args)
    {
        var rootCommand = new RootCommand
        {
            Name = "PWBS",
            Description = "PAiP Web Build System is Build System for easy automation process."
        };
        
        var configFileOption = new Option<FileInfo?>(
            name: "--config-file",
            description: "PWBS Configuration File",
            getDefaultValue: () => new FileInfo(PWBSConfigFileManager.GetDefaultConfigurationFilePath())
        );
        rootCommand.AddOption(configFileOption);
        
        var tasksArgument = new Argument<string[]>(
            name: "tasks",
            description: "List of tasks to execute."
        );
        rootCommand.Add(tasksArgument);
        
        rootCommand.SetHandler(MainHandler, configFileOption, tasksArgument);
        
        if (args.Length == 0)
        {
            return rootCommand.Invoke("--help");
        }

        return rootCommand.Invoke(args);
        
    }
}