using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Open–closed principle - In object -oriented programming, the open–closed principle states "software entities (classes, modules, functions, etc.) should be open for extension, but closed for modification"; [1] that is, such an entity can allow its behaviour to be extended without modifying its source code. 
//The idea here is, once a class has been built (and probably shipped) we should stop modifying it.
//Rather we should design classes in such a way that, when we want extra stuff - behavior or functions - we extend the base class
//that way, different parts of the software that are dependent on the base class don't break unintentionally because you kept modifying it

//in this example, let's suppose Batman already has a vehicle to fight on land.
//Then, he wants to start fighting in the sky or the ice
//He will start modifying his existing vehicle. That would be bad. What if flying stops the vehicle from driving on road?
//So, Batman should ideally build a second vehicle based on the base vehicle
//alter this second vehicle which has all the features of the existing base vehicle plus modifications needed to fly and so on.

//https://en.wikipedia.org/wiki/Open%E2%80%93closed_principle

namespace SolidWithSuperHeroes
{
    public class OinSOLID 
    {

    }

    //Here, anytime a new terrain type has to be considerd
    //like say, Batman has a fight in the Ice and he wants Ice Boots
    //we need to keep adding if else and so on.
    //We have to keep editing original source code
    //which is just awful programming.
    public class HandleTerrainOfBattleBadly
    {
        public string? TerrainType;

        public virtual void ChooseBatVehicle()
        {
            if(TerrainType == "Ground")
            {
                Console.WriteLine("Choosing Batmobile for Ground Battle");
            }
            else if(TerrainType =="Streets")
            {
                Console.WriteLine("Choosing BatPod for Streets Battle");
            }
            else
            {
                Console.WriteLine("Choosing BatPlane for Air Battle");
            }
        }
    }

    public class HandleTerrainOfBattleProperly
    {
        //by default, batman always uses the Batmobile.
        public virtual void ChooseBatVehicle()
        {
            Console.WriteLine("Choosing Batmobile for Ground Battle");
        }
    }

    public class HandleTerrainOfBattleProperlyStreets : HandleTerrainOfBattleProperly
    {
        public override void ChooseBatVehicle()
        {
            Console.WriteLine("Choosing BatPod for Street Battle");
        }
    }

    public class HandleTerrainOfBattleProperlyAir : HandleTerrainOfBattleProperly
    {
        public override void ChooseBatVehicle()
        {
            Console.WriteLine("Choosing BatPlane for Street Battle");
        }
    }

    //we can keep extending the base call for as many scenarios as we want.
    public class HandleTerrainOfBattleProperlyIce : HandleTerrainOfBattleProperly
    {
        public override void ChooseBatVehicle()
        {
            Console.WriteLine("Choosing Ice Boots for Ice Battle Battle");
        }
    }


}
