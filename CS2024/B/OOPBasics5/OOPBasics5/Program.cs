// See https://aka.ms/new-console-template for more information
using CollectionsProvider;

Console.WriteLine("Hello, World!");

//BasicLINQStuff1();
BasicLINQStuff2();


void BasicLINQStuff1()
{
    CollectionsOne collectionsOne = new CollectionsOne();

    var SuperHeroFirstCollection = collectionsOne.SuperHeroSetOne();
    var SuperHeroSecondCollection = collectionsOne.SuperHeroSetTwo();

    Console.WriteLine("Displaying the first collection ");
    foreach(var x in SuperHeroFirstCollection)
    {
        x.DisplayHeroDetails();
    }

    Console.WriteLine("Displaying the second collection ");
    foreach (var x in SuperHeroSecondCollection)
    {
        x.DisplayHeroDetails();
    }

    //lets get all the DC heroes and then get all the marvel heroes

    var DCHeroes = SuperHeroSecondCollection.Where(x => x.Brand == "DC");
    var MarvelHeroes = SuperHeroSecondCollection.Where(x => x.Brand == "Marvel");

    Console.WriteLine("Here are all the DC Heroes");
    foreach(var x in  DCHeroes)
    {
        x.DisplayHeroDetails();
    }

    Console.WriteLine("Here are all the Marvel Heroes");
    foreach (var x in MarvelHeroes)
    {
        x.DisplayHeroDetails();
    }
}

void BasicLINQStuff2()
{
    CollectionsOne collectionsOne = new CollectionsOne();

    var SuperHeroFirstCollection = collectionsOne.SuperHeroSetOne();
    var SuperHeroSecondCollection = collectionsOne.SuperHeroSetTwo();

    var SuperHeroesOrderedByNameAscending = SuperHeroSecondCollection.OrderBy(x => x.Name).ToList();
    var SuperHeroesOrderedByDescending = SuperHeroSecondCollection.OrderBy(x => x.Name).ToList();

    Console.WriteLine("Heroes sorted by Ascending with Name");
    foreach (var x in SuperHeroesOrderedByNameAscending)
    {
        x.DisplayHeroDetails();
    }

    Console.WriteLine("Heroes sorted by Descending with Name");
    foreach (var x in SuperHeroesOrderedByDescending)
    {
        x.DisplayHeroDetails();
    }

    //grab the first hero from DC
    var FirstHeroFromDC = SuperHeroSecondCollection.First(x => x.Brand == "DC");
    FirstHeroFromDC.DisplayHeroDetails();

    //grab the first hero or the default 
    var FirstHeroFromDCOrDefault = SuperHeroSecondCollection.FirstOrDefault(x => x.Brand == "DC");
    FirstHeroFromDCOrDefault.DisplayHeroDetails();  //TODO. add a null check if neccessary. use a if null check or the '?' null check operator

    //grab the last hero. and the default
    var LastHeroFromDC = SuperHeroSecondCollection.Last(x => x.Brand == "DC");
    var LastHeroFromDCOrDefault = SuperHeroSecondCollection.LastOrDefault(x => x.Brand == "DC");

    LastHeroFromDC.DisplayHeroDetails();
    LastHeroFromDCOrDefault.DisplayHeroDetails();

    //get the 3rd hero. 
    //TODO add default example if you wish.
    var ThirdSuperHero = SuperHeroSecondCollection.ElementAt(3); //remember counting starts from zero. so, this will give us the 4th hero.
    ThirdSuperHero.DisplayHeroDetails();

    //get exactly one hero of type DC
    //TODO. add a code to check for a situation where there are more than one matching result. 
    var SingleHero = SuperHeroSecondCollection.Single(x => x.Name == "Batman");
    SingleHero.DisplayHeroDetails();

    //lets do some selections
    var SelectedHeroesOnlyNames = SuperHeroSecondCollection.Select(x => x.Name).ToList();
    var SelectedHeroesOnlyNamesOnlyDC = SuperHeroSecondCollection.Where(x => x.Brand == "DC").Select(x=>x.Name).ToList();
    var SelectedHeroesWithAnonymousTypes = SuperHeroSecondCollection.Where(x=>x.Brand == "Marvel").Select(x => new
    {
        x.Name,
        x.AlterEgo
    }).ToList();
    var justsomebreakpoint = "break point";//put a break point here to see all the results//TODO. add a display if you want

    var SumOfAllHeroNumbers = SuperHeroSecondCollection.Sum(x => x.NumberHero);
    var CountOfAllHeroes = SuperHeroSecondCollection.Count(x => x.Brand == "DC" || x.Brand == "Marvel");
    //TODO use the Average function on your own
    var HeroMin = SuperHeroSecondCollection.Min(x => x.NumberHero);
    var HeroMax = SuperHeroSecondCollection.Max(x => x.NumberHero);
    var justsomebreakpoint2 = "break point";//put a break point here to see all the results//TODO. add a display if you want
}
