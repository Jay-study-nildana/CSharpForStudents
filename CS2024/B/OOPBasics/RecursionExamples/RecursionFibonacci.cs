using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursionExamples
{
    public class RecursionFibonacci
    {
        public static int GetNthFibonacci(int n)
        {
            if (n <= 1)
                return n;
            else
                return GetNthFibonacci(n - 1) + GetNthFibonacci(n - 2);
        }

    }
}
