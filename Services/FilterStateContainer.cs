using C3.Models;
using System.Linq;
using System.Collections.Generic;

namespace C3.Services;

public class FilterStateContainer
{
    private int? _warId;
    private bool _isOkay;
    private bool _inHospital;
    private bool _isMonitored;
    private bool? _hasHigherStats;
    private string? _searchString;
    private List<int> _targets = [];
    private IReadOnlyCollection<string> _cachedSelectedValues = new List<string>().AsReadOnly();

    public event Action? OnChange;

    public int? WarId
    {
        get => _warId;
        set
        {
            if (_warId != value)
            {
                _warId = value;
                NotifyStateChanged();
            }
        }
    }

    public bool IsOkay
    {
        get => _isOkay;
        set
        {
            if (_isOkay != value)
            {
                _isOkay = value;
                UpdateSelectedValuesCache();
                NotifyStateChanged();
            }
        }
    }

    public bool InHospital
    {
        get => _inHospital;
        set
        {
            if (_inHospital != value)
            {
                _inHospital = value;
                UpdateSelectedValuesCache();
                NotifyStateChanged();
            }
        }
    }

    public bool IsMonitored
    {
        get => _isMonitored;
        set
        {
            if (_isMonitored != value)
            {
                _isMonitored = value;
                NotifyStateChanged();
            }
        }
    }

    public bool? HasHigherStats
    {
        get => _hasHigherStats;
        set
        {
            if (_hasHigherStats != value)
            {
                _hasHigherStats = value;
                NotifyStateChanged();
            }
        }
    }

    public string? SearchString
    {
        get => _searchString;
        set
        {
            if (_searchString != value)
            {
                _searchString = value;
                NotifyStateChanged();
            }
        }
    }

    public List<int> Targets
    {
        get => _targets;
        set
        {
            _targets = value;
            NotifyStateChanged();
        }
    }

    public IReadOnlyCollection<string> SelectedValues
    {
        get => _cachedSelectedValues;
        set
        {
            var isOkay = value.Contains("Okay");
            var inHospital = value.Contains("Hospital");

            if (isOkay != _isOkay || inHospital != _inHospital)
            {
                _isOkay = isOkay;
                _inHospital = inHospital;
                UpdateSelectedValuesCache();
                NotifyStateChanged();
            }
        }
    }

    public void FromOptions(CustomFilterOptions options)
    {
        _warId = options.WarId;
        _isOkay = options.IsOkay;
        _inHospital = options.InHospital;
        _isMonitored = options.IsMonitored;
        _hasHigherStats = options.HasHigherStats;
        _searchString = options.FilterString;
        _targets = options.Targets ?? [];
        UpdateSelectedValuesCache();
        NotifyStateChanged();
    }

    public CustomFilterOptions ToOptions()
    {
        return new CustomFilterOptions
        {
            WarId = _warId,
            IsOkay = _isOkay,
            InHospital = _inHospital,
            IsMonitored = _isMonitored,
            HasHigherStats = _hasHigherStats,
            FilterString = _searchString,
            Targets = _targets
        };
    }

    private void UpdateSelectedValuesCache()
    {
        var selected = new List<string>();
        if (_isOkay) selected.Add("Okay");
        if (_inHospital) selected.Add("Hospital");
        _cachedSelectedValues = selected.AsReadOnly();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
