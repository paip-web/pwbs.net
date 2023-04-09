using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using PWBS.ConfigFile.PropertyClasses;

namespace PWBS.ConfigFile;

/// <summary>
/// PWBS Configuration File Object
/// </summary>
public class PWBSConfigurationFile
{
    /// <summary>
    /// List of Commands
    /// </summary>
    public Dictionary<string, PropertyClasses.PWBSCommandProperty> commands { get; set; } = new();
    
    /// <summary>
    /// Create PWBS Configuration File Object Based on provided JSON String
    /// </summary>
    /// <param name="json">JSON String</param>
    /// <returns>PWBS Configuration File Object</returns>
    public static PWBSConfigurationFile? createPWBSConfigurationFileBasedOnJson(string json)
    {
        return CustomDeserializer(
            JsonNode.Parse(
                json,
                new JsonNodeOptions(),
                new JsonDocumentOptions() {
                    AllowTrailingCommas = true,
                    CommentHandling = JsonCommentHandling.Skip,
                    MaxDepth = int.MaxValue,
                }
            )
        );
    }
    
    /// <summary>
    /// Create JSON String from PWBS Configuration File Object
    /// </summary>
    /// <param name="pwbsConfigurationFile">PWBS Configuration File Object</param>
    /// <returns>JSON String</returns>
    public static string createJsonBasedOnPWBSConfigurationFile(PWBSConfigurationFile pwbsConfigurationFile)
    {
        return JsonSerializer.Serialize(pwbsConfigurationFile);
    }

    /// <summary>
    /// Custom Assert Function for Deserialization
    /// </summary>
    /// <param name="condition">Condition that should be true</param>
    /// <param name="message">Optional message for if condition is not met</param>
    /// <returns></returns>
    /// <exception cref="PWBSConfigurationFileDeserializationException">Assertion Error</exception>
    private static bool DeserializationAssert(bool condition, string message = "")
    {
        if (!condition) throw new PWBSConfigurationFileDeserializationException(
            string.IsNullOrWhiteSpace(message) ? "Assertion Error" : $"Assertion Error: {message}"
        );
        return !condition;
    }

    /// <summary>
    /// Custom deserializer for PWBSConfigurationFile
    /// Made because default deserializer doesn't work at all with anything.
    /// </summary>
    /// <param name="deserializedObject">Deserialized Object</param>
    /// <returns>Correctly Deserialized Object</returns>
    private static PWBSConfigurationFile? CustomDeserializer(JsonNode? deserializedObject)
    {
        // Alias for shorter code
        var dObj = deserializedObject;
        // Check if deserialized object is not null
        if (
            DeserializationAssert(dObj is not null, "Deserialized Object is null")
            || dObj is null
        ) return null;
        // Create object for serialization
        var retObj = new PWBSConfigurationFile
        {
            commands = new Dictionary<string, PWBSCommandProperty>()
        };
        // // Deserialize Object
        // $.commands
        var dObjCommands = dObj["commands"];
        if (
            DeserializationAssert(dObjCommands is not null, "commands is null")
            || dObjCommands is null
        ) return null;
        var dObjCommandsDict = dObjCommands.Deserialize<Dictionary<string, JsonNode?>>();
        if (
            DeserializationAssert(dObjCommandsDict is not null, "commands is null")
            || dObjCommandsDict is null
        ) return null;
        // $.commands.*
        foreach (var commandKeyValuePair in dObjCommandsDict)
        {
            var commandKey = commandKeyValuePair.Key;
            var commandValue = commandKeyValuePair.Value;
            // Ignore nulls
            if (commandValue is null) continue;
            // $.commands.* -> Single Commands
            try
            {
                if (commandValue.AsValue().TryGetValue(out string? commandValueString))
                {
                    retObj.commands.Add(commandKey, new PWBSCommandProperty(commandValueString));
                    continue;
                }
            }
            catch (InvalidOperationException)
            {
            }
            // $.commands.* -> Multi Commands
            try
            {
                if (commandValue.AsArray().Count > 0)
                {
                    if (commandValue is null) throw new InvalidOperationException();
                    IEnumerable<string> commandValueStringList = commandValue
                        .AsArray()
                        .Where(node => node is not null)
                        .Where(node => {
                            if (node is null) return false;
                            try
                            {
                                node.AsValue();
                                return true;
                            }
                            catch (InvalidOperationException)
                            {
                                return false;
                            }
                        })
                        .Select(node => node!.AsValue())
                        .Where(node => node.TryGetValue(out string? _))
                        .Select(node => node.GetValue<string>());
                    retObj.commands.Add(commandKey, new PWBSCommandProperty(commandValueStringList));
                    continue;
                }
            }
            catch (InvalidOperationException)
            {
            }
            // $.commands.* -> Invalid
            // DeserializationAssert(false, $"Invalid Command Value: {commandValue}");
        }

        // Return
        return retObj;
    }
}

public class PWBSConfigurationFileDeserializationException : ArgumentException
{
    public PWBSConfigurationFileDeserializationException(string message) : base(message)
    {
    }
}
