using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidWithSuperHeroes
{
    internal class IinSOLID
    {
    }

    //Interface segregation principle - the interface segregation principle (ISP) states that no code should be forced to depend on methods it does not use.
    //To me, this is similar to the The single-responsibility principle (SRP), applied to interfaces. 
    //the ultimate goal is to ensure that every interface has a single behavior. Later, you can build your classes or other interfaces by combining different interfaces as per the required purpose of coding.

    //Here, we return to something similar to what Bad Batman was doing in SRP example.

    public interface IBatmanDoingBadISPOne
    {
        //this is actually okay. Exactly one behavior
        void BeatUpBadGuys();
    }

    public interface IBatmanDoingBadISPTwo
    {
        //This is bad for so many reasons.
        //BeatUpBadGuys is repeated from the above interface. Which is not cool at all.
        void BeatUpBadGuys();
        //also, this interface has more than one behavior, which is also against the Inteface Segregation Principle
        void FixTheBatMobile();
        void DateCatWoman();
        void AlsoTryDatingWonderWoman();
    }

    //Let's do this the right way.
    //every behavior is now a separate interface.

    public interface IBatmanProperISPOne
    {
        void BeatUpBadGuys();
    }

    public interface IBatmanProperISPTwo
    {
        void FixTheBatMobile();
    }

    public interface IBatmanProperISPThree
    {
        void DateCatWoman();
    }

    public interface IBatmanProperISPFour
    {
        void AlsoTryDatingWonderWoman();
    }

    //now can combine like this.

    //here is Batman when he is busy crime fighting.
    public class BatmanDuringCrimeFighting : IBatmanProperISPOne, IBatmanProperISPTwo
    {
        public void BeatUpBadGuys()
        {
            Console.WriteLine("Take That Joker!!! And Two Face, with Two Punches, on Punch on both sides");
        }

        public void FixTheBatMobile()
        {
            Console.WriteLine("Alfred, come and give me spanner number 5");
        }
    }

    //here is Batman, just chilling when he is enjoying an off day.
    public class BatmanDuringOffDay : IBatmanProperISPThree
    {
        public void DateCatWoman()
        {
            Console.WriteLine("Let's keep the suits on this time");
        }
    }

    //here is Batman, feeling a little weird, trying to date Wonder Woman, for some bizarre reason
    public class BatmanFeelingCrazy : IBatmanProperISPFour
    {
        public void AlsoTryDatingWonderWoman()
        {
            Console.WriteLine("I hope Superman does not find out, Diana");
        }
    }
}
