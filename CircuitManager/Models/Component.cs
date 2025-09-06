namespace CircuitManager.Models;

public class Component
{
    public int Id { get; set; }            // techniczne ID (dla EF i relacji)
    public string Name { get; set; } = string.Empty;
    public string Shortcut { get; set; } = string.Empty;
    
    public override string ToString()
    {
        return $"{Shortcut} ({Name})";
    }
    
    // na potrzeby wielu-do-wielu z CircuitElement:
    public ICollection<CircuitElement> CircuitElements { get; set; } = new List<CircuitElement>();
    
}