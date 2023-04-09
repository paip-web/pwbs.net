using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PWBS.ConfigFile.PropertyClasses;

public class PWBSCommandPropertyJsonConverter : JsonConverter<PWBSCommandProperty>
{
    public override PWBSCommandProperty Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        // JsonConverter and array are a joke like all of System.Text.Json
        throw new JsonException();
    }

    public override void Write(
        Utf8JsonWriter writer,
        PWBSCommandProperty commandProperty,
        JsonSerializerOptions options)
    {
        if (commandProperty.CommandStrings.Count == 1)
        {
            writer.WriteStringValue(commandProperty.CommandStrings[0]);
        }
        else
        {
            writer.WriteStartArray();
            foreach (var command in commandProperty.CommandStrings)
            {
                writer.WriteStringValue(command);
            }
            writer.WriteEndArray();
        }
    }
}

[JsonConverter(typeof(PWBSCommandPropertyJsonConverter))]
public class PWBSCommandProperty
{
    [JsonIgnore]
    public List<string> CommandStrings = new();

    public object? Command
    {
        get
        {
            if (CommandStrings.Count == 1) return CommandStrings[0];
            return CommandStrings;
        }
    }

    public PWBSCommandProperty(string command)
    {
        CommandStrings.Add(command);
    }

    public PWBSCommandProperty(IEnumerable<string> commands)
    {
        CommandStrings = new List<string>(commands);
    }
}