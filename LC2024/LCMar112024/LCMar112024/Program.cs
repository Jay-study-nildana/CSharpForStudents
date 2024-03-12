// See https://aka.ms/new-console-template for more information
using System.Reflection;

Console.WriteLine("Hello, World!");

#region 27. Remove Element

//https://leetcode.com/problems/remove-element/description/

var RemoveElementnums = new int[] { 0, 1, 2, 2, 3, 0, 4, 2 };
int RemoveElementval = 2;

//var responseRemoveElement = RemoveElement(RemoveElementnums, RemoveElementval);

//First Solution. extremely slow.

int RemoveElement(int[] nums, int val)
{
    var response = 0;
    var outputArray = new int[nums.Length];

    foreach (var item in nums)
    {
        if(item == val)
        {
            //do nothing. this means, its being remove

        }
        else
        {
            outputArray[response] = item;
            response++; //count the numbers which are not val
        }
    }
    outputArray.CopyTo(nums, 0);
    return response;
}

//a second solution
//var responseRemoveElement2 = RemoveElement2(RemoveElementnums, RemoveElementval);

//Second Solution. a little faster. 

int RemoveElement2(int[] nums, int val)
{
    var response = 0;
    var tracker = 0;

    var afterremovingval = nums.ToList<int>().Select(x => x).Where(x => x != val).ToList(); //simply remove the val from the original collection
    response = afterremovingval.Count; //count what is remaining
    afterremovingval.ToArray<int>().CopyTo(nums, 0); //just convert the remaining collection into array

    return response;
}

#endregion

#region 26. Remove Duplicates from Sorted Array

//  https://leetcode.com/problems/remove-duplicates-from-sorted-array/description/

var RemoveDuplicatesnums = new int[] { 1, 1, 2 };
//var responseRemoveDuplicates = RemoveDuplicates(RemoveDuplicatesnums);

int RemoveDuplicates(int[] nums)
{
    var response = 0;
    var uniqueCollection = new List<int>();

    foreach(var x in nums)
    {
        if(uniqueCollection.Contains(x) == true)
        {
            //element already included. nothing to do. 
        }
        else
        {
            //element not in collection. add it. 
            uniqueCollection.Add(x);
        }
    }
    response = uniqueCollection.Count;
    uniqueCollection.ToArray<int>().CopyTo(nums, 0);

    return response;
}

#endregion

#region 1346. Check If N and Its Double Exist

// https://leetcode.com/problems/check-if-n-and-its-double-exist/description/

var CheckIfExistarr = new int[] { 10, 2, 5, 3 };
var CheckIfExistarr2 = new int[] { 3, 1, 7, 11 }; 

//var responseCheckIfExist = CheckIfExist(CheckIfExistarr2);
bool CheckIfExist(int[] arr)
{
    var response = false;

    for(int i=0;i < arr.Length;i++)
    {
        var currentnumber = arr[i];
        for(int j=0; j < arr.Length;j++)
        {
            if(currentnumber == arr[j] * 2 && i!=j)
            {
                return true;
            }
        }
    }

    return response;
}

//var responseCheckIfExist2 = CheckIfExist2(CheckIfExistarr2);

//faster solution
bool CheckIfExist2(int[] arr)
{
    var response = false;
    var checkList = arr.ToList();

    //the original solution used to loops. this one uses just one, speeeding things up

    for (int i = 0; i < arr.Length; i++)
    {
        var currentnumber = arr[i];

        if (currentnumber % 2 != 0)
        {
            //odd number. just ignore.
        }
        else
        {
            var checkDividedby2 = checkList.Contains(currentnumber / 2);
            if (checkDividedby2 == true)
            {
                double indexOfFoundNumber = checkList.FindIndex(x => x == currentnumber / 2); //we know the number is odd. So, we can simply divide by 2
                //and locate the index of that number.

                if (indexOfFoundNumber == i) //we know i cannot be same as j. NOTE: I am not using j variable at all. 
                {
                    // do nothing/ this is not valid because i = j in this case. 
                }
                else
                {
                    return true;
                }
            }
        }

    }

    return response;
}

#endregion

#region 941. Valid Mountain Array

//https://leetcode.com/problems/valid-mountain-array/description/

var VMAarr = new int[] { 2, 1 };
var VMAarr2 = new int[] { 3, 5, 5 };
var VMAarr3= new int[] { 0, 3, 2, 1 };
var VMAarr4 = new int[] { 1, 3, 2 };
var VMAarr5 = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
var VMAarr6 = new int[] { 3, 7, 6, 4, 3, 0, 1, 0 };

//var responseVMA = ValidMountainArray(VMAarr6); ;

