using System;
using System.IO;
using Jay.LearningHelperForStudents;
using Jay.LearningHelperForStudents.Utilities;

var lh = new Lh();

// Prompt user for file path
Console.Write("Enter the file path: ");
var path = Console.ReadLine()?? "somefile.txt";

//display the full path the user entered

var fullpath = Path.GetFullPath(path);
Console.WriteLine("Full File Path : "+fullpath);

lh.AddSimpleConsoleDivider();


try
{
    var text = File.ReadAllText(path);
    Process(text);
}
catch (FileNotFoundException fnf)
{
    // handle specific known error
    Console.WriteLine($"File not found: {fnf.FileName}");
    lh.AddSimpleConsoleDivider();
}
catch (IOException io)
{
    // handle other IO issues
    Console.WriteLine($"IO error: {io.Message}");
}
finally
{
    // cleanup, always executed
    ReleaseTemporaryResources();
}

// Example process method
void Process(string content)
{
    Console.WriteLine("File content:");
    Console.WriteLine(content);
    lh.AddSimpleConsoleDivider();
}

// Example cleanup method
void ReleaseTemporaryResources()
{
    Console.WriteLine("Cleanup done.");
}