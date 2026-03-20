namespace DCSuperHeroes.Infrastructure.Configuration;

public sealed record AppSettings
{
    public StorageSettings Storage { get; init; } = new();
    public bool SeedSampleDataOnStartup { get; init; } = true;
    public int RecommendationCount { get; init; } = 3;
}