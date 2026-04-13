# List of Algorithms

There are several common problems, implemented in code in various folders, that keep coming back in this curriculum. With algorithms, there are two steps:

1. **Step One:** Understand the algorithm itself, with pen and paper.
2. **Step Two:** Implement the algorithm in any way you wish.  
   - There can be multiple implementations of the same algorithm.

---

## Example: Bubble Sort

### Part One: Algorithm in Simple English

1. Compare each pair of adjacent elements in a list.
2. If the elements are in the wrong order, swap them.
3. Repeat this process for each element, reducing the range each time.
4. Continue until no swaps are needed (the list is sorted).

### Part Two: Implementations

#### 1. Bubble Sort with Arrays

```csharp
public static void BubbleSort(int[] arr)
{
    int n = arr.Length;
    for (int i = 0; i < n - 1; i++)
    {
        for (int j = 0; j < n - i - 1; j++)
        {
            if (arr[j] > arr[j + 1])
            {
                int temp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = temp;
            }
        }
    }
}
```

#### 2. Bubble Sort with Lists

```csharp
public static void BubbleSort(List<int> list)
{
    int n = list.Count;
    for (int i = 0; i < n - 1; i++)
    {
        for (int j = 0; j < n - i - 1; j++)
        {
            if (list[j] > list[j + 1])
            {
                int temp = list[j];
                list[j] = list[j + 1];
                list[j + 1] = temp;
            }
        }
    }
}
```

#### 3. Generic Bubble Sort

```csharp
public static void BubbleSort<T>(IList<T> list, IComparer<T> comparer = null)
{
    comparer ??= Comparer<T>.Default;
    int n = list.Count;
    for (int i = 0; i < n - 1; i++)
    {
        for (int j = 0; j < n - i - 1; j++)
        {
            if (comparer.Compare(list[j], list[j + 1]) > 0)
            {
                T temp = list[j];
                list[j] = list[j + 1];
                list[j + 1] = temp;
            }
        }
    }
}
```

#### 4. Bubble Sort with Linked List (Singly Linked)

```csharp
public class Node
{
    public int Data;
    public Node Next;
    public Node(int data) { Data = data; }
}

public static void BubbleSortLinkedList(Node head)
{
    if (head == null) return;
    bool swapped;
    do
    {
        swapped = false;
        Node current = head;
        while (current.Next != null)
        {
            if (current.Data > current.Next.Data)
            {
                int temp = current.Data;
                current.Data = current.Next.Data;
                current.Next.Data = temp;
                swapped = true;
            }
            current = current.Next;
        }
    } while (swapped);
}
```

## Actual List of Algorithms 

1. BoundedRecentActivity
Maintain a list of the most recent activities up to a fixed size. When a new activity occurs, add it to the list. If the list exceeds the maximum size, remove the oldest activity.
2. ConcurrentFrequencyCount
Track how many times each item appears, allowing updates from multiple threads at the same time. When an item is seen, safely increment its count.
3. CountDistinct
Count the number of unique items in a collection. As you process each item, keep track of which items you have already seen.
4. Repository (Crud + Memory)
Store objects in memory and provide methods to create, read, update, and delete them. Each object can be found by a unique identifier.
5. GroupAnagrams
Group words that are anagrams of each other together. For each word, sort its letters and use the sorted word as a key to group similar words.
6. GroupByKey
Organize items into groups based on a key. For each item, determine its key and add it to the group for that key.
7. MergeSort
Divide the list into halves until each part has one element. Then, merge the parts back together in sorted order.
8. PairAndSwap
Go through the list two elements at a time and swap each pair. If there is an odd element at the end, leave it as is.
9. RemoveInPlace
Remove all items matching a condition from a list without creating a new list. Shift the remaining items to fill the gaps.
10. TopKbyFrequency
Find the K items that appear most frequently. Count how often each item appears, then select the K items with the highest counts.

