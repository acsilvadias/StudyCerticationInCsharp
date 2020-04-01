using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace ParallelListing
{
    class Program
    {

        static void Main(string[] args)
        {
            //CallParallelInvoke();
            //CallParallelForEach();
            //CallParallelForEachFor();
            //CallParallelLoopResult();
            CallParallelLinqQuery();

            Console.WriteLine("Finished processing. Press a key to end.");
            Console.ReadKey();
        }


        #region ***** Parallel.Linq.Query Listing 5 *****
        class Person
        {
            public string Name { get; set; }
            public string City { get; set; }

        }
        static void CallParallelLinqQuery()
        {
            Person[] people = new Person[]
            {
                new Person { Name = "Alan", City = "Hull" },
                new Person { Name = "Beryl", City = "Seattle" },
                new Person { Name = "Charles", City = "London" },
                new Person { Name = "David", City = "Seattle" },
                new Person { Name = "Eddy", City = "Paris" },
                new Person { Name = "Fred", City = "Berlin" },
                new Person { Name = "Gordon", City = "Hull" },
                new Person { Name = "Henry", City = "Seattle" },
                new Person { Name = "Isaac", City = "Seattle" },
                new Person { Name = "James", City = "London" }
            };

            var result = from person in people.AsParallel()
                         where person.City == "Seattle"
                         select person;
            foreach (var person in result)
            {
                Console.WriteLine(person.Name);
            }


        }

        #endregion

        #region ***** ParallelLoopResult Listing 4 *****
        static void CallParallelLoopResult()
        {
            var items = Enumerable.Range(0, 500).ToArray();

            ParallelLoopResult result = Parallel.For(0,items.Count(),(int i, ParallelLoopState p) =>{
                if (i == 200)
                    p.Stop();

                WorkOnItem(items[i]);

            });

            Console.WriteLine("Completed: " + result.IsCompleted);
            Console.WriteLine("Items: " + result.LowestBreakIteration);
        }
        #endregion

        #region ***** Parallel.For Listing 3 *****

        private static void CallParallelForEachFor()
        {
            var items = Enumerable.Range(0, 500).ToArray();

            Parallel.For(0, items.Length, i =>
            {
                WorkOnItem(items[i]);

            });
        }

        #endregion

        #region ***** Parallel.ForEach Listing 2 *****
        private static void CallParallelForEach()
        {
            var items = Enumerable.Range(0, 500);
            Parallel.ForEach(items, item =>
            {
                WorkOnItem(item);
            });
        }


        #endregion

        #region ***** Parallel.Invoke Listing 1 *****
        static void WorkOnItem(object item)
        {
            Console.WriteLine("Starting Working om: " + item);
            Thread.Sleep(100);
            Console.WriteLine("Finished Working om: " + item);
        }
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
