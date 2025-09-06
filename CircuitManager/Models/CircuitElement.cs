using System.ComponentModel.DataAnnotations;

namespace CircuitManager.Models;

public class CircuitElement
{
    [Key]
    public int Id { get; set; }                    // wewnętrzne ID
    public string Name { get; set; } = string.Empty;      // Twój "id" użytkowy
    public MachineType MachineType { get; set; }

    // self-reference (następnik w obwodzie)
    public int? NextCircuitElementId { get; set; }
    public CircuitElement? NextCircuitElement { get; set; }

    // lista komponentów przypięta do elementu (katalogowe komponenty)
    public ICollection<Component> ComponentList { get; set; } = new List<Component>();
}