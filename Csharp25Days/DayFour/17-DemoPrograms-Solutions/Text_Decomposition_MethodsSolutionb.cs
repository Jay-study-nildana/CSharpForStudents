using System.Text.RegularExpressions;

string somesentence = "I am the knight. I am the vengence. I am batman";

//remove any whitespaces

somesentence = somesentence.Trim();

// remove extra characters

somesentence = Regex.Replace(somesentence, @"[^\w\s]", "");

var tokensofsomesentence = somesentence.Split(" ");

var tokendictionary = new Dictionary<string, int>();

foreach(var token in tokensofsomesentence)
{
    if(tokendictionary.ContainsKey(token))
    {
        tokendictionary[token]++;  //increment the count by one to indicate that it is now increased. 
    }
    else
    {
        tokendictionary.Add(token, 1); //add the token brand new to the dictionary. count starts at one to be incremented later. 
    }
}

foreach(var item in tokendictionary)
{
    Console.WriteLine($" Token : {item.Key}  Count : {item.Value}");
}

Console.ReadLine();