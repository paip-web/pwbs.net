namespace PWBS.ConfigFile;

public class PWBSConfigFileManager
{
    private List<ConfigurationFilePluginInterface> _configurationFilePlugins = new List<ConfigurationFilePluginInterface>();
    private ConfigurationFilePluginInterface DefaultConfigurationFilePlugin =>
        _configurationFilePlugins[0]
        ?? throw new Exception("No default configuration file plugin found.");

    private ConfigurationFilePluginInterface? LoadedConfigurationFilePlugin; 

    private void AddConfigurationFilePlugins()
    {
    }
    
    public PWBSConfigFileManager()
    {
        AddConfigurationFilePlugins();
    }
    
    public object? LoadConfigurationFile(string configurationFilePath = "")
    {
        ConfigurationFilePluginInterface? foundConfigurationFilePlugin = null;
        foreach (var configurationFilePlugin in _configurationFilePlugins)
        {
            if (configurationFilePlugin.IsValid(configurationFilePath))
            {
                foundConfigurationFilePlugin = configurationFilePlugin;
                break;
            }
        }

        if (foundConfigurationFilePlugin is null) return null;
        
        var data = foundConfigurationFilePlugin.LoadFile(configurationFilePath);
        LoadedConfigurationFilePlugin = foundConfigurationFilePlugin;
        return data;
    }
    
    public void SaveConfigurationFile(object data, string configurationFilePath = "")
    {
        if (LoadedConfigurationFilePlugin == null)
            throw new Exception("No configuration file plugin loaded.");
        LoadedConfigurationFilePlugin.SaveFile(configurationFilePath, data);
    }
    
    public void NewConfigurationFile(object data, string configurationFilePath = "")
    {
        DefaultConfigurationFilePlugin.NewFile(configurationFilePath, data);
    }
}