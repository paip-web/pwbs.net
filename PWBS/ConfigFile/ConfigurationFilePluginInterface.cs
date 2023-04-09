namespace PWBS.ConfigFile;

public interface ConfigurationFilePluginInterface
{
    /// <summary>
    /// Check if the configuration file is valid for this plugin
    /// </summary>
    /// <param name="configurationFilePath">Configuration File Path or Empty string for default one</param>
    /// <returns>If configuration file is valid for this plugin</returns>
    bool IsValid(string configurationFilePath);
    /// <summary>
    /// Load Configuration File
    /// </summary>
    /// <param name="configurationFilePath">Configuration File to Load or Empty String for default one</param>
    /// <returns>Serialized Data from file</returns>
    PWBSConfigurationFile LoadFile(string configurationFilePath);

    /// <summary>
    /// Save Configuration File
    /// </summary>
    /// <param name="configurationFilePath">Configuration File to Save to or Empty String for default one</param>
    /// <param name="data">Data to save</param>
    void SaveFile(string configurationFilePath, PWBSConfigurationFile data);
    /// <summary>
    /// Create New Configuration File
    /// </summary>
    /// <param name="configurationFilePath">Configuration File to Save or Empty String for default one</param>
    /// <param name="data">Data to save</param>
    void NewFile(string configurationFilePath, PWBSConfigurationFile data);
}