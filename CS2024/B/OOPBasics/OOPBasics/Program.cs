// See https://aka.ms/new-console-template for more information
using BakeryandSweets;
using FamilyStuff;
using OOPBasics;
using RecursionExamples;
using SuperHeroClassLibrary;

//uncomment and comment specific sections for specific outputs
//if 'build' of the solution is taking too long, you can always uncheck and check projects not needed in the "Startup Projects" section.

//SuperHeroClassRelatedOutputs();
//FictionalDetectiveClassRelatedOutputs();
//BakeryStoreClassRelatedOutputs();
//RecursionRelatedOutputs();
//TypeConversionOutputs();
//ConstructorsOutputs();
//InheritanceOutputs();
AbstractClassesInterfaces();

Console.WriteLine("Hello, World!");

# region Super Hero Class 

void SuperHeroClassRelatedOutputs()
{
    //These are classes from the Class Library Project

    Hero batman = new Hero();
    Villain joker = new Villain();

    batman.Name = "Batman";
    batman.PrimaryPower = "Money";
    batman.SecondaryPower = "Expert Hand to Hand Combat";

    joker.Name = "Joker";
    joker.PrimaryPower = "Insanity";
    joker.SecondaryPower = "Multiple Origin Stories";

    var BatmanString = "Name : " + batman.Name + " Powers : " + batman.PrimaryPower + ", " + batman.SecondaryPower;
    Console.WriteLine(BatmanString);
    var JokerString = "Name : " + joker.Name + " Powers : " + joker.PrimaryPower + ", " + joker.SecondaryPower;
    Console.WriteLine(JokerString);
}

# endregion

#region Fictional Detective Class

void FictionalDetectiveClassRelatedOutputs()
{
    //This class defined locally

    FictionalDetective SherlockHolmes = new FictionalDetective();

    //assigning values to the fields using the dot operator

    SherlockHolmes.Name = "Sherlock Holmes";
    SherlockHolmes.PrimaryPower = "Deductive Reasoning";
    SherlockHolmes.SecondaryPower = "John Watson";

    //SherlockHolmes.SomeRandomNumber = 50; //this line is not possible because it is private.

    //set the private value using the method 
    SherlockHolmes.SetSomeRandomNumber(20);


    //getting values from the fields, and also building a string that can be used to display
    var SherlockHolmesString = "Name : " + SherlockHolmes.Name + " Powers : " + SherlockHolmes.PrimaryPower + ", " + SherlockHolmes.SecondaryPower;
    Console.WriteLine(SherlockHolmesString);

    //getting values from the fields, and also building a string that can be used to display
    //notice how we have to use the method to get the private field
    var SherlockHolmesStringPart2 = "Name : " + SherlockHolmes.Name + " Powers : " + SherlockHolmes.PrimaryPower + ", " + SherlockHolmes.SecondaryPower + ", Some Random Number : " + SherlockHolmes.GetSomeRandomNumber().ToString();
    Console.WriteLine(SherlockHolmesStringPart2);

    //now showing the value from the static property
    Console.WriteLine("The static property from the FictionalDetective class: " + FictionalDetective.SomeRandomYear);

    //now showing the constant string 
    Console.WriteLine(FictionalDetective.SomeConstantString);

    //SherlockHolmes.ReadThisPlease = "This Cannot be Changed as it is readonly";  //this won't work
    Console.WriteLine(SherlockHolmes.ReadThisPlease);

    //use the method available to show the object data

    SherlockHolmes.DisplayTheFictionalDetective();

    //call the static method with the class direclty, just like class property
    FictionalDetective.MouseColor();
}



#endregion

#region Bakery Store

