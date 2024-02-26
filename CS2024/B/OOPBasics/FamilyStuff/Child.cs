using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyStuff
{
    //this would be an example of Hierarchical inheritance
    //Child inherits from Parent
    //Parent inherits from FamilyMember
    public class Child : Parent
    {
        public string SchoolOfStudying { get; set; }

        public Child()
        { }

        //using the base keyword to use the constructor of the base class
        public Child(string Name, string Description, string Age,string SchoolOfStudying) : base(Name,Description,Age)
        {
            this.SchoolOfStudying = SchoolOfStudying;
        }

        public void DisplayFamilyMemberDetai()
        {
            base.DisplayFamilyMemberDetails(); //using base keyword to access base class function to show fields in base class
            //show the details available only in child
            Console.WriteLine("School : " + SchoolOfStudying);

        }

        //the usage of the new keyword hides the base class method
        public new void HobbiesDisplay()
        {
            Console.WriteLine("Hobbies : Chess, Sleeping and Making Fun of Parents");
        }

        public override void VacationPlanDisplay()
        {
            Console.WriteLine("Children want to go to Egypt and have a grand adventure involving mummies. Hopefully, Parents will just stay back at the hotel room");
        }

        //child cannot over ride the base class method. it's sealed.
        //public override void FamilyMoto()
        //{
        //    Console.WriteLine("Family Stays Together");
        //}

    }
}
