// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

# region 2540. Minimum Common Value


//https://leetcode.com/problems/minimum-common-value/description/

var nums1 = new int[] {1,2,3,6};
var nums2 = new int[] { 2, 3, 4, 5 }; 

//var response = GetCommon(nums1, nums2);
//var response2 = GetCommon2(nums1, nums2);

int GetCommon(int[] nums1, int[] nums2)
{

    var nums1List = nums1.ToList<int>();
    var nums2List = nums2.ToList<int>();

    var CommonItems = new List<int>();

    foreach (var x in nums1)
    {
        foreach (var y in nums2)
        {
            if (x == y) CommonItems.Add(y);
        }
    }

    if(CommonItems.Count > 0)
    {
        return CommonItems.Min();
    }

    return -1;
}

int GetCommon2(int[] nums1, int[] nums2)
{

    var nums1List = nums1.ToList<int>();
    var nums2List = nums2.ToList<int>();

    var CommonItems = nums1.Intersect(nums2).ToList<int>();

    if (CommonItems.Count > 0)
    {
        return CommonItems.Min();
    }

    return -1;
}


#endregion

# region 1. Two Sum

//https://leetcode.com/problems/two-sum/description/

var TwoSumnums = new int[] { 2, 7, 11, 15 };
var TwoSumtarget = 9;

var TwoSumnums2 = new int[] { 0, 4, 3, 0 };
var TwoSumtarget2 = 0;
//var responsetwosum1 = TwoSum(TwoSumnums2, TwoSumtarget2);

int[] TwoSum(int[] nums, int target)
{

    var numsList = nums.ToList<int>();
   

    var answer = new int[] { 0, 0 };

    for (int i=0;i< numsList.Count-1;i++)
    {
        for(int j=i+1;j< numsList.Count;j++)
        {
            if (numsList[i] + numsList[j] == target)
            {
                //found a match



                answer[0] = numsList.IndexOf(numsList[i]);
                answer[1] = numsList.IndexOf(numsList[j]);

                if (numsList[i] == numsList[j])
                {
                    var splitList = numsList.Slice(answer[0]+1, numsList.Count - (answer[0]+1));
                    answer[1] = splitList.IndexOf(numsList[j]) + answer[0]+1;
                }

                return answer;
            }
        }
    }
    return answer;
}


#endregion

#region 1480. Running Sum of 1d Array

//https://leetcode.com/problems/running-sum-of-1d-array/description/

var RunningSumNums = new int[] { 1, 2, 3, 4 };

//var response = RunningSum(RunningSumNums);

int[] RunningSum(int[] nums)
{

    var outputarray = new int[nums.Length];
    var tempList = nums.ToList<int>();

    for(int i = 0;i< nums.Length;i++)
    {
        var currentList = tempList.Slice(0, i+1);
        var total = CalculateTotal(currentList);
        outputarray[i] = total;
    }

    return outputarray;

}

//var sampleList = new List<int>() { 1, 2, 3, 4 };
//var total = CalculateTotal(sampleList);

int CalculateTotal(List<int> NumbersToTotal)
{
    if(NumbersToTotal.Count == 1) return NumbersToTotal[0];

    return NumbersToTotal.First() + CalculateTotal(NumbersToTotal.Slice(1,NumbersToTotal.Count - 1));
}

#endregion

#region 1672. Richest Customer Wealth

//https://leetcode.com/problems/richest-customer-wealth/description/

var MaximumWealthAccounts = new int[][] { [1, 2, 3], [3, 2, 1] };
//var responseMaximumWealth = MaximumWealth(MaximumWealthAccounts);
int MaximumWealth(int[][] accounts)
{
    var MaxWealth = 0;

    foreach(var x in  accounts)
    {
        var tempWealth = CalculateTotalWealth(x.ToList<int>());
        if(tempWealth > MaxWealth)
            MaxWealth = tempWealth;
    }

    return MaxWealth;
}

int CalculateTotalWealth(List<int> NumbersToTotal)
{
    if (NumbersToTotal.Count == 1) return NumbersToTotal[0];

    return NumbersToTotal.First() + CalculateTotalWealth(NumbersToTotal.Slice(1, NumbersToTotal.Count - 1));
}

#endregion

#region 412. Fizz Buzz

//https://leetcode.com/problems/fizz-buzz/description/

//FizzBuzz(3);
IList<string> FizzBuzz(int n)
{
    var responseString = new List<string>();

    var i = 1;
    while(i <= n)
    {
        var tempString = FizzBuzzGuy(i);
        responseString.Add(tempString);
        i++;
    }

    return responseString;
}

