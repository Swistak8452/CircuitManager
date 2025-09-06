using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using CircuitManager.Models.Converters;

[JsonConverter(typeof(MachineTypeConverter))]
public enum MachineType
{
    [EnumMember(Value = "convoyer")]
    Convoyer,
    [EnumMember(Value = "obrotnica")]
    Obrotnica,
    [EnumMember(Value = "magazyn_palet")]
    MagazynPalet
}