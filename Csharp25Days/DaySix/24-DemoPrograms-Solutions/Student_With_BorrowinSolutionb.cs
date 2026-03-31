void DisplayNames(IReadOnlyCollection<string> names)
{
    foreach(var x in names)
    {
        Console.WriteLine(x);
    }

    Console.WriteLine("-----------Finished displaying names------------");
}

var collectionone = new DemoCollection();
collectionone.AddName("Batman");
collectionone.AddName("What have I done");
DisplayNames(collectionone.ListOfNames);
collectionone.AddName("Padme");
collectionone.AddName("Jedi");
collectionone.RemoveName("What have I done");
DisplayNames(collectionone.ListOfNames);



class DemoCollection
{
    private List<string> _listofnames = new List<string>();

    public IReadOnlyList<string> ListOfNames => _listofnames.AsReadOnly();

    public void AddName(string name)
    {
        _listofnames.Add(name);
    }

    public void RemoveName(string name)
    {
        _listofnames.Remove(name);
    }
}