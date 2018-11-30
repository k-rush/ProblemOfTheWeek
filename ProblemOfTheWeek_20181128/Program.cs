using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProblemOfTheWeek_20181128
{
    class Program
    {
        static void Main(string[] args)
        {

            Random rand = new Random();
            int numInts = Int32.Parse(args[0]);
            int[] intArray = new int[numInts];

            //Generate random integers..
            Console.WriteLine($"Generating {numInts} random integers");
            for (int i = 0; i < numInts; i++)
            {
                intArray[i] = rand.Next(-10000, 10000);
            }

            Console.WriteLine($"Starting execution of optimized solution with {intArray.Length} ints.");
            Stopwatch optimizedTime = Stopwatch.StartNew();
            Console.WriteLine($"Count: {CountOptimized(intArray)}");
            optimizedTime.Stop();
            Console.WriteLine($"Optimized execution time: {(double)optimizedTime.ElapsedMilliseconds / 1000} s");

            Console.WriteLine($"Starting execution of naive solution with {intArray.Length} ints.");
            Stopwatch naiveTime = Stopwatch.StartNew();
            Console.WriteLine($"Count: {CountNaive(intArray)}");
            naiveTime.Stop();
            Console.WriteLine($"Naive execution time: {(double)naiveTime.ElapsedMilliseconds / 1000} s");
        }

        public static int CountNaive(int[] a)
        {
            int n = a.Length;
            int count = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    for (int k = j + 1; k < n; k++)
                    {
                        if (a[i] + a[j] + a[k] == 0)
                            count++;
                    }
                }
            }

            return count;
        }

        public static int CountOptimized(int[] a)
        {
            int n = a.Length;
            a = a.OrderBy(k => k).ToArray();
            int count = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    count += GetNumberCount(-(a[i] + a[j]), a);
                }
            }
            return (int)Math.Floor((double)(count + 2) / 3);
        }

        private static Dictionary<int, int> numbersCount = new Dictionary<int, int>();
        //Gets the count of a certain number in a given array.  Uses memoization.
        public static int GetNumberCount(int partialSum, int[] a)
        {
            if (numbersCount.ContainsKey(partialSum))
            {
                return numbersCount[partialSum];
            }
            else
            {

                int count = BinarySearchCounter(partialSum, a);
                numbersCount[partialSum] = count;
                return count;
            }
        }

        //Finds and counts a given key in a sorted array starting with a binary search.
        public static int BinarySearchCounter(int key, int[] a)
        {
            int lower = 0;
            int upper = a.Length - 1;
            int index;
            int count = 0;
            while ((upper - lower) > 1)
            {
                index = lower + (int)Math.Floor(((double)(upper - lower) / 2));
                if (key == a[index])
                {
                    count = 1;
                    int incrementingIndex = index + 1;
                    int decrementingIndex = index - 1;
                    while (incrementingIndex < (a.Length - 1) && key == a[incrementingIndex++]) count++;
                    while (decrementingIndex > 0 && key == a[decrementingIndex--]) count++;
                    return count;
                }
                else if (key < a[index]) upper = index;
                else if (key > a[index]) lower = index;
            }
            return 0;

        }


    }
}
