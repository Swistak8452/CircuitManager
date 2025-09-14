using System.ComponentModel.DataAnnotations;

namespace CircuitManager.Models;

// Maszyna - element układu
public class CircuitElement
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MachineTypeId { get; set; }
    public MachineType? MachineType { get; set; }
    public int? NextCircuitElementId { get; set; }
    public CircuitElement? NextCircuitElement { get; set; }
    public ICollection<Component> ComponentList { get; set; } = new List<Component>();
}