// See https://aka.ms/new-console-template for more information
using ListGeneratorHelper;
using System.Collections;

Console.WriteLine("Hello, World!");

//BasicListStuff();
//ListOperations();
//ListOperationsPart2();
//ListOperationsPart3();
//MoreListThings();
//MoreListThings2();
//MoreListThings3();
//MoreListThings4();

//TODO. Need more knowledge to implment Covariance and Contravariance


void BasicListStuff()
{
    List<string> listOfSuperHeroes = new List<string>();
    List<int> listOfNumbers = new List<int>();

    listOfSuperHeroes = new List<string>{ "Batman","Superman","Wonder Woman"};
    listOfNumbers = new List<int> { 10, 20, 69 };

    foreach(var x in listOfSuperHeroes)
    {
        Console.WriteLine(x);
    }

    foreach(var x in listOfNumbers)
    { 
        Console.WriteLine(x); 
    }

    //usign the generator helper class library

    var ListGenerator = new GenerateListUsingRandom();
    var superheroList = ListGenerator.GenerateSuperHeroes(5);
    var numberList = ListGenerator.GenerateRandomNumbers(5);

    Console.WriteLine("Showing list items from the generator helper");

    foreach (var x in superheroList)
    {
        Console.WriteLine(x);
    }

    foreach (var x in numberList)
    {
        Console.WriteLine(x);
    }

}

void ListOperations()
{
    var ListOfNames = new List<string>();

    //Add and AddRange
    ListOfNames.Add("John Connor"); //add one item
    ListOfNames.AddRange(new List<string>() { "Sarah Connor", "Skynet Cyberdyne" });  //add an entire list

    Console.WriteLine("After Add and Add Range");

    foreach (var x in ListOfNames)
    {
        Console.WriteLine(x);
    }

    //Insert and InsertRange
    ListOfNames.Insert(1, "T-800");  //insert one item at location 1
    ListOfNames.InsertRange(2, new List<string>() { "T-600", "TX The Hotty" });  //insert an entire list at location 2

    Console.WriteLine("After Insert and Insert Range");
    foreach(var x in ListOfNames)
    { Console.WriteLine(x); }

    //Remove, removeAt, removerange
    var firstremovedName = ListOfNames.Remove("T-600");
    Console.WriteLine("Removed item is : " + firstremovedName);

    ListOfNames.RemoveAt(2);

    ListOfNames.RemoveRange(0, 2); //remove 2 items at location 0

    Console.WriteLine("After Remove, RemoveAt, Remove Range operation");

    foreach(var x in ListOfNames)
    {
        Console.WriteLine(x);
    }

    ListOfNames.Clear();

    Console.WriteLine("After Clear operation");

    if(ListOfNames.Count == 0)
    {
        Console.WriteLine("Nothing to show because, the list is empty");
    }    

    foreach (var x in ListOfNames)
    {
        Console.WriteLine(x); //nothing to show.
    }

    //TODO, compare and contrast, RemoveAll and Clear


}

void ListOperationsPart2()
{
    var ListGenerator = new GenerateListUsingRandom();
    var superheroList = ListGenerator.GenerateSuperHeroes(5);
    var numberList = ListGenerator.GenerateRandomNumbers(5);

    List<string> listOfSuperHeroes = new List<string>();
    List<int> listOfNumbers = new List<int>();

    listOfSuperHeroes = new List<string> { "Batman", "Superman", "Wonder Woman" };
    listOfNumbers = new List<int> { 10, 20, 69 };

    //IndexOf

    var IndexOfBatman = listOfSuperHeroes.IndexOf("Batman");
    Console.WriteLine("Location Index of Batman : " + IndexOfBatman);

    //BinarySearch
    var SearchforBatman = listOfSuperHeroes.BinarySearch("Batman");
    Console.WriteLine("Location Index of Batman with Binary Search: " + SearchforBatman);

    var SupermanInList = listOfSuperHeroes.Contains("Superman");
    Console.WriteLine("Status of Superman in the list : " + SupermanInList);

    listOfSuperHeroes.Sort();

    Console.WriteLine("list after being sorted");

    foreach(var x in listOfSuperHeroes)
    {
        Console.WriteLine(x);
    }

    listOfSuperHeroes.Reverse();
    
    Console.WriteLine("list after being sorted - reverse");

    foreach (var x in listOfSuperHeroes)
    {
        Console.WriteLine(x);
    }

    var SuperHeroArray = listOfSuperHeroes.ToArray();

    Console.WriteLine("Showing the array, extracted out of the list");

    foreach (var x in SuperHeroArray)
    {
        Console.WriteLine(x);
    }

}

