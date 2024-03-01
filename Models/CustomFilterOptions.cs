using System.Text.Json.Serialization;

namespace C3.Models;

public class CustomFilterOptions
{
    public static CustomFilterOptions Instance { get; private set; } = new();

    public int? WarId { get; set; }
    public bool IsOkay { get; set; }
    public bool InHospital { get; set; }
    public bool IsMonitored { get; set; }
    public bool? HasHigherStats { get; set; }
    public string? FilterString { get; set; }
    public List<int> Targets { get; set; } = [];

    [JsonConstructor]
    private CustomFilterOptions() { }

    public static CustomFilterOptions FromCache(CustomFilterOptions? filterOptions)
    {
        Instance = filterOptions ?? new();

        return Instance;
    }
}