string FizzBuzzGuy(int n)
{
    if(n % 3 == 0 && n % 5 == 0)
    {
        return "FizzBuzz";
    }
    else if(n % 3 == 0)
    {
        return "Fizz";
    }
    else if( n % 5 == 0)
    {
        return "Buzz";
    }
    else
    {
        return n.ToString();
    }

}

#endregion

#region 1342. Number of Steps to Reduce a Number to Zero

//https://leetcode.com/problems/number-of-steps-to-reduce-a-number-to-zero/description/

// NumberOfSteps(14);
int NumberOfSteps(int num)
{
    int numberOfsteps = 0;

    while(num > 0)
    {
        numberOfsteps++;
        if (num % 2 == 0) num = num / 2;
        else if (num % 2 == 1) num = num - 1;
    }

    return numberOfsteps;
}

#endregion

#region 383. Ransom Note

// https://leetcode.com/problems/ransom-note/description/


var CanConstructranssomNote = "aa";
var CanConstructmagazine = "aab";

//var response = CanConstruct(CanConstructranssomNote, CanConstructmagazine);
bool CanConstruct(string ransomNote, string magazine)
{
    var ListOfRansomNoteCharacters = new List<char>();
    var ListOfRansomNoteCharacterCount = new List<int>();
    var ListOfRansomNoteSet = new List<CanConstructOne>();
    var ListOfMagazineSet = new List<CanConstructOne>();

    foreach (var x in ransomNote)
    {
        //check if character is already in the collection
        if(ListOfRansomNoteSet.Exists(y => y.Character == x) == false)
        {
            //not already in collection. add it.
            var tempCanConstructOne = new CanConstructOne();
            tempCanConstructOne.Character = x;
            tempCanConstructOne.CharacterCount = 1;
            ListOfRansomNoteSet.Add(tempCanConstructOne);
        }
        else
        {
            //get item from collection
            var tempCanConstructOne = ListOfRansomNoteSet.Select(y => y).Where(y => y.Character == x).First();
            //update count
            tempCanConstructOne.CharacterCount++;

        }
    }

    //do the same for the magazine list.
    foreach (var x in magazine)
    {
        //check if character is already in the collection
        if (ListOfMagazineSet.Exists(y => y.Character == x) == false)
        {
            //not already in collection. add it.
            var tempCanConstructOne = new CanConstructOne();
            tempCanConstructOne.Character = x;
            tempCanConstructOne.CharacterCount = 1;
            ListOfMagazineSet.Add(tempCanConstructOne);
        }
        else
        {
            //get item from collection
            var tempCanConstructOne = ListOfMagazineSet.Select(y => y).Where(y => y.Character == x).First();
            //update count
            tempCanConstructOne.CharacterCount++;

        }
    }

    foreach(var x in ListOfRansomNoteSet)
    {
        //check if the number of characters need for ransom note is available in the magazine
        //get the corresponding item from magainze
        if(ListOfMagazineSet.Exists(y => y.Character == x.Character) == false)
        {
            return false;//we don't have a character we need. so, ransom note cannot be written
        }

        var tempITem = ListOfMagazineSet.Select(y => y).Where(y => y.Character == x.Character).First(); //if we reached here we know the required character is already there. 
        if(x.CharacterCount <= tempITem.CharacterCount)
        {
            //if characters neccessary for ransom is available in magazine
            //do nothing
        }
        else
        {
            return false;
        }
    }

    return true;
}



#endregion

#region  9. Palindrome Number

// https://leetcode.com/problems/palindrome-number/description/

var checkPal = 10;
//var responseIsPalindrome = IsPalindrome(checkPal);

 bool IsPalindrome(int x)
{
    //negative numbers cannot be palindrome becuase of the - symbol
    if(x < 0)
        return false;
    //its easy to do this if it is a string
    var stringOfX = x.ToString().ToArray();
    //anb
    var ReverseOfX = stringOfX.Reverse().ToArray();
    for(int i =0;i< stringOfX.Length;i++)
    {
        var leftchar = stringOfX[i];
        var rightchar = ReverseOfX[i];
        if (stringOfX[i] != ReverseOfX[i])
            return false;
    }

    return true;
    
}

#endregion



Console.WriteLine();

# region some helper classes used for solutions above

class CanConstructOne
{
    public char Character;
    public int CharacterCount;
}

#endregion
