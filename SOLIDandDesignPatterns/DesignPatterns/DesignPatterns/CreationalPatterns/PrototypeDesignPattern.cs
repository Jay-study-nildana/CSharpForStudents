using SOLIDDesignPrinciples;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.CreationalPatterns
{
    class PrototypeDesignPattern
    {
        //this is just an empty class which came with the file creation
        //this is not actually used anywhere so it does not contain anything.
    }

    //Okay, this design pattern is mainly concerned with copying objects. 

    //TODO - look into ICloneable 
    //TODO - look into Serialization

    //here, we focus on a more direct way of deep copying objects
    //as always, we use the monkey stuff.

    //and we start with an interface
    //I am keeping this as generic as posible
    //so tomorrow, i can consider reusing it in other projects
    //as deep copying is a common activity.
    interface IDeepCopyObjects<T>
    {
        T DeepCopyTheObject();
    }

    //here is a monkey class implementation 
    //with a deep copy functionality built in using 
    public class TheCopierMonkey : MonkeyBaseClass, IDeepCopyObjects<TheCopierMonkey>
    {
        public TheCopierMonkey DeepCopyTheObject()
        {
            var copyMonkey = new TheCopierMonkey();

            copyMonkey.firstNameOfMonkey = this.firstNameOfMonkey;
            copyMonkey.gender = this.gender;
            copyMonkey.lastNameOfMonkey = this.lastNameOfMonkey;
            copyMonkey.lifeStage = this.lifeStage;

            return copyMonkey;
        }
    }
}
