using Spectre.Console;

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
}