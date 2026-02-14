using System.Text.Json.Serialization;

namespace C3.Models;

public class CustomFilterOptions
{
    public int? WarId { get; set; }
    public bool IsOkay { get; set; }
    public bool InHospital { get; set; }
    public bool IsMonitored { get; set; }
    public bool? HasHigherStats { get; set; }
    public string? FilterString { get; set; }
    public List<int> Targets { get; set; } = [];

    public CustomFilterOptions() { }

    public void UpdateFrom(CustomFilterOptions? filterOptions)
    {
        if (filterOptions is null) return;

        WarId = filterOptions.WarId;
        IsOkay = filterOptions.IsOkay;
        InHospital = filterOptions.InHospital;
        IsMonitored = filterOptions.IsMonitored;
        HasHigherStats = filterOptions.HasHigherStats;
        FilterString = filterOptions.FilterString;
        Targets = filterOptions.Targets;
    }
}
