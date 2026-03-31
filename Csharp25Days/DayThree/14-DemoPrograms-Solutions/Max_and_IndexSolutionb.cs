
int[] listofnumbers = { 1, 2, 3, 4, 5, 6, 7, 8 };

for (int i = 0; i < listofnumbers.Length; i++)
{
    Console.WriteLine($" Number : {listofnumbers[i]}  Location : {i}");
    
}
Console.WriteLine("-----");

var response = GetMax(listofnumbers);

Console.WriteLine($" Max Number : {response.maxvalue} Location : {response.location}");

Random random = new Random();
int[] listofnumbers2 = { 
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        random.Next(100),
                        };

for (int i = 0; i < listofnumbers2.Length; i++)
{
    Console.WriteLine($" Number : {listofnumbers2[i]}  Location : {i}");

}
Console.WriteLine("-----");

var response2 = GetMax(listofnumbers2);

Console.WriteLine($" Max Number : {response2.maxvalue} Location : {response2.location}");

static (int location,int maxvalue) GetMax(int[] listofnumbers)
{
    int location = 0;
    int maxvalue = 0;

    for(int i=0;i<listofnumbers.Length;i++)
    {
        if (listofnumbers[i] > maxvalue)
        {
            maxvalue = listofnumbers[i];
            location = i;
        }
    }

    return (location, maxvalue);
}