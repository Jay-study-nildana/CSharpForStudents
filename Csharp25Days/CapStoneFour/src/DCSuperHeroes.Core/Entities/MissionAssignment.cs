using DCSuperHeroes.Core.Common;
using DCSuperHeroes.Core.Enums;

namespace DCSuperHeroes.Core.Entities;

public sealed class MissionAssignment : BaseEntity
{
    public Guid MissionId { get; set; }
    public Guid HeroId { get; set; }
    public SupportRole Role { get; set; }
    public decimal AssignedReadinessScore { get; set; }
    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;
}