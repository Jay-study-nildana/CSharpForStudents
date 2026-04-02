using Jay.LearningHelperForStudents;

var randomgenerator = new RandomRepeatedWordsGenerator();

//get a collection from the GenerateRandomRepeatedWords Method.

var wordscollection = randomgenerator.GenerateRandomRepeatedWords(null, 4, 1, 4, null, true, true);

foreach (var word in wordscollection)
{
    Console.WriteLine(word);
}

//now, let's use this for a simple word count and top k problem. 

var wordCountDictionary = new Dictionary<string, int>();

foreach(var x in wordscollection)
{
    if (wordCountDictionary.ContainsKey(x) == false)
    {
        wordCountDictionary.Add(x, 1);
    }
    else
    {
        wordCountDictionary[x]++;
    }
}

//now, let's get the top 2 words that are repeated the most
var toptwowords = wordCountDictionary.
                    OrderByDescending(x => x.Value).
                    Take(2).
                    Select(x => new { Key = x.Key, Value = x.Value }).
                    ToList();

//now, let's get the top 2 words that are repeated the most
var toptwowordsDictionary = wordCountDictionary.
                    OrderByDescending(x => x.Value).
                    Take(2).
                    Select(x => new { Key = x.Key, Value = x.Value }).ToDictionary(x => x.Key, x => x.Value);                   

foreach (var x in toptwowords)
{
    Console.WriteLine($"Word: {x.Key}, Count: {x.Value}");
}
