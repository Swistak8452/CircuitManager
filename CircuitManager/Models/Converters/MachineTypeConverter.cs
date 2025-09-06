namespace CircuitManager.Models.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

public class MachineTypeConverter : JsonConverter<MachineType>
{
    private static readonly Dictionary<MachineType, string> ToText = new()
    {
        { MachineType.Convoyer, "convoyer" },
        { MachineType.Obrotnica, "obrotnica" },
        { MachineType.MagazynPalet, "magazyn_palet" }
    };

    private static readonly Dictionary<string, MachineType> FromText = new(StringComparer.OrdinalIgnoreCase)
    {
        { "convoyer", MachineType.Convoyer },
        { "obrotnica", MachineType.Obrotnica },
        { "magazyn_palet", MachineType.MagazynPalet }
    };

    public override MachineType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => FromText.TryGetValue(reader.GetString()!, out var value)
            ? value
            : throw new JsonException("Nieznany typ");

    public override void Write(Utf8JsonWriter writer, MachineType value, JsonSerializerOptions options)
        => writer.WriteStringValue(ToText[value]);
}
