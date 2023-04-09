namespace PWBS.ConfigFile;

public class ConfigurationFileJsonPlugin: ConfigurationFilePluginInterface
{
    public bool IsValid(string configurationFilePath)
    {
        if (configurationFilePath == "")
            configurationFilePath = PWBSConfigFileManager.GetDefaultConfigurationFilePath();
        return Path.GetExtension(configurationFilePath) == "json";
    }

    public PWBSConfigurationFile LoadFile(string configurationFilePath)
    {
        if (configurationFilePath == "")
            configurationFilePath = PWBSConfigFileManager.GetDefaultConfigurationFilePath();
        
        if (!File.Exists(configurationFilePath))
            throw new ConfigurationFileException("Configuration File not found.");

        var jsonText = File.ReadAllText(configurationFilePath);
        var data = PWBSConfigurationFile.createPWBSConfigurationFileBasedOnJson(jsonText);
        if (data is null) throw new ConfigurationFileException("Configuration File is invalid.");
        return data;
    }

    public void SaveFile(string configurationFilePath, PWBSConfigurationFile data)
    {
        if (configurationFilePath == "")
            configurationFilePath = PWBSConfigFileManager.GetDefaultConfigurationFilePath();
        
        var json = PWBSConfigurationFile.createJsonBasedOnPWBSConfigurationFile(data);

        using var configurationFileWriter = File.CreateText(configurationFilePath);
        configurationFileWriter.Write(json);
        configurationFileWriter.Close();
    }

    public void NewFile(string configurationFilePath, PWBSConfigurationFile data)
    {
        if (configurationFilePath == "")
            configurationFilePath = PWBSConfigFileManager.GetDefaultConfigurationFilePath();

        var json = PWBSConfigurationFile.createJsonBasedOnPWBSConfigurationFile(data);

        using var configurationFileWriter = File.CreateText(configurationFilePath);
        configurationFileWriter.Write(json);
        configurationFileWriter.Close();
    }
}