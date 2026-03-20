using System;
using System.Collections.Generic;

class ReferenceType_Mutation_vs_Reassignment
{
    // Demonstrates mutating a list vs reassigning the parameter
    // Show how ref allows caller's reference to be replaced
    // Time: O(n) for operations shown, Space: O(1) extra

    static void Mutate(List<int> list)
    {
        // Mutates the object; caller sees changes
        list.Add(99);
    }

    static void Reassign(List<int> list)
    {
        // Reassigns the local parameter only; caller unaffected
        list = new List<int> { -1, -2 };
    }

    static void ReassignRef(ref List<int> list)
    {
        // Using ref allows caller's variable to be reassigned
        list = new List<int> { 7, 8, 9 };
    }

    static void Main()
    {
        var lst = new List<int> { 1, 2 };
        Mutate(lst);
        Console.WriteLine("After Mutate: " + string.Join(",", lst)); // 1,2,99

        Reassign(lst);
        Console.WriteLine("After Reassign: " + string.Join(",", lst)); // still 1,2,99

        ReassignRef(ref lst);
        Console.WriteLine("After ReassignRef: " + string.Join(",", lst)); // 7,8,9
    }
}