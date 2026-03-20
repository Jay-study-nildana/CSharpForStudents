using System;

class Grade_Calculator_Switch
{
    // Map numeric score to letter grade using switch expression
    // Control flow: switch on tens-place
    // Time: O(1), Space: O(1)
    static string Grade(int score)
    {
        if (score < 0 || score > 100) throw new ArgumentOutOfRangeException(nameof(score));

        return (score / 10) switch
        {
            10 => "A", // 100
            9 => "A",
            8 => "B",
            7 => "C",
            6 => "D",
            _ => "F"
        };
    }

    static void Main()
    {
        Console.WriteLine(Grade(95)); // A
        Console.WriteLine(Grade(83)); // B
        Console.WriteLine(Grade(59)); // F
    }
}