using System.Collections;
using System.Collections.Generic;
using VillainsGeneric;

namespace IteratorPattern
{
    //class DaysInMonthCollection : IEnumerable<MonthWithDays>
    public class VillainCollection : IEnumerable<Villain>
    {
        public IEnumerator<Villain> GetEnumerator()
        {
            return new VillainEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
