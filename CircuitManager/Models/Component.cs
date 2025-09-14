namespace CircuitManager.Models;

// Element interfejsu
public class Component
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public IODirection Direction { get; set; } = IODirection.Input;
    public override string ToString() => $"{Label} ({Name})";
    public ICollection<CircuitElement> CircuitElements { get; set; } = new List<CircuitElement>();
}

// Typ interfejsu
public enum IODirection
{
    Input = 0,
    Output = 1
}