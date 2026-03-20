// 02-Interlocked_CorrectUsage.cs
// TryIncrementIfBelow uses CompareExchange to increment only if current < cap.

using System.Threading;

public class CappedCounter
{
    private int _value;

    public bool TryIncrementIfBelow(int cap)
    {
        while (true)
        {
            int current = Volatile.Read(ref _value);
            if (current >= cap) return false;
            int desired = current + 1;
            // If _value is still 'current', sets it to 'desired' and returns previous
            int original = Interlocked.CompareExchange(ref _value, desired, current);
            if (original == current) return true; // succeeded
            // else loop and retry
        }
    }

    public int Value => Volatile.Read(ref _value);
}