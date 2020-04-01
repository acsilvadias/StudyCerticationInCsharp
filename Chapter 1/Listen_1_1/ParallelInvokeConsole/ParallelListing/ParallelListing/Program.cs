using System;
using System.Threading.Tasks;
using System.Threading;

namespace ParallelListing
{
    class Program
    {

        static void Main(string[] args)
        {
            CallParallelInvoke();

            Console.WriteLine("Finished processing. Press a key to end.");
            Console.ReadKey();
        }


    #region ***** Parallel.Invoke *****
        private static void CallParallelInvoke()
        {
            Parallel.Invoke(() => Task1(), () => Task2());
        }
        static void Task1()
        {
            Console.WriteLine("Task1 1 starting");
            Thread.Sleep(2000);
            Console.WriteLine("Task1 1 ending");
        }

        static void Task2()
        {
            Console.WriteLine("Task1 2 starting");
            Thread.Sleep(2000);
            Console.WriteLine("Task1 2 ending");
        }
    #endregion
    }
}
