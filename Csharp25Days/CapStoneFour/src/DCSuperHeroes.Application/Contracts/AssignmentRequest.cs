using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Application.Contracts;

public sealed record AssignmentRequest(Guid MissionId, Guid HeroId, SupportRole Role);