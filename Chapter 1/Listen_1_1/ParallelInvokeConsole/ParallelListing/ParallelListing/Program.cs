using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Dynamic;
using System.Globalization;

namespace ParallelListing
{
    class Program
    {

        static void Main(string[] args)
        {
            CallStringInterpolation();

            //CallMusicTrackFormatter();
            //CallStringComparisonAndCulture();
            //CallStartWithAndEndWith();
            //CallParallelInvoke();
            //CallParallelForEach();
            //CallParallelForEachFor();
            //CallParallelLoopResult();
            //CallParallelLinqQuery();
            //CallExercicioMeasureUp();
            //CallExpandoObjectSample();
            //CallEnforceEcapsulation();
            
            Console.WriteLine("Finished processing. Press a key to end.");
            Console.ReadKey();
        }

        #region ***** String comparison ,cultures  and interpolation *****

        private static void CallStringInterpolation()
        {
            string name = "Bob";
            int age = 21;

            Console.WriteLine("Your name is {0} and your age is {1,-5:D}", name, age);
            Console.WriteLine($"With Interpolation Your name is {name} and your age is {age,-5:D}");

            Console.WriteLine($"C#,7");
            Console.WriteLine($"[{"VB",7}]");

        }


        private static void CallMusicTrackFormatter()
        {
            MusicTrack song = new MusicTrack(artist: "Rob Miles", title: "My Way");

            Console.WriteLine("Track: {0:F}", song);
            Console.WriteLine("Artist: {0:A}", song);
            Console.WriteLine("Title: {0:T}", song);

        }
        class MusicTrack : IFormattable
        {
            string Artist{ get ; set; }
            string Title{ get; set ; }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                if (string.IsNullOrWhiteSpace(format))
                    format = "G";

                switch (format)
                {
                    case "A": return Artist;
                    case "T": return Title;

                    case "G"://default format
                    case "F": return Artist + " " + Title;
                    default:
                        throw new FormatException("Format spefier was invalid.");
                }


            }

            public override string ToString()
            {
                return Artist + " " + Title;
            }

            public MusicTrack(string artist, string title)
            {
                Artist = artist;
                Title = title;
            }

        }
        private static void CallStringComparisonAndCulture()
        {
            int i = 99;
            double pi = 3.141592654;

            Console.WriteLine(" {0, -10:D}{0, -10:X}{1,5:N2} {1,5:C2}", i, pi);

            double value = 12345.6789;

            Console.WriteLine(value.ToString("C2",CultureInfo.CurrentCulture));
            Console.WriteLine(value.ToString("C3", CultureInfo.CurrentCulture));
            Console.WriteLine(value.ToString("C", CultureInfo.CreateSpecificCulture("en-US")));



        }

        #endregion

        #region ***** StringBuilder 2.67,68,69 *****
        private static void CallStartWithAndEndWith()
        {
            string strSample = " Antônio Carlos da Silva Dias";

            Console.WriteLine("Nome Atual: [ {0} ] deve retirar --> [{1} ]", strSample, " da");
            Console.WriteLine("Nome para exibir: {0}", RemoveString(strSample," da"));
        }

        private static string RemoveString(string source, string search)
        {
            int position = source.IndexOf(search);
            string newSource = null;
            if (position >= 0)
            {
                newSource = source.Remove(position, search.Length);
            }

            return newSource;
        }
        
        #endregion

        #region ***** Use Enforce encapsulation 2.29 *****

        class BanckAccount 
        {
            private decimal _accountBalance = 0;

            public void PayInFunds(decimal amountToPayIn)
            {
                _accountBalance = _accountBalance + amountToPayIn;
            }

            public virtual bool WithdrawFunds(decimal amountWithdraw)
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

        private class OverdraftAccount : BanckAccount
        {
            decimal overdraftLimit = 100;
            decimal _amountbalance = 0;

            public override bool WithdrawFunds(decimal amountToWithdraw)
            {
                if (amountToWithdraw > base.GetBalance() + overdraftLimit)
                    return false;

                _amountbalance = _amountbalance - amountToWithdraw;
                return true;
            }
        }
        private static void CallEnforceEcapsulation()
        {
            OverdraftAccount c = new OverdraftAccount();
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
