using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenAppliance
{
    public class Refrigerator
    {
        //this field can be null. the null symbol (question mark) is used to indicate that.
        public int? ModelOfRefrigerator { get; set; }

        public string BrandOfRefrigerator {  get; set; } //this can also be null. So, you will see a 'warning label' in visual studio.
    }
}
