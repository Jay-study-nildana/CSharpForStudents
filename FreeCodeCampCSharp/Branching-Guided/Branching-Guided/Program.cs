// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//TODO - Unfinished. Do it properly.

// the ourAnimals array will store the following: 
string animalSpecies = "";
string animalID = "";
string animalAge = "";
string animalPhysicalDescription = "";
string animalPersonalityDescription = "";
string animalNickname = "";

// variables that support data entry
int maxPets = 8;
string? readResult;
string menuSelection = "";

// array used to store runtime data, there is no persisted data
string[,] ourAnimals = new string[maxPets, 6];

// display the top-level menu options

do
{
    // pause code execution
    readResult = Console.ReadLine();

} while (menuSelection != "exit");

switch (menuSelection)
{
    case "1":
        // List all of our current pet information
        Console.WriteLine("this app feature is coming soon - please check back to see progress.");
        Console.WriteLine("Press the Enter key to continue.");
        readResult = Console.ReadLine();
        break;
    case "2":
        // Add a new animal friend to the ourAnimals array
        Console.WriteLine("this app feature is coming soon - please check back to see progress.");
        Console.WriteLine("Press the Enter key to continue.");
        readResult = Console.ReadLine();
        break;

}

switch (i)
{
    case 0:
        // variable assignments were removed for this view of the structure
        animalSpecies = "dog";
        animalID = "d1";
        animalAge = "2";
        animalPhysicalDescription = "medium sized cream colored female golden retriever weighing about 65 pounds. housebroken.";
        animalPersonalityDescription = "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.";
        animalNickname = "lola";
        break;

    case 1:
        // variable assignments were removed for this view of the structure
        animalSpecies = "dog";
        animalID = "d2";
        animalAge = "9";
        animalPhysicalDescription = "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.";
        animalPersonalityDescription = "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.";
        animalNickname = "loki";
        break;

    case 2:
        // variable assignments were removed for this view of the structure
        animalSpecies = "cat";
        animalID = "c3";
        animalAge = "1";
        animalPhysicalDescription = "small white female weighing about 8 pounds. litter box trained.";
        animalPersonalityDescription = "friendly";
        animalNickname = "Puss";
        break;

    case 3:
        // variable assignments were removed for this view of the structure
        animalSpecies = "cat";
        animalID = "c4";
        animalAge = "?";
        animalPhysicalDescription = "";
        animalPersonalityDescription = "";
        animalNickname = "";
        break;

    default:
        // variable assignments were removed for this view of the structure
        animalSpecies = "";
        animalID = "";
        animalAge = "";
        animalPhysicalDescription = "";
        animalPersonalityDescription = "";
        animalNickname = "";
        break;
}
