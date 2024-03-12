// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

# region 4. Median of Two Sorted Arrays


//https://leetcode.com/problems/median-of-two-sorted-arrays/description/


var nums1 = new int[] { 1, 2 };
var nums2 = new int[] { 3, 4 };
//double response = FindMedianSortedArrays(nums1, nums2);
double FindMedianSortedArrays(int[] nums1, int[] nums2)
{
    double response = 0.0;
    var totallength = nums1.Length + nums2.Length;
    var mergedarray = new int[totallength];
    var firstcounter = 0;
    var secondcounter = 0;
    for (int i = 0; i < nums1.Length + nums2.Length; i++)
    {
        if (secondcounter == nums2.Length)
        {
            //just copy the first array
            mergedarray[i] = nums1[firstcounter];
            //move to the next number
            firstcounter++;
        }
        else if (firstcounter == nums1.Length)
        {
            //just copy the rest of the second array.
            mergedarray[i] = nums2[secondcounter];
            //move to the next number
            secondcounter++;
        }

        else if (nums1[firstcounter] < nums2[secondcounter])
        {
            mergedarray[i] = nums1[firstcounter];
            //move to the next number
            firstcounter++;
        }
        else
        {
            mergedarray[i] = nums2[secondcounter];
            //move to the next number
            secondcounter++;
        }
    }

    //array merged. now, get the median.
    int middlelocation = (mergedarray.Length / 2);
    response = mergedarray[middlelocation];

    if (mergedarray.Length % 2 == 0) //even numbers. so we need two numbers for median
    {
        //go one level back
        middlelocation--;
        double leftnumber = (double)mergedarray[middlelocation];
        double rightnumber = (double)mergedarray[middlelocation + 1];

        response = (leftnumber + rightnumber) / 2;
    }


    return response;
}

Console.WriteLine("Hello, World!");

#endregion

#region 1051. Height Checker

//https://leetcode.com/problems/height-checker/

var HCheights = new int[] { 1, 1, 4, 2, 1, 3 };
//var responseHC1 = HeightChecker(HCheights);
int HeightChecker(int[] heights)
{
    var response = 0;

    //TODO is it possible do this without using the sort function here? 
    //not sure if we can use sort functions here. but I will use it. 
    var expectedarray = heights.Order().ToArray();

    for(int i =0;i<heights.Length;i++)
    {
        if (heights[i] == expectedarray[i])
        {
            //matching. do nothing
        }
        else
        {
            response++; //found a mismatch.
        }
    }

    return response;
}

#endregion