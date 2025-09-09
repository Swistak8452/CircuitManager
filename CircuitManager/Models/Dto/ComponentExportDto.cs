namespace CircuitManager.Models.Dto;

public class ComponentExportDto
{
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;     // prefiks tagu
    public string Direction { get; set; } = "Input";      // "Input" / "Output"
    public int Index { get; set; }                        // numer w obrębie labela
    public string Tag { get; set; } = string.Empty;       // np. TP1_B1
}
