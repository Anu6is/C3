using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using C3.Domain.Models;
using C3.Domain.Specifications;
using C3.Application.Services;
using C3.Application.State;
using C3.Presentation.ViewModels;
using C3.Presentation.Mapping;
using C3.Infrastructure.Storage;
using C3.Presentation.Enums;
using C3.Shared;

namespace C3.Pages;

public partial class Home : IDisposable
{
    [CascadingParameter] public WarData? Data { get; set; }
    [CascadingParameter] public bool IsMobile { get; set; }

    [Inject] private BrowserStorageService BrowserStore { get; set; } = default!;
    [Inject] private FilterStateContainer FilterState { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private FactionMemberService MemberService { get; set; } = default!;
    [Inject] private FactionMemberViewModelMapper ViewModelMapper { get; set; } = default!;

    private LoadingState _dataState => Data?.Initialized == true && Data.RivalFaction != null ? LoadingState.Loaded : LoadingState.Loading;
    private MudDataGrid<FactionMemberViewModel>? _dataGrid;
    private MudTextField<string>? _searchField;
    private MudMessageBox? _spyInfoBox;
    private List<FactionMemberViewModel> _viewModels = [];
    private string? _searchInput;
    private CancellationTokenSource? _searchCts;
    private int _lastDataVersion;
    private bool _filterChanged;

    private IReadOnlyCollection<string> SelectedValues { get => FilterState.SelectedValues; set => FilterState.SelectedValues = value; }
    private bool Monitored { get => FilterState.IsMonitored; set => FilterState.IsMonitored = value; }
    private bool? HasHigherStats { get => FilterState.HasHigherStats; set => FilterState.HasHigherStats = value; }
    private string? SearchString { get => _searchInput; set { _searchInput = value; _ = DebouncedSearchAsync(value); } }

    protected override Task OnInitializedAsync() {
        _searchInput = FilterState.SearchString;
        FilterState.OnChange += HandleStateChange;
        _ = Data?.MonitorWarProgressAsync();
        return base.OnInitializedAsync();
    }

    private async Task DebouncedSearchAsync(string? searchTerm) {
        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();
        try { await Task.Delay(300, _searchCts.Token); FilterState.SearchString = searchTerm; } catch (TaskCanceledException) { }
    }

    private void HandleStateChange() { if (_searchInput != FilterState.SearchString) _searchInput = FilterState.SearchString; UpdateTableView(); _filterChanged = true; StateHasChanged(); }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (_dataState == LoadingState.Loaded && _dataGrid != null && _viewModels.Any() && !firstRender && _lastDataVersion == 0)
            await _dataGrid.SetSortAsync(HasHigherStats is not null ? "Total Stats" : "Level", SortDirection.Descending, x => HasHigherStats is not null ? x.BattleStats : x.Level);
    }

    protected override void OnParametersSet() { if (Data?.ChangeTracker.Version != _lastDataVersion) UpdateTableView(); }
    protected override bool ShouldRender() {
        if (!_filterChanged && Data?.ChangeTracker.Version == _lastDataVersion) return false;
        _filterChanged = false; _lastDataVersion = Data?.ChangeTracker.Version ?? 0; return true;
    }

    private async Task OnToggleChanged(bool state) {
        HasHigherStats = HasHigherStats switch { true => null, false => true, _ => false };
        if (_dataGrid != null) await _dataGrid.SetSortAsync(HasHigherStats is not null ? "Total Stats" : "Level", SortDirection.Descending, x => HasHigherStats is not null ? x.BattleStats : x.Level);
    }

    private void UpdateTableView() {
        if (Data?.CurrentUser is null || Data?.RivalFaction?.Members is null) { _viewModels = []; return; }
        var spec = new MemberFilterSpecification().WithStates(SelectedValues).WithMonitoredOnly(Monitored ? Data.WarSession.FactionTargets : [])
            .WithStatComparison(HasHigherStats, Data.CurrentUser.Stats.Total, id => Data.Spies.GetValueOrDefault(id)?.Total ?? 0)
            .WithSearchTerm(FilterState.SearchString, id => Data.Spies.GetValueOrDefault(id)?.Total ?? 0);
        var filtered = MemberService.FilterMembers(Data.RivalFaction.Members, spec);
        var dtos = MemberService.GetMemberDtos(filtered, Data.Spies);
        var monitoredSet = Data.WarSession.FactionTargets.ToHashSet();
        _viewModels = dtos.Select(dto => ViewModelMapper.Map(dto, Data.CurrentUser.Stats, monitoredSet)).ToList();
    }

    private async Task ToggleMonitoringAsync(bool monitor, int userId) {
        if (monitor) Data!.WarSession.FactionTargets.Add(userId); else Data!.WarSession.FactionTargets.Remove(userId);
        FilterState.Targets = Data!.WarSession.FactionTargets;
        await BrowserStore.SaveSessionAsync(Data.WarSession);
        UpdateTableView();
    }

    private void HandleEmptyStateAction()
    {
        if (HasHigherStats.HasValue && (Data?.Spies.Count ?? 0) == 0)
            _ = ShowSpyInfoDialog();
        else
            ClearFilters();
    }

    private void ClearFilters() { FilterState.ResetFilters(); UpdateTableView(); }
    private async Task ShowSpyInfoDialog() { if (_spyInfoBox != null) await _spyInfoBox.ShowAsync(); }
    private async Task FocusSearch() { if (_searchField != null) await _searchField.FocusAsync(); }
    private void ToggleMonitored() => Monitored = !Monitored;
    public void Dispose() { FilterState.OnChange -= HandleStateChange; _searchCts?.Cancel(); _searchCts?.Dispose(); }
}
