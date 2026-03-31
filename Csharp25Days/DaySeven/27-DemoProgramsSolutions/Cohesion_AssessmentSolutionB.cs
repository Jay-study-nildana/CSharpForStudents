NumberMath math = new NumberMath(() => 10);
math.ShowDefaultNumber();
NumberMath math2 = new NumberMath(DefaultNumberOne);
math2.ShowDefaultNumber();
NumberMath math3 = new NumberMath(DefaultNumberTwo);
math3.ShowDefaultNumber();

static int DefaultNumberOne()
{
    return 25;
}

static int DefaultNumberTwo()
{
    Console.WriteLine("Calculating default number...");
    int somenumber = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
    return somenumber;
}

public class NumberMath
{
    private readonly Func<int> _defaultNumber;
    public NumberMath(Func<int> GetDefaultNumber)
    {
        _defaultNumber = GetDefaultNumber;
    }

    public void ShowDefaultNumber()
    {
        Console.WriteLine($"The default number is: {_defaultNumber()}");
    }
}