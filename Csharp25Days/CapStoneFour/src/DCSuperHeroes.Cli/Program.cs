using System.Text.Json;
using DCSuperHeroes.Application.Interfaces;
using DCSuperHeroes.Application.Services;
using DCSuperHeroes.Core.Events;
using DCSuperHeroes.Infrastructure.Configuration;
using DCSuperHeroes.Infrastructure.Logging;
using DCSuperHeroes.Infrastructure.Persistence;
using DCSuperHeroes.Cli.Menus;

var settings = await LoadSettingsAsync();
var dataDirectory = ResolvePath(settings.Storage.DataDirectory);
var logDirectory = ResolvePath(settings.Storage.LogDirectory);

var service = BuildService(dataDirectory, logDirectory);

if (settings.SeedSampleDataOnStartup)
{
	await service.SeedSampleDataAsync();
}

service.MissionAssigned += OnMissionAssigned;
service.MissionCompleted += OnMissionCompleted;

var menu = new MainMenu(service, settings.RecommendationCount);
await menu.RunAsync();

return;

static IJusticeLeagueService BuildService(string dataDirectory, string logDirectory)
{
	var heroRepository = new HeroRepository(dataDirectory);
	var missionRepository = new MissionRepository(dataDirectory);
	var assignmentRepository = new AssignmentRepository(dataDirectory);
	var logger = new FileLeagueLogger(logDirectory);

	return new JusticeLeagueService(heroRepository, missionRepository, assignmentRepository, logger);
}

static async Task<AppSettings> LoadSettingsAsync()
{
	var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
	if (!File.Exists(settingsPath))
	{
		return new AppSettings();
	}

	var json = await File.ReadAllTextAsync(settingsPath);
	return JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
	{
		PropertyNameCaseInsensitive = true
	}) ?? new AppSettings();
}

static string ResolvePath(string relativePath)
{
	return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relativePath));
}

static void OnMissionAssigned(object? sender, MissionAssignedEventArgs args)
{
	Console.ForegroundColor = ConsoleColor.Green;
	Console.WriteLine($"[EVENT] {args.Hero.Alias} assigned to {args.Mission.CodeName} as {args.Assignment.Role}.");
	Console.ResetColor();
}

static void OnMissionCompleted(object? sender, MissionCompletedEventArgs args)
{
	Console.ForegroundColor = ConsoleColor.Magenta;
	Console.WriteLine($"[EVENT] Mission {args.Mission.CodeName} completed by {string.Join(", ", args.AssignedHeroes.Select(hero => hero.Alias))}.");
	Console.ResetColor();
}
