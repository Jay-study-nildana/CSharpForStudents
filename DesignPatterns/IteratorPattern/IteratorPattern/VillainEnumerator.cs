using BatmanVillains;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillainsGeneric;

namespace IteratorPattern
{
    //class DaysInMonthEnumerator : IEnumerator<MonthWithDays>
    public class VillainEnumerator : IEnumerator<Villain>
    {
        //keeps track of number of villains that have been added or created for the collection
        int numberOfVillains = 0;

        //a new villain is created every time.
        public Villain Current => new BatVillainGenerator().getRandomVillain();

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            //nothing to dispose
        }

        public bool MoveNext()
        {
            numberOfVillains++;

            //at most we can have 5 villains
            return numberOfVillains < 5;
        }

        public void Reset()
        {
            numberOfVillains = 0;
        }
    }
}
