using System;

namespace NathanColeman.IndieGameDev.Utils;

public static class DateOnlyExtensions
{
    public static string DayOrdinal(this DateOnly date)
    {
        ArgumentNullException.ThrowIfNull(date);

        var number = date.Day;

        if (number <= 0) return number.ToString();

        if (number % 100 is 11 or 12 or 13) return number + "th";

        return (number % 10) switch
        {
            1 => number + "st",
            2 => number + "nd",
            3 => number + "rd",
            _ => number + "th"
        };
    }
}
