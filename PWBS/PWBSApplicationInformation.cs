using System.Diagnostics;
using System.Reflection;

namespace PWBS;

/// <summary>
/// PWBS Application Information
/// </summary>
public record PWBSApplicationInformation
{
    /// <summary>
    /// Get Application Information
    /// </summary>
    /// <returns>PWBSApplicationInformation object</returns>
    public static PWBSApplicationInformation GetApplicationInformation()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        return new PWBSApplicationInformation(
            fileVersionInfo.ProductVersion ?? ""
        );
    }
    
    /// <summary>
    /// Constructor for PWBSApplicationInformation
    /// </summary>
    /// <param name="version">Version of Application</param>
    /// <param name="edition">Edition of Application</param>
    public PWBSApplicationInformation(string version, string edition = ".NET Edition")
    {
        this.Version = new Version(version);
        this.Edition = edition;
    }
    
    /// <summary>
    /// Version of Application
    /// </summary>
    public Version Version { get; init; }
    /// <summary>
    /// Edition of Application
    /// </summary>
    public string Edition { get; init; }
};