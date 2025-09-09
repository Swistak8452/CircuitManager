namespace CircuitManager.Models;

public class MachineType
{
    public int Id { get; set; }
    public string Label { get; set; } = "";
    public string Name { get; set; } = "";
    public override string ToString() => $"{Label} ({Name})";
}