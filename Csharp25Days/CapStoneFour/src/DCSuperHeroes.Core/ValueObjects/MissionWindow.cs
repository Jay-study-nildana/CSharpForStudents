namespace DCSuperHeroes.Core.ValueObjects;

public readonly record struct MissionWindow(DateTime StartsAtUtc, int DurationHours)
{
    public DateTime EndsAtUtc => StartsAtUtc.AddHours(DurationHours);
}