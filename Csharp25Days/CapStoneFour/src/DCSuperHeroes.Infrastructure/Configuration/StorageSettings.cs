namespace DCSuperHeroes.Infrastructure.Configuration;

public sealed record StorageSettings
{
    public string DataDirectory { get; init; } = "data";
    public string LogDirectory { get; init; } = "logs";
}