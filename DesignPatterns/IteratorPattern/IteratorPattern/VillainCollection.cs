using BatmanVillains;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
