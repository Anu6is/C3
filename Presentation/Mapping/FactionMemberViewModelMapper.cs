using MudBlazor;
using C3.Application.DTOs;
using C3.Application.Services.Display;
using C3.Presentation.ViewModels;
using C3.Domain.Models;

namespace C3.Presentation.Mapping;

public class FactionMemberViewModelMapper
{
    private readonly NumberFormattingService _formatter;
    private readonly ColorMappingService _colorMapper;

    public FactionMemberViewModelMapper(
        NumberFormattingService formatter,
        ColorMappingService colorMapper)
    {
        _formatter = formatter;
        _colorMapper = colorMapper;
    }

    public FactionMemberViewModel Map(
        FactionMemberDto dto,
        BattleStats userStats,
        HashSet<int> monitoredIds)
    {
        var statsDisplay = dto.SpyData is not null
            ? _formatter.FormatBattleStat(dto.SpyData.Total)
            : "Unavailable";

        var statsColor = dto.SpyData is not null
            ? _colorMapper.GetBattleStatColor(dto.SpyData.Total, userStats.Total)
            : Color.Info;

        return new FactionMemberViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Level = dto.Level,
            StatusDisplay = dto.Status,
            StatusColor = _colorMapper.GetStatusColor(dto.Status),
            StateDisplay = dto.State,
            StateColor = _colorMapper.GetStateColor(dto.State),
            StateIcon = _colorMapper.GetStateIcon(dto.State),
            StateUntil = dto.StateUntil,
            BattleStats = dto.SpyData?.Total ?? 0,
            BattleStatsDisplay = statsDisplay,
            BattleStatsColor = statsColor,
            HasSpyData = dto.SpyData is not null,

            Strength = dto.SpyData?.Strength ?? 0,
            StrengthText = dto.SpyData is not null ? _formatter.FormatBattleStat(dto.SpyData.Strength) : "Unknown",
            StrengthColor = dto.SpyData is not null ? _colorMapper.GetBattleStatColor(dto.SpyData.Strength, userStats.Strength) : Color.Default,

            Defense = dto.SpyData?.Defense ?? 0,
            DefenseText = dto.SpyData is not null ? _formatter.FormatBattleStat(dto.SpyData.Defense) : "Unknown",
            DefenseColor = dto.SpyData is not null ? _colorMapper.GetBattleStatColor(dto.SpyData.Defense, userStats.Defense) : Color.Default,

            Speed = dto.SpyData?.Speed ?? 0,
            SpeedText = dto.SpyData is not null ? _formatter.FormatBattleStat(dto.SpyData.Speed) : "Unknown",
            SpeedColor = dto.SpyData is not null ? _colorMapper.GetBattleStatColor(dto.SpyData.Speed, userStats.Speed) : Color.Default,

            Dexterity = dto.SpyData?.Dexterity ?? 0,
            DexterityText = dto.SpyData is not null ? _formatter.FormatBattleStat(dto.SpyData.Dexterity) : "Unknown",
            DexterityColor = dto.SpyData is not null ? _colorMapper.GetBattleStatColor(dto.SpyData.Dexterity, userStats.Dexterity) : Color.Default,

            IsMonitored = monitoredIds.Contains(dto.Id),
            AttackUrl = $"https://www.torn.com/loader.php?sid=attack&user2ID={dto.Id}",
            ProfileUrl = $"https://www.torn.com/profiles.php?XID={dto.Id}"
        };
    }
}
