using System;

namespace BatmanVillains
{
    //abstract. That means, this clas cannot be used directly. 
    public abstract class CoreCharacter
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string AlterEgo { set; get; }
    }
}