void ListOperationsPart3()
{
    //some Query related stuff
    var ListGenerator = new GenerateListUsingRandom();
    var superheroList = ListGenerator.GenerateSuperHeroes(5);
    var numberList = ListGenerator.GenerateRandomNumbers(5);

    List<string> listOfSuperHeroes = new List<string>();
    List<int> listOfNumbers = new List<int>();

    listOfSuperHeroes = new List<string> { "Batman", "Superman", "Wonder Woman" };
    listOfNumbers = new List<int> { 10, 20, 69 };

    var BatmanExists = listOfSuperHeroes.Exists(hero => hero == "Batman");

    if(BatmanExists == true)
    {
        Console.WriteLine("Batman is in the list");
    }

    var BatmanExists2 = listOfSuperHeroes.Find(hero => hero == "Batman");

    Console.WriteLine("We found the hero : " + BatmanExists2);

    var BatmanIndex = listOfSuperHeroes.FindIndex(hero => hero == "Batman");
    Console.WriteLine("The index location of Batman is : " + BatmanIndex);

    var TheLastHero = listOfSuperHeroes.FindLast(hero => hero == "Wonder Woman");
    Console.WriteLine("The last hero is : " +  TheLastHero);

    var TheLastHeroIndex = listOfSuperHeroes.FindLastIndex(hero => hero == "Wonder Woman");
    Console.WriteLine("The index of th elast hero is " + TheLastHeroIndex);

    //add a bunch of Batman so we can get multiple responses in the list
    listOfSuperHeroes.Add("Batman");
    listOfSuperHeroes.Add("Batman");
    listOfSuperHeroes.Add("Batman");
    var FindAllBatman = listOfSuperHeroes.FindAll(hero => hero == "Batman");

    foreach(var hero in FindAllBatman)
    {
        Console.WriteLine(hero);
    }

    //TODO ConvertAll. A little unclear how and what this does.
}

void MoreListThings()
{
    Dictionary<int,string> ListOfStarWarMovies = new Dictionary<int,string>();

    ListOfStarWarMovies.Add(1, "The Phantom Menace");
    ListOfStarWarMovies.Add(2, "Attack of the Clones");
    ListOfStarWarMovies.Add(3, "Revenge of the Sith");
    ListOfStarWarMovies.Add(4, "A New Hope");
    ListOfStarWarMovies.Add(5, "Empire Strikes Back");
    ListOfStarWarMovies.Add(6, "Return of the Jedi");

    //show the dictionary list.

    foreach(var x in ListOfStarWarMovies)
    {
        Console.WriteLine(" Movie Number : " +  x.Key + " Movie Title : " +  x.Value);
    }

    //remove a movie using they key

    ListOfStarWarMovies.Remove(1);

    Console.WriteLine("After removing the first movie");

    foreach (var x in ListOfStarWarMovies)
    {
        Console.WriteLine(" Movie Number : " + x.Key + " Movie Title : " + x.Value);
    }

    //get all the keys
    var AllMovieKeys = ListOfStarWarMovies.Keys;

    Console.WriteLine("Showing all the keys");

    foreach (var key in AllMovieKeys)
    {
        Console.WriteLine(key);
    }

    var CheckForRemovedKey = ListOfStarWarMovies.ContainsKey(1);
    var CheckForremovedValue = ListOfStarWarMovies.ContainsValue("The Phantom Menace");

    Console.WriteLine("Status of Removed Key : " + CheckForRemovedKey);

    Console.WriteLine("Status of Removed Value : " + CheckForremovedValue);

    //clear the entire collection
    //ListOfStarWarMovies.Clear();

    var SortedListOfMovies = new SortedList<int,string>();

    SortedListOfMovies.Add(1, "Philosophers Stone");
    SortedListOfMovies.Add(2, "Chamber of Secrets");
    SortedListOfMovies.Add(3, "Prisoner of Azkaban");

    foreach(var x in  SortedListOfMovies)
    {
        Console.WriteLine(x);
    }

    //TODO. what exactly does this SortedList do which the regular List and Dictionary cannot



}