void BakeryStoreClassRelatedOutputs()
{
    BakeryStore bakeryStoreOne = new BakeryStore();
    BakeryStore bakeryStoreTwo = new BakeryStore();

    //passing objects as parameters. passed by reference
    Console.WriteLine("Total Number of Employees :" + BakeryStore.TotalNumberOfEmployeesInTwoStores(bakeryStoreOne, bakeryStoreTwo));

    //taking advantage of default values. we don't need to supply the store employees

    BakeryStore bakeryStoreThree = new BakeryStore();
    bakeryStoreThree.UpdateStoreDetails("Mysore");

    //but we can supply store employees count if we want to.

    BakeryStore bakeryStoreFour = new BakeryStore();
    bakeryStoreThree.UpdateStoreDetails("Mysore", 10);

    //we can also use the filed names to get flexibility in parameter order
    BakeryStore bakeryStoreFive = new BakeryStore();
    bakeryStoreFive.UpdateStoreDetails(NumberOfStoreEmployees: 69, StoreCity: "Gundlupet");

    //calling overloaded functions

    bakeryStoreFive.DisplayTheStore();
    bakeryStoreFive.DisplayTheStore(0);
    bakeryStoreFive.DisplayTheStore(1);

    //let's look at some parameter modifiers

    //by default, everything is passed by value. 
    //value types have the actual value, so they get passed by value
    //reference types have the reference as the value, so, they pass the reference, which is the value
    //yes, this can get confusing. 

    int NumberOfHeads = 10;

    Console.WriteLine("Value of Number Of Heads : " + NumberOfHeads);

    PassingNumberOfHeadsByReference(ref NumberOfHeads);

    Console.WriteLine("Value of Number Of Heads : " + NumberOfHeads); //value has been changed due to ref passing

    PassingNumberOfHeadsByValue(NumberOfHeads);

    Console.WriteLine("Value of Number Of Heads : " + NumberOfHeads); //value has not changed, as value was passed

    int NumberOfHeadsNeverInitialized;

    NumberOfHeadsWithOutOption(out NumberOfHeadsNeverInitialized);

    Console.WriteLine("Value of Number Of Heads : " + NumberOfHeadsNeverInitialized); //value has been initialized at the target function

    int NumberOfHeadsNeverInitialized2;

    //NumberOfHeadsWithOutOption(NumberOfHeadsNeverInitialized2); //this will be a error because it is being used without being initialized

    int NumberOfHeadsTented = 80;
    NumberOfHeadsReadOnlyRef(ref NumberOfHeadsTented); //raedonly ref

    int NumberOfHeadsTented2 = 120;
    NumberOfHeadsUsingInRef(in NumberOfHeadsTented2); //another way of doing readonly

    BakeryStore bakeryStoreSix = new BakeryStore();
    ref string tempref = ref bakeryStoreSix.GetStoreAddressReference();  //collect the reference to a property inside the object

    tempref = "New Address: 10, 5th Street, Mysore";

    Console.WriteLine(bakeryStoreSix.StoreAddress);  //we had the reference. So, the value inside the object has been updated

    BakeryStore bakeryStoreSeven = new BakeryStore();
    bakeryStoreSeven.CollectAllTheParameters("Cake", "Pastry", "Egg Puffs", "Mushroom Puffss");  //using the params keyword. strings only
}

void NumberOfHeadsUsingInRef(in int numberOfHeadsTented2)
{
    //numberOfHeadsTented2 = 150; //again, this will give an error . readonly
    //TODO what is the deal with this in and ref readonly as they appear similar.??
}

void NumberOfHeadsReadOnlyRef(ref readonly int numberOfHeadsTented)
{
    //numberOfHeadsTented = 890;  //this will be a error because at the time of calling, it's read only
}

void NumberOfHeadsWithOutOption(out int NumberOfHeadsNeverInitialized)
{
    NumberOfHeadsNeverInitialized = 50;
}

void PassingNumberOfHeadsByValue(int numberOfHeads)
{
    numberOfHeads = 100;
}

void PassingNumberOfHeadsByReference(ref int numberOfHeads)
{
    numberOfHeads = 20;
}


#endregion

#region Recursion Examples

void RecursionRelatedOutputs()
{
    Console.WriteLine("Multiply 10 with 20 : " + RecursionMultiply.Multiply(10, 20));
    Console.WriteLine("Fibonacci of 10 is : "+RecursionFibonacci.GetNthFibonacci(10));
    //add more recursion uses. it will be nice. TODO
}

#endregion

#region Type Conversion 

