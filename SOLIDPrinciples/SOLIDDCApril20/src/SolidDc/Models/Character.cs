using System;

namespace SolidDc.Models
{
    // A simple DC-themed Character model used across examples.
    // Keep this class focused on holding character state (SRP).
    public class Character
    {
        // Public properties represent the character's data
        public string Name { get; set; }
        public int PowerLevel { get; set; }

        public Character(string name, int powerLevel)
        {
            Name = name;
            PowerLevel = powerLevel;
        }

        // A method that returns a short description
        public string Describe() => $"{Name} (Power: {PowerLevel})";

        // Override ToString to provide a useful default representation
        public override string ToString() => Describe();
    }
}
