namespace CircuitManager.Models.Dto;

public class CircuitElementExportDto
{
    public string Name { get; set; } = string.Empty;
    public required MachineTypeDto MachineType { get; set; }
    public List<ComponentExportDto> ComponentList { get; set; } = new();
    public CircuitElementExportDto? NextCircuitElement { get; set; }
}