//TODO. I kept failing multiple times
//I am not sure in a real coding test, they will give me all the test cases. 
bool ValidMountainArray(int[] arr)
{
    var response = false;
    var peak = 0;
    var climbingup = true; //in the beginning we are climbing.
    var climbingdown = false;

    if(arr.Length <=2)
    {
        //less than 2, no mountain can be made, I think.
        return false;
    }

    for(int i=0;i<arr.Length-1;i++)
    {
        var currentnumber = arr[i];
        var nextnumber = arr[i+1];

        if(currentnumber == nextnumber)
        {
            return false;//two consequetive number means no strict increase or decrease.
        }

        if(currentnumber < nextnumber && climbingup == true && climbingdown == false)
        {
            peak = nextnumber;
        }
        else if(climbingdown == false) //direction reversal is a one time event. 
        {
            climbingdown = true; //moutain peak is one way. you start by going up. then you go down. you cannot go back up. 
            climbingup = false;
            i++; //we have reached the peak. next number must be lower so move on to the next number
            currentnumber = arr[i];
            if(i+1 == arr.Length)
            {
                //end of the array
                //last item has to be lower. so return true.
                return true;
            }
            else
            {
                nextnumber = arr[i + 1];
            }

        }

        //start checking for going down.
        if(climbingup == false && climbingdown == true)
        {
            if (currentnumber < peak && currentnumber > nextnumber)
            {
                //we are lower than the peak, so, we are going down for sure
                //and currently higher than the next step
                //do nothing. all good. 
                response = true;
                peak = currentnumber;
            }
            else
            {
                //suddenly we have a number that is greater than peak
                //or we have a number that is greater than the current step
                return false;
            }
        }


    }

    return response;
}


#endregion

#region 1299. Replace Elements with Greatest Element on Right Side

// https://leetcode.com/problems/replace-elements-with-greatest-element-on-right-side/description/

var REarr = new int[] { 17, 18, 5, 4, 6, 1 };
//var responseRE1 = ReplaceElements(REarr);
int[] ReplaceElements(int[] arr)
{
    if(arr.Length == 1)
    {
        //only 1 item means, we just get -1
        arr[arr.Length - 1] = -1;
        return arr; 
    }  

    if(arr.Length == 2)
    {
        //only two items means, the last number becomes the first number
        //and the 2nd (the new last number) becomes -1
        //Last but one number is always the last number
        arr[arr.Length - 2] = arr[arr.Length - 1];
        //last number is -1
        arr[arr.Length - 1] = -1;

        return arr;
    }

    for(int i=0;i< arr.Length-2;i++)
    {
        var greatestnumber = arr[i+1]; //start by assuming that the number is the greatest number.
        for(int j=i+2;j < arr.Length;j++)
        {
            if (arr[j] > greatestnumber) //see if you can find a new greater number
                greatestnumber = arr[j]; 
        }
        arr[i] = greatestnumber;
    }
    //Last but one number is always the last number
    arr[arr.Length-2] = arr[arr.Length-1];
    //last number is -1
    arr[arr.Length-1] = -1;

    return arr;
}

#endregion

#region 283. Move Zeroes

// https://leetcode.com/problems/move-zeroes/description/

var MZnums = new int[] { 0, 1, 0, 3, 12 };
var MZnums2 = new int[] { 1, 0 };
//MoveZeroes(MZnums2);

void MoveZeroes(int[] nums)
{
    var currentposition = 0;

    if(nums.Length == 1)
    {
        //just one element. do nothing. 
    }
    else
    {
        for (int i = 0; i < nums.Length; i++)
        {

            for (int j = currentposition; j < nums.Length; j++)
            {
                if (nums[j] == 0)
                {
                    //do nothing. 
                }
                else
                {
                    //we found a non-zero number. 
                    nums[currentposition] = nums[j];
                    if(currentposition!=j) //don't overwrite current position. you just updated it!
                    {
                        nums[j] = 0;
                    }
                    currentposition++;
                    j = nums.Length; //end loop.
                }
            }
        }
    }

}


#endregion

#region 905. Sort Array By Parity

//https://leetcode.com/problems/sort-array-by-parity/description/

var SAPnums = new int[] { 3, 1, 2, 4 };
//var SAPresponse1 = SortArrayByParity(SAPnums);

int[] SortArrayByParity(int[] nums)
{
    var returnarray = new int[nums.Length];

    var intcounter = 0;
    var oddcounter = nums.Length - 1;

    for(int i = 0;i< nums.Length;i++)
    {
        if (nums[i] %2 ==  0)
        {
            //push from the beginning
            returnarray[intcounter] = nums[i];
            intcounter++;
        }
        else
        {
            //push from the end
            returnarray[oddcounter] = nums[i];
            oddcounter--;
        }
    }
    return returnarray;
}

#endregion

Console.WriteLine();