void MoreListThings2()
{
    //HashTable. looks like the standard list again. 

    var HashListOfMovies = new Hashtable();
    HashListOfMovies.Add(1, new MovieItem()
    {
        MovieNumber = 1,
        Title = "Philosopher Stone"
    });

    HashListOfMovies.Add(2, new MovieItem()
    {
        MovieNumber = 2,
        Title = "Chamber of Secrets"
    });

    foreach (DictionaryEntry x in HashListOfMovies)
    {
        var TargetObject = (MovieItem)x.Value;
        TargetObject.DisplayMovieItem();
    }

    //HashSet 
    //Useful for making set operations

    var HashSetOfMoviesOne = new HashSet<MovieItem>();

    var MovieItemOne = new MovieItem()
    {
        MovieNumber = 1,
        Title = "Philosopher Stone"

    };
    var MovieItemTwo = new MovieItem()
    {
        MovieNumber = 2,
        Title = "Chamber of Secrets"

    };

    HashSetOfMoviesOne.Add(MovieItemOne);
    HashSetOfMoviesOne.Add(MovieItemTwo);

    var HashSetOfMoviesTwo = new HashSet<MovieItem>();

    var MovieItemThree = new MovieItem()
    {
        MovieNumber = 3,
        Title = "Prisoner of Azkaban"

    };
    var MovieItemFour = new MovieItem()
    {
        MovieNumber = 4,
        Title = "Goblet of Fire"

    };

    HashSetOfMoviesTwo.Add(MovieItemThree);
    HashSetOfMoviesTwo.Add(MovieItemFour);

    Console.WriteLine("Movies in First Set");

    foreach(MovieItem x in HashSetOfMoviesOne)
    {
        x.DisplayMovieItem();
    }

    Console.WriteLine("Movies in Second Set");

    foreach (MovieItem x in HashSetOfMoviesTwo)
    {
        x.DisplayMovieItem();
    }

    //lets union the two sets
    var UnionHashSetofMovies = new HashSet<MovieItem>();
    UnionHashSetofMovies.UnionWith(HashSetOfMoviesOne);
    UnionHashSetofMovies.UnionWith(HashSetOfMoviesTwo);

    Console.WriteLine("Movies in Union Set");

    foreach (MovieItem x in UnionHashSetofMovies)
    {
        x.DisplayMovieItem();
    }

    //TODO. Let's look at IntersectWith. Dig Deeper

    //TODO. Also, let's find out exact usage scenarios of these Hash Things
}

void MoreListThings3()
{
    var ArrayListOfMovies = new ArrayList();

    var MovieItemOne = new MovieItem()
    {
        MovieNumber = 1,
        Title = "Philosopher Stone"

    };
    var MovieItemTwo = new MovieItem()
    {
        MovieNumber = 2,
        Title = "Chamber of Secrets"

    };

    ArrayListOfMovies.Add(MovieItemOne);
    ArrayListOfMovies.Add(MovieItemTwo);

    foreach(var x in ArrayListOfMovies)
    {
        var MovieItem = (MovieItem)x;
        MovieItem.DisplayMovieItem();
    }

    //again, TODO, why use an ArrayList??? a lot of these things seem to be duplicating each others capabilities

    var StackOfMovies = new Stack<MovieItem>();
    StackOfMovies.Push(MovieItemOne);
    StackOfMovies.Push(MovieItemTwo);

    foreach(var x in  StackOfMovies)
    {
        x.DisplayMovieItem();
    }

    var popamovie = StackOfMovies.Pop();

    Console.WriteLine("Popped movie on display");
    popamovie.DisplayMovieItem();

    //TODO do other stack operations 

    var QueueOfMovies = new Queue<MovieItem>();

    QueueOfMovies.Enqueue(MovieItemOne);
    QueueOfMovies.Enqueue(MovieItemTwo);

    foreach(var x in  QueueOfMovies)
    {
        x.DisplayMovieItem();
    }

    //TODO. do more queue operations.
}

void MoreListThings4()
{


    var MovieItemOne = new MovieItem()
    {
        MovieNumber = 1,
        Title = "Philosopher Stone"

    };
    var MovieItemTwo = new MovieItem()
    {
        MovieNumber = 2,
        Title = "Chamber of Secrets"

    };

    IEnumerable<MovieItem> EnumerableListOfMovies = new List<MovieItem>() { MovieItemOne, MovieItemTwo };

    //now, use the enumerator 
    IEnumerator<MovieItem> enumerator = EnumerableListOfMovies.GetEnumerator();
    enumerator.Reset();
    while(enumerator.MoveNext())
    {
        enumerator.Current.DisplayMovieItem();
    }

    //TODO. The real question is, when and where will we use this as we already have fantastic List Operations

    //using the custom implementation of IEnumerable with MovieItem
    var tempListOfMoviesWithCustomImplementation = new CustomMovieList();

    tempListOfMoviesWithCustomImplementation.AddTheMovie(MovieItemOne);
    tempListOfMoviesWithCustomImplementation.AddTheMovie(MovieItemTwo);

    //this movie won't get addded because the title is short
    var MovieItemThree = new MovieItem()
    {
        MovieNumber = 3,
        Title = "C3PO"
    };

    tempListOfMoviesWithCustomImplementation.AddTheMovie(MovieItemThree);
   
    foreach(MovieItem x in tempListOfMoviesWithCustomImplementation)
    {
        x.DisplayMovieItem(); //here only two movies will be shown. the 3rd movie never got added.
    }

    //comparing too objects

    var MovieItemFive = new MovieItem2()
    {
        MovieNumber = 3,
        Title = "C3PO"
    };

    var MovieItemFiveCopy = new MovieItem2()
    {
        MovieNumber = 3,
        Title = "C3PO"
    };

    var MovieItemFiveCopy2 = new MovieItem2()
    {
        MovieNumber = 3,
        Title = "C3PZ"
    };

    if (MovieItemFive.Equals(MovieItemFiveCopy))
    {
        Console.WriteLine("The two movies are equal");
    }

    if(MovieItemFive.Equals(MovieItemFiveCopy2) == false)
    {
        Console.WriteLine("The two movies are NOT equal");
    }

    var MovieItemSix = new MovieItem2()
    {
        MovieNumber = 4,
        Title = "C3PO"
    };

    Console.WriteLine("Comparing Movie Five with Movie Six : " + MovieItemFive.CompareTo(MovieItemSix));
}

//IEnumerable implementation
//TODO, need to dig deeper into this. more examples. more scenarios.
public class CustomMovieList : IEnumerable<MovieItem>
{
    //private collection of movies
    private List<MovieItem> movieItems = new List<MovieItem>();

    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    //implement our enumerator
    public IEnumerator<MovieItem> GetEnumerator()
    {
        for(int i=0;i<movieItems.Count;i++)
        {
            yield return movieItems[i];
        }
    }

    public void AddTheMovie(MovieItem newmovie)
    {
        if(newmovie.Title.Length<5)
        {
            //if the movie title length is less than 5, than, don't add the movie
            Console.WriteLine("The title is too short");
        }
        else
        {
            movieItems.Add(newmovie);
        }
    }

    //TODO, add more list operations like clear, remove and so on.
}

public class MovieItem
{
    public int MovieNumber;
    public string Title;

    public void DisplayMovieItem()
    {
        Console.WriteLine(" Number : " +  MovieNumber + " Title : " + Title);
    }
}


//implements the equatable for comparing objects
public class MovieItem2 : IEquatable<MovieItem2>, IComparable<MovieItem2>
{
    public int MovieNumber;
    public string Title;

    public int CompareTo(MovieItem2? other)
    {
        if(this.MovieNumber > other.MovieNumber)
        {
            return 1;
        }
        if(this.MovieNumber == other.MovieNumber)
        {
            return 0;
        }
        if(this.MovieNumber < other.MovieNumber)
        {
            return -1;
        }
        return 0;
    }

    public void DisplayMovieItem()
    {
        Console.WriteLine(" Number : " + MovieNumber + " Title : " + Title);
    }

    public bool Equals(MovieItem2? other)
    {
        if(this.Title == other.Title && this.MovieNumber == other.MovieNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}