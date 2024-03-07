using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenAppliance
{
    public class GenericAppliance
    {
        //the generic method that works with both Electric Cooker and also Water Bottle class
        public void DisplayApplianceDetails<T>(T genericobjectone) where T : class
        {
            //check for type and accordingly do the display
            if(genericobjectone.GetType() == typeof(ElectricCooker))
            {
                var ElectricCookerObject = genericobjectone as ElectricCooker;
                Console.WriteLine("Power Source : " + ElectricCookerObject.PowerSource + ". Time for Cooking : " + ElectricCookerObject.TimeToCook);
            }
            if (genericobjectone.GetType() == typeof(WaterBottle))
            {
                var WaterBottleObject = genericobjectone as WaterBottle;
                Console.WriteLine("Material of Bottle: "+ WaterBottleObject.MaterialType + ". Volume : "+ WaterBottleObject.VolumeHolds);
            }
        }
    }
}