void TypeConversionOutputs()
{
    //some basic Implicit Casting. happens without any effort from developer

    int SomeNumber = 10;

    Console.WriteLine("The number is : " + SomeNumber);  //int automatically gets casted into a string

    double SomeDoubleNumber = 20.89;

    Console.WriteLine("The double number is : " + SomeDoubleNumber);  //double automatically gets casted into a string

    int SomeIntegerNumberFromDouble = (int)SomeDoubleNumber; //converting a double value to an int value with an explicit cast. 

    Console.WriteLine("The double number converted to integer is : " + SomeIntegerNumberFromDouble);  //double automatically gets casted into a string

    //using Parse and TryParse //extract numbers from strings
    string SomeNumberStoredAsString = "69";
    int SomeParsedNumber = int.Parse(SomeNumberStoredAsString);
    Console.WriteLine("The extracted number is : " + SomeParsedNumber);

    string SomeNumberStoredAsStringPart2 = "a";  //this will fail
    int SomeParsedNumberOutput = 0;
    bool ParseStatus = int.TryParse(SomeNumberStoredAsStringPart2, out SomeParsedNumberOutput);
    if(ParseStatus == true)
    {
        Console.WriteLine("Parsing worked with the string. The Number is : " + SomeParsedNumberOutput);
    }
    else
    {
        Console.WriteLine("Parsing Failed and there was no number to extract");
    }
    int SomeParsedNumberOutput2 = 0;
    bool ParseStatus2 = int.TryParse(SomeNumberStoredAsString, out SomeParsedNumberOutput2); //this will pass

    if (ParseStatus2 == true)
    {
        Console.WriteLine("Parsing worked with the string. The Number is : " + SomeParsedNumberOutput2);
    }
    else
    {
        Console.WriteLine("Parsing Failed and there was no number to extract");
    }

    //one more way to convert stuff
    double AnotherNumberForconverting = 6969.6969;
    string AnotherNumberForconvertingAfterConversion = Convert.ToString(AnotherNumberForconverting);

    Console.WriteLine(AnotherNumberForconvertingAfterConversion);
}

#endregion

# region Constructors

void ConstructorsOutputs()
{
    var HeroNumberOne = new Hero("Arthur Curry", "Talks to Fish", "Weild The Trident");

    var HeroNumberOneString = "Name : " + HeroNumberOne.Name + " Powers : " + HeroNumberOne.PrimaryPower + ", " + HeroNumberOne.SecondaryPower;
    Console.WriteLine(HeroNumberOneString);

    Console.WriteLine("Value from the static field set by constructor : " + Hero.BrandOfSuperHeroes);

    var HeroNumberTwo = new Hero("Coding Tutor");

    var HeroNumberTwoString = "Name : " + HeroNumberTwo.Name + " Powers : " + HeroNumberTwo.PrimaryPower + ", " + HeroNumberTwo.SecondaryPower;
    Console.WriteLine(HeroNumberTwoString);
}



#endregion


#region Inheritance

void InheritanceOutputs()
{
    var ParentOne = new Parent("Jay","Guy Who Talks","69");
    ParentOne.DisplayFamilyMemberDetails();

    var ChildOne = new Child(); ////this would be an example of Hierarchical inheritance

    var DistantRelativeOne = new DistantRelative(); // //example of Hybrid Inheritance

    //note: multiple inheritance is not availbe for classes in c sharp
    //multiple inheritance is available with interfaces, which we will discuss later.

    var FamilyNewBusinessOne = new FamilyNewBusiness();
    FamilyNewBusinessOne.ShowFamilyDetails(); //uses the base keyword to get info from the base class

    var ChildTwo = new Child("Small Baby", "Child Prodigy", "3", "School of Computers"); //uses base keyword to access base class constructor
    ChildTwo.DisplayFamilyMemberDetails(); //uses base keyword to call base class method.

    //method hiding with the new keyword
    var ParentTwo = new Parent();
    var ChildThree = new Child();

    ParentTwo.HobbiesDisplay(); //shows hobby details of parents
    ChildThree.HobbiesDisplay(); //shows hobby details of child, where, parent base method has been hidden with new keyword

    //method over riding with virtual and override keywords
    ParentTwo.VacationPlanDisplay();
    ChildThree.VacationPlanDisplay();

    //example of a sealed class
    var OldParentOne = new OldParent();
    OldParentOne.DisplayOldParentDetails();

    //example of sealed method.
    ParentTwo.FamilyMoto();
    ChildThree.FamilyMoto();
}

#endregion

# region Abstract Classes and Interfaces

void AbstractClassesInterfaces()
{
    //var AbstractGrandPaOne = new AbstractGrandPa();  //this won't work because the abstrac Grandpa is abstract

    var RealGrandPaOne = new RealGrandPa(); //real grandpa based on the abstract grandpa
    RealGrandPaOne.DisplayAbstractGrandPaDetails();
    RealGrandPaOne.AbstractMethodFromAbstractGrandPa();

    var ParentFour = new Parent();
    ParentFour.TellChildrenToStudy();

    //Dynamic Polymorphism involving Abstract Classes and Inheritance. TODO. need to dig a little deeper here.

    var ParentFive = new ParentPartTwo();
    ParentFive.TellChildrenToStudy();
    ParentFive.TellKidsToHaveFun();

    //explicit implementation of interfaces
    var ParentSix = new ParentPartThree();

    //now call method that will in turn call the explicitly implemented interfaces
    ParentSix.ParentDisplayStuff();
}

#endregion

