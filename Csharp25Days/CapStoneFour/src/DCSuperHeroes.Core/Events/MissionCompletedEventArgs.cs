using DCSuperHeroes.Core.Entities;

namespace DCSuperHeroes.Core.Events;

public sealed class MissionCompletedEventArgs : EventArgs
{
    public MissionCompletedEventArgs(Mission mission, IReadOnlyList<Hero> assignedHeroes)
    {
        Mission = mission;
        AssignedHeroes = assignedHeroes;
    }

    public Mission Mission { get; }
    public IReadOnlyList<Hero> AssignedHeroes { get; }
}