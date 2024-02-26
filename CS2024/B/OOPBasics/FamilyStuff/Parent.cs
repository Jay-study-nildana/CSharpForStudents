using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyStuff
{
    //simple inheritance
    public class Parent : FamilyMember, IParentActivities
    {
        public string Age { get; set; }

        public Parent(string Name, string Description, string Age) { 

            this.Description = Description;
            this.Name = Name; 
            this.Age = Age;
        
        }

        public void DisplayFamilyMemberDetails()
        {
            var OutputString = "Name : " + this.Name + " Age: " + this.Age + " Description : " + this.Description;
            Console.WriteLine(OutputString);
        }

        public Parent() { 
        }

        public void HobbiesDisplay()
        {
            Console.WriteLine("Parents are boring and don't have any hobbies");
        }

        public virtual void VacationPlanDisplay()
        {
            Console.WriteLine("Parents simply want to chill out at home. That is the vacation plan");
        }

        public sealed override void FamilyMoto()
        {
            Console.WriteLine("Family Stays Together");
        }

        public void TellChildrenToStudy()
        {
            Console.WriteLine("Everyday, you have to study for about 2 hours. And, Also do Homework");
        }

        public void PrayToGodForChildWelfare()
        {
            throw new NotImplementedException();
        }
    }
}
