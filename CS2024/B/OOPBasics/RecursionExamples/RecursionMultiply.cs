namespace RecursionExamples
{
    //TODO add mroe recursion examples. can be a fun exercise for myself and students of course.
    public class RecursionMultiply
    {
        public static int Multiply(int x, int y)
        {
            if (y == 1)
            {
                return x; // Base case: When y is 1, return x
            }
            else
            {
                return x + Multiply(x, y - 1); // Recursive call
            }
        }

    }
}
