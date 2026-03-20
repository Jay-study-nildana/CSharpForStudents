using DCSuperHeroes.Core.Entities;

namespace DCSuperHeroes.Core.Events;

public sealed class MissionAssignedEventArgs : EventArgs
{
    public MissionAssignedEventArgs(Mission mission, Hero hero, MissionAssignment assignment)
    {
        Mission = mission;
        Hero = hero;
        Assignment = assignment;
    }

    public Mission Mission { get; }
    public Hero Hero { get; }
    public MissionAssignment Assignment { get; }
}