// See https://aka.ms/new-console-template for more information


using CupOfMug;
using KitchenAppliance;
using ShopRetail;
using static ShopRetail.WareHouse;



Console.WriteLine("Hello, World!");

//PartialStaticStuff();
//SystemObjectThings();
//GenericStuff();
//NullStuff();
//DestructorGarbageStuff();
DelegateEventStuff();
# region Partial and Static 

void PartialStaticStuff()
{
    //this classs has been defined in multiple files using the partial option
    var MugOne = new Mug();

    MugOne.DisplayMugDetails();

    //using the partial method
    MugOne.MugDisplayPartTwo();

    //using the static class with static methods and fields

    RedMugOfLove.DisplayRedMugDetails();

    //use the enums
    MugOne.DisplayAllEnums();

    //using the struct
    var MugStructOne = new MugOfStruct();

    MugStructOne.SetMugDetails();
    MugStructOne.DisplayMugDetails();
}

#endregion

#region System Object 

void SystemObjectThings()
{
    var MugForComparingOne = new Mug();
    var MugForComparingTwo = new Mug();
    var MugForComparingThree = new Mug();
    MugForComparingThree.Weight = 6969;

    Console.WriteLine("The two mugs are true? : " + MugForComparingOne.Equals(MugForComparingTwo));

    Console.WriteLine("The two mugs are true? : " + MugForComparingOne.Equals(MugForComparingThree));

    Console.WriteLine(MugForComparingOne.ToString());
    Console.WriteLine(MugForComparingTwo.ToString());
    Console.WriteLine(MugForComparingThree.ToString());

    //boxing and unboxing

    //creating a object to store the mug aka boxing
    object ObjectOne = new Mug();
    //same boxing an integer
    object ObjectTwo = 20;

    //unboxing the object into the target type
    var MugOneFromObject = (Mug)ObjectOne;
    var IntegerOneFromObject = (int)ObjectTwo;

    MugOneFromObject.DisplayMugDetails();
    Console.WriteLine("Value of the integer after unboxing : " + IntegerOneFromObject);

}

#endregion

#region Generics

void GenericStuff()
{

    var MugOfGenericOne = new MugGeneric<string>();

    MugOfGenericOne.NameOFMug = "This Mug is Generic";

    var MugOfGenericTwo = new MugGeneric<int>();
    MugOfGenericTwo.NameOFMug = 69;

    Console.WriteLine("String Generic Mug " + MugOfGenericOne.NameOFMug);
    Console.WriteLine("int Generic Mug " + MugOfGenericTwo.NameOFMug);

    //generic type with multiple types
    var MugOfGenericThree = new MugGeneric2<string,string,string>();

    MugOfGenericThree.FirstDetail = "This Mug is Amazing.";
    MugOfGenericThree.SecondDetail = "69";
    MugOfGenericThree.ThirdDetail = "This is made of ceramic";

    MugOfGenericThree.DisplayTheDetails();

    //TODO. need to dig into these Generic Constants at some point in the future

    var GenericApplianceOne = new GenericAppliance();
    var WaterBottleOne = new WaterBottle();
    var ElectricCookerOne = new ElectricCooker();

    WaterBottleOne.VolumeHolds = 500;
    WaterBottleOne.MaterialType = "Plastic";

    ElectricCookerOne.TimeToCook = 69;
    ElectricCookerOne.PowerSource = "Wood";

    GenericApplianceOne.DisplayApplianceDetails(WaterBottleOne);
    GenericApplianceOne.DisplayApplianceDetails(ElectricCookerOne);
}

#endregion

#region Null Stuff

void NullStuff()
{
    var RefrigeratorOne = new Refrigerator();

    RefrigeratorOne = null;  //set it to null so we can do null operations.

    //the following will raise a null exception. purposely not handling it.

    //if(RefrigeratorOne.ModelOfRefrigerator.HasValue == false)
    //{
    //    Console.WriteLine("the object is currently null");
    //}
    //else
    //{
    //    Console.WriteLine("the object is not NULL at all");
    //}

    var RefrigeratorTwo = new Refrigerator();
    RefrigeratorTwo.BrandOfRefrigerator = "Cooling Company";
    RefrigeratorTwo.ModelOfRefrigerator = 69;

    if (RefrigeratorTwo.ModelOfRefrigerator.HasValue == false)
    {
        Console.WriteLine("the object is currently null");
    }
    else
    {
        Console.WriteLine("the object is not NULL at all");
        Console.WriteLine(RefrigeratorTwo.BrandOfRefrigerator + RefrigeratorTwo.ModelOfRefrigerator);
    }

    //using the null coalescing operator

    var RefrigeratorThree = new Refrigerator();
    RefrigeratorThree.BrandOfRefrigerator = null;
    RefrigeratorThree.ModelOfRefrigerator = null;

    var BrandName = RefrigeratorThree.BrandOfRefrigerator ?? "No Brand Assigned";
    var ModelName = RefrigeratorThree.ModelOfRefrigerator ?? 0;

    Console.WriteLine("Brand Name : " + BrandName + ". Model Name : " + ModelName);

    //using the null propagation operator

    var RefrigeratorFour = new Refrigerator();
    RefrigeratorFour.BrandOfRefrigerator = null;
    RefrigeratorFour.ModelOfRefrigerator = null;

    Console.WriteLine("Brand Name : " + RefrigeratorFour?.BrandOfRefrigerator + ". Model Name : " + RefrigeratorFour?.ModelOfRefrigerator);

}

#endregion

#region Destructor and Garbage Collection

void DestructorGarbageStuff()
{

    //TODO: when we exit this function, the destructor is not getting called. it should right?
    MugWithDestructor mugWithDestructor = new MugWithDestructor();

    using(MugWithDisposable  mugWithDisposable1 = new MugWithDisposable())
    {
        mugWithDisposable1.JustSomethingToDoWithDatabase();

        //just before we exit the dispose gets called automatically
    }
}

#endregion

#region Delegates and Events

void DelegateEventStuff()
{

    var WareHouseOne = new WareHouse();

    TotalStockDelegate totalStockDelegateOne;

    totalStockDelegateOne = new TotalStockDelegate(WareHouseOne.TotalStock);

    var stockresponsefromdelegate = totalStockDelegateOne.Invoke(30, 39);

    Console.WriteLine("Total Stock, obtained via, delegate : " + stockresponsefromdelegate);

    //delegate linked to multiple methods
    TotalStockDelegateSecondOne totalStockDelegateTwo;

    totalStockDelegateTwo = WareHouseOne.TotalCustomers;
    totalStockDelegateTwo += WareHouseOne.TotalCustomersLaughing; //adding a second method using +=

    totalStockDelegateTwo.Invoke(25, 45);

    //note events are usually useful when you are using something like WPF and UI related coding
    //otherwise, you don't use them that much

    //TODO. I have skipped auto implemented events for now. we are not doing WPF right now. 

    //Subscriber object that has the method that will handle the event related activity
    HandleCustomerComplaint handleCustomerComplaint = new HandleCustomerComplaint();

    //Publisher where events are raised.
    ComplaintTrigger trigger = new ComplaintTrigger();

    //linking the event with the delegate.
    trigger.OnCustomerComplaint += handleCustomerComplaint.TakeCareOfComplaintOne;

    trigger.CustomerHaSRaisedComplaint(69);

    //another option to handle events is to use anonymous methods.

    //here is another publisher with event being raised

    //TODO, it is not working right now. from here 

    //ComplaintTriggerPartTwo complaintTriggerPartTwo = new ComplaintTriggerPartTwo();

    //complaintTriggerPartTwo.OnCustomerComplaintUsingAnonymous += delegate(int a) => {
    //    Console.WriteLine(" Complaint " + a + "has been handled");
    //};

    //complaintTriggerPartTwo.CustomerHaSRaisedComplaintWithAnonymous(69);

    //till here. anonymous methods not working.

    //another example of anonymous methods 

    AnonymousEmployee anonymousEmployee = new AnonymousEmployee();
    anonymousEmployee.MethodOne(69); 
    anonymousEmployee.MethodTwo(5);
    anonymousEmployee.MethodThree(6);
    var responseFour = anonymousEmployee.MethodFour(10,20);
    Console.WriteLine("The response from Method Four is : " + responseFour);


    anonymousEmployee.MultiplyWithTenFunc(20);

    anonymousEmployee.DisplayTheNumberAction(69);




}



#endregion
