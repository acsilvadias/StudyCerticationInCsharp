using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Dynamic;

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
            //CallParallelLinqQuery();
            //CallExercicioMeasureUp();
            //CallExpandoObjectSample();
            CallEnforceEcapsulation();


            Console.WriteLine("Finished processing. Press a key to end.");
            Console.ReadKey();
        }



        #region ***** Use Enforce encapsulation 2.29 *****
        class BAnckAccount
        {
            private decimal _accountBalance = 0;

            public void PayInFunds(decimal amountToPayIn)
            {
                _accountBalance = _accountBalance + amountToPayIn;
            }

            public bool WithdrawFunds(decimal amountWithdraw)
            {
                if (amountWithdraw > _accountBalance)
                    return false;

                _accountBalance = _accountBalance - amountWithdraw;
                return true;
            }

            public decimal GetBalance()
            {
                return _accountBalance;
            }
        }

        private static void CallEnforceEcapsulation()
        {
            BAnckAccount c = new BAnckAccount();
            c.PayInFunds(50);
            Console.WriteLine("Pay in 50");
            c.PayInFunds(50);
            if (c.WithdrawFunds(10))
                Console.WriteLine("Withdraw 10");

            Console.WriteLine("Account balance is: {0}", c.GetBalance());

        }
        #endregion

        #region ***** Use ExpandoObject Properties Listing 2-25 *****
        private static void CallExpandoObjectSample()
        {
            dynamic person = new ExpandoObject();

            person.Name = "Antônio Carlos";
            person.Age = 43 + 1;
            Console.WriteLine("Name: {0} Age: {1}", person.Name, person.Age);
        }
        #endregion
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

            ParallelLoopResult result = Parallel.For(0, items.Count(), (int i, ParallelLoopState p) =>
            {
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

        #region ***** ExercicioMeasureUp 01 *****
        private static void CallExercicioMeasureUp()
        {
            Costumer customer1 = new Costumer { Name = "Janet" };
            TestCustomer1(customer1);

            Costumer customer2 = new Costumer { Name = "Kim" };
            TestCustomer2(ref customer2);

            Card card1 = new Card { Number = "1111" };
            TestCard1(card1);

            Card card2 = new Card { Number = "2222" };
            TestCard2(ref card2);

            Console.WriteLine("customer1: " + customer1.Name);
            Console.WriteLine("customer2: " + customer2.Name);
            Console.WriteLine("card1: " + card1.Number);
            Console.WriteLine("card2: " + card2.Number);

        }

        static void TestCustomer1(Costumer customer) { customer.Name = "Kim"; }
        static void TestCustomer2(ref Costumer customer) => customer.Name = "Janet";

        static void TestCard1(Card card) => card.Number = "2222";

        static void TestCard2(ref Card card) => card.Number = "1111";

        internal class Costumer
        {
            private string name;
            public string Name { get => name; set => name = value; }

        }

        internal class Card
        {
            private string number;
            public string Number { get => number; set => number = value; }

        }
        #endregion
    }

}
