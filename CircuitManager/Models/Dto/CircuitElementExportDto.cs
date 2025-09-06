namespace CircuitManager.Models.Dto;

public class CircuitElementExportDto
{
    public string Name { get; set; } = string.Empty;
    public MachineType MachineType { get; set; }
    public List<ComponentExportDto> ComponentList { get; set; } = new();
    public CircuitElementExportDto? NextCircuitElement { get; set; }
}