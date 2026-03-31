using System;
Random random = new Random();

List<int> listofnumbers2 = new List<int>{
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



for (int i = 0; i < listofnumbers2.Count; i++)
{
    Console.WriteLine($" Number : {listofnumbers2[i]}  Location : {i}");

}

Console.WriteLine("-----");

listofnumbers2.RemoveAll(x => x % 2 != 0);

for (int i = 0; i < listofnumbers2.Count; i++)
{
    Console.WriteLine($" Number : {listofnumbers2[i]}  Location : {i}");

}

Console.WriteLine("-----");