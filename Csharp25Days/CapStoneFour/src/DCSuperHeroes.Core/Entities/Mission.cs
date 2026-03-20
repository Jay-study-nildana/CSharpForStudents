using DCSuperHeroes.Core.Common;
using DCSuperHeroes.Core.Enums;
using DCSuperHeroes.Core.ValueObjects;

namespace DCSuperHeroes.Core.Entities;

public sealed class Mission : BaseEntity
{
    public string CodeName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public ThreatLevel ThreatLevel { get; set; }
    public MissionStatus Status { get; set; } = MissionStatus.Draft;
    public int RequiredTeamSize { get; set; } = 1;
    public bool RequiresMysticSupport { get; set; }
    public bool RequiresStealth { get; set; }
    public MissionWindow Window { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string OutcomeSummary { get; set; } = string.Empty;

    public bool IsOpen => Status is MissionStatus.Draft or MissionStatus.Ready or MissionStatus.Active;

    public void UpdateStatusFromAssignments(int assignmentCount)
    {
        if (Status is MissionStatus.Completed or MissionStatus.Aborted)
        {
            return;
        }

        Status = assignmentCount >= RequiredTeamSize
            ? MissionStatus.Ready
            : MissionStatus.Draft;
    }
}