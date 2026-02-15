using System.Text.Json.Serialization;

namespace C3.Application.DTOs;

public class CustomFilterOptions
{
    public int? WarId { get; set; }
    public bool IsOkay { get; set; }
    public bool InHospital { get; set; }
    public bool IsMonitored { get; set; }
    public bool? HasHigherStats { get; set; }
    public string? FilterString { get; set; }
    public List<int> Targets { get; set; } = [];

    [JsonConstructor]
    public CustomFilterOptions() { }
}
