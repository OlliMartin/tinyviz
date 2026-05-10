using System.ComponentModel.DataAnnotations;

namespace TinyViz.Contracts.Model.ChartSpecification;

[UsedImplicitly]
public record TraceDescriptor
{
    [Required]
    public required string TypeName { get; init; }
}
