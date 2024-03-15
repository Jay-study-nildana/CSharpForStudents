// See https://aka.ms/new-console-template for more information

#region 487. Max Consecutive Ones II

//TODO. 
//memory is beating 90%, so, that's good.
//runtime is a problem with only 9 % beaten

//https://leetcode.com/problems/max-consecutive-ones-ii/description/

var FMCOnums = new int[] { 1, 0, 1, 1, 0 };
//var responseFMCO1 = FindMaxConsecutiveOnes(FMCOnums);
int FindMaxConsecutiveOnes(int[] nums)
{
    var high = 0;

    for(int i=0;i< nums.Length; i++)
    {
        var currenthigh = 0;
        var foundzero = false;
        for (int j=i;j<nums.Length;j++)
        {

            
            if (nums[j] == 1 && foundzero == false)
            {
                currenthigh++; //we are seeing 1s and nothing else.
            }
            else if (nums[j] == 0 && foundzero == false)
            {
                //we met a zero for the first time. flip it to 1. 
                //move to the next item
                //j++;
                currenthigh++; //count it as 1 due to the flip
                foundzero = true; //to inform that we already performed the flip
            }
            else if(foundzero==true && nums[j] == 1)
            {
                //okay we have flipped but we are seeing 1s and nothing else. 
                currenthigh++;
            }
            else
            {
                //we have already flipped and we have met a (second) zero
                //stop the loop. 
                j = nums.Length;
            }

        }

        //check if we have found a new max continous 1s
        if (high < currenthigh)
        {
            high = currenthigh;
            currenthigh = 0;  //reset the high and get it ready for the next loop
            foundzero = false;//rest and get ready for the next loop
        }
        else
        {
            currenthigh = 0;  //reset the high and get it ready for the next loop
            foundzero = false;//rest and get ready for the next loop
        }

        if (high >= nums.Length - (i+1))
        {
            //stop this loop because, we already have the longest sequence.
            i = nums.Length;
        }

    }

    return high;
}

#endregion

#region 414 Third Maximum Number

//https://leetcode.com/problems/third-maximum-number/

var TMnums = new int[] { 3, 2, 1, 2 };
var TMnums2 = new int[] { 1,1,2 };
//var TMresponse1 = ThirdMax(TMnums2);

int ThirdMax(int[] nums)
{
    var response = 0;

    //once again, I am not sure if we are allowed to use sort facility. but I am using it.
    //same with distinct facility
    var tempnums = nums.OrderDescending().Distinct().ToArray();

    //remove duplicates
    

    if(tempnums.Length >= 3)
    {
        response = tempnums[2];
    }
    else if(tempnums.Length == 2)
    {
        if (tempnums[0] > tempnums[1])
            response = tempnums[0];
        else
            response = tempnums[1];
    }
    else
    {
        response = tempnums[0];
    }

    return response;
}

#endregion

#region 448 Find All Numbers Dissappeared in an Array

//https://leetcode.com/problems/find-all-numbers-disappeared-in-an-array/description/

var FDNnums = new int[] { 4, 3, 2, 7, 8, 2, 3, 1 };
var FDNnums2 = new int[] { 1,1 };
var FDNnums3 = new int[] { 1, 1, 2, 2 };
//var responseFDNa = FindDisappearedNumbers(FDNnums2);

//TODO. not good either with RunTime or Memory. 
//this is a solution but not even a good solution.
IList<int> FindDisappearedNumbers(int[] nums)
{
    var response = new List<int>();
    var responsecounter = 0;

    //once again, TODO, not sure if we can use sort functions. 
    //but I will use it. 

    var sortednums = nums.Order().Distinct().ToArray();

    var nextnumber = 0;
    var sortednumtracker = 0;
    for(int i =0;i<nums.Length;i++)
    {
        
        if(sortednumtracker >= sortednums.Length)
        {
            //we have hit the end of the distinct elements in our distinct sorted array
            //but items are still missing. just add them up.
            nextnumber++;  //for example, if 1 is missing, we just checked it againsts 0 so we store the next number
            //response[responsecounter] = nextnumber; //I just realized, we have to return an array
            response.Add(nextnumber); //add number to the list.
            responsecounter++; //move on to the next location where missing numbers will be stored.
        }
        else
        {
            //the idea is, either the number exists in the original array
            //or, the number is missing, and we put it in the output array
            //thus for looping through the entire expected number set
            //satisfying the loop condition
            if (sortednums[sortednumtracker] == nextnumber + 1) //if we have the immediate next number, then we are fine
            {
                //move on to the next number
                sortednumtracker++;
                nextnumber++;//get ready to check the next number
            }
            else
            {
                //we have a winner. let's put it in our output array
                nextnumber++;  //for example, if 1 is missing, we just checked it againsts 0 so we store the next number
                               //response[responsecounter] = nextnumber; //I just realized, we have to return an array
                response.Add(nextnumber); //add number to the list.
                responsecounter++; //move on to the next location where missing numbers will be stored.
            }
        }
        


    }

    return response;
}


#endregion

Console.WriteLine("Hello, World!");
