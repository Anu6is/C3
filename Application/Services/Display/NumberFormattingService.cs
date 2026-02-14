namespace C3.Application.Services.Display;

public class NumberFormattingService
{
    public string FormatBattleStat(ulong value)
    {
        if (value == 0) return "Unknown";

        return value switch
        {
            >= 1_000_000_000 => $"{(value / 1_000_000_000.0):0.#}B",
            >= 1_000_000 => $"{(value / 1_000_000.0):0.#}M",
            >= 1_000 => $"{(value / 1_000.0):0.#}K",
            _ => value.ToString()
        };
    }
}
