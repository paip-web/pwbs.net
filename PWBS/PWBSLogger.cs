﻿using System.Text.Json;
using Spectre.Console;
using Spectre.Console.Json;

namespace PWBS;

/// <summary>
/// Logger class for PWBS
/// </summary>
public static class PWBSLogger
{
    /// <summary>
    /// Application Information
    /// </summary>
    public static PWBSApplicationInformation ApplicationInformation = PWBSApplicationInformation.GetApplicationInformation();
    
    /// <summary>
    /// Application Banner
    /// </summary>
    public static void AppBanner()
    {
        Rule nameRule = new("[navy]PAiP Web Build System[/]")
        {
            Style = Style.Parse("navy")
        };
        Rule versionRule = new($"[navy]Version: [/][aqua]{ApplicationInformation.Version}[/]")
        {
            Style = Style.Parse("navy")
        };
        Rule editionRule = new($"[navy]Edition: [/][aqua]{ApplicationInformation.Edition}[/]")
        {
            Style = Style.Parse("navy")
        };
        
        AnsiConsole.Write(nameRule);
        AnsiConsole.Write(versionRule);
        AnsiConsole.Write(editionRule);
    }

    /// <summary>
    /// Execute Task Log Function
    /// </summary>
    /// <param name="taskName">Name of Task</param>
    /// <param name="taskAction">Action of Task</param>
    public static void ExecuteTask(string taskName, Action taskAction)
    {
        var markupString = $"[green]Executing Task: [/][aqua]{Markup.Escape(taskName)}[/]";
        Rule taskRule = new(markupString)
        {
            Style = Style.Parse("green")
        };
        AnsiConsole.Write(taskRule);

        taskAction();
        
        Rule endRule = new()
        {
            Style = Style.Parse("green")
        };
        AnsiConsole.Write(endRule);
    }
    
    /// <summary>
    /// Debug Log Object
    /// </summary>
    /// <param name="objectToLog">Object to Log</param>
    /// <param name="title">Optional Title</param>
    /// <typeparam name="T">Type of Object</typeparam>
    public static void DebugLogObject<T>(T objectToLog, string title = "")
    {
        string jsonString = JsonSerializer.Serialize(objectToLog, new JsonSerializerOptions { WriteIndented = true });
        var json = new JsonText(jsonString);

        AnsiConsole.Write(
            new Panel(json)
                .Header(string.IsNullOrWhiteSpace(title) ? "Debug Log Object" : $"Debug Log Object: {title}")
                .Collapse()
                .RoundedBorder()
                .BorderColor(Color.Yellow));
    }
}