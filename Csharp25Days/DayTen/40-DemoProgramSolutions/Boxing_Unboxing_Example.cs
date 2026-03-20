using System;
using System.Collections;

class Boxing_Unboxing_Example
{
    // Small struct used in a non-generic collection causes boxing
    public struct SmallPoint { public int X; public int Y; public SmallPoint(int x,int y){X=x;Y=y;} }

    static void Main()
    {
        var list = new ArrayList(); // non-generic -> holds objects
        SmallPoint p = new SmallPoint(1,2);
        list.Add(p); // boxing occurs: struct copied into object on heap
        object boxed = list[0];
        var unboxed = (SmallPoint)boxed; // unboxing creates a copy

        Console.WriteLine($"boxed type: {boxed.GetType().Name}, unboxed X={unboxed.X}");
        Console.WriteLine("Boxing causes heap allocation and copy. Avoid by using generics (List<T>) or keeping value types in arrays.");
    }
}