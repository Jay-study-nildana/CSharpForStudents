// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

//Directory.EnumerateFiles("c:\Genre\Rock"
//put the sample folder into a location of your choice.
//for example debug

IEnumerable<string> files = Directory.EnumerateFiles("./");
IEnumerable<string> files2 = Directory.EnumerateFiles("./samplefolder");

foreach (var file in files)
{
    Console.WriteLine(file);
}

foreach (var file in files2)
{
    Console.WriteLine(file);
}

// Find all *.* files in the stores folder and its subfolders
IEnumerable<string> allFilesInAllFolders = Directory.EnumerateFiles("samplefolder", "*.*", SearchOption.AllDirectories);

foreach (var file in allFilesInAllFolders)
{
    Console.WriteLine(file);
}

// Find all *.md files in the stores folder and its subfolders
IEnumerable<string> allFilesmd = Directory.EnumerateFiles("samplefolder", "*.md", SearchOption.AllDirectories);

foreach (var file in allFilesmd)
{
    Console.WriteLine(file);
}

//now, let's make a list of all md files

List<string> htmlFiles = new List<string>();

// Find all *.html files in the stores folder and its subfolders
IEnumerable<string> allFilesHTML = Directory.EnumerateFiles("samplefolder", "*.html", SearchOption.AllDirectories);

foreach (var file in allFilesHTML)
{
    //Console.WriteLine(file);
    htmlFiles.Add(file);
    
}

Console.WriteLine("All the HTML files");
foreach (var file in htmlFiles)
{
    Console.WriteLine(file);
}




