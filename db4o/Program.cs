using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db4objects.Db4o;
using Idbo;


namespace db4o
{
    public enum OperatorType
    {
        stacjonarny, 
        komorkowy 
    }
    public class Person
    {
        public string Imie { get; set; }
        public string Nazwisko {get; set;}
        public Address Adres { get; set; }
        public List<Phone> Telefon { get; set; }
    }

    public class Address
    {
        public string Ulica { get; set; }
        public string Miasto { get; set; }
    }

    public class Phone
    {
        public int Numer { get; set; }
        public string operatorTel { get; set; }
    }

    public class Program
    {
        public static void ListResult(List<Person> result)
        {
            Console.WriteLine(result.Count);
            foreach (var item in result)
            {
                try
                {
                    Console.WriteLine("Imie: " + item.Imie);
                    Console.WriteLine("Nazwisko: " + item.Nazwisko);
                    Console.WriteLine("Ulica: " + item.Adres.Ulica);
                    Console.WriteLine("Miasto: " + item.Adres.Miasto);

                    foreach (var itemTel in item.Telefon)
                    {
                        Console.WriteLine("Telefon: " + itemTel.Numer );
                        Console.WriteLine("Operator: " + itemTel.operatorTel);
                    }
                    
                }
                catch
                { }
                    Console.WriteLine();
            }
        }

        public static void wyjscie()
        {
            Console.Clear();
            Environment.Exit(0);
        }


        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            do
            {
                Console.Clear();
                Console.WriteLine("Witaj w bazie:");
                Console.WriteLine("==============");
                Console.WriteLine();
                Console.WriteLine("0 -> Wyswietlenie danych");
                Console.WriteLine("1 -> Wprowadzenie nowej osoby");
                Console.WriteLine("9 -> WYJSCIE");
                Console.WriteLine();

                IObjectContainer db = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), "person.data");

                cki = Console.ReadKey();

                if (cki.Key == ConsoleKey.D0) //wyswietlanie
                {
                    try
                    {
                        Console.Clear();
                        var osoby = db.Query<Person>().ToList();
                        ListResult(osoby);
                        Console.WriteLine();
                        Console.WriteLine("Nacisnij ENTER by wrocic do menu.");
                        Console.ReadLine();
                        db.Close();
                    }
                    finally
                    {
                        db.Close();
                    }
                }

                else if (cki.Key == ConsoleKey.D1) //dodawanie
                {
                    var person = new Person();
                    person.Telefon = new List<Phone>();
                    Console.Clear();
                    Console.Write("Podaj imie:\n");
                    person.Imie = Console.ReadLine();
                    Console.Write("\nPodaj nazwisko:\n");
                    person.Nazwisko = Console.ReadLine();

                    Console.WriteLine("\nCzy chcesz dodać adres? [T/N]");
                    ConsoleKeyInfo takNie = Console.ReadKey();
                    if (takNie.Key == ConsoleKey.T)
                    {
                        var address = new Address();
                        Console.WriteLine("\n\nPodaj ulice:");
                        address.Ulica = Console.ReadLine();
                        Console.WriteLine("\nPodaj miasto:");
                        address.Miasto = Console.ReadLine();
                        person.Adres = address;
                    }

                    do
                    {
                        if (takNie.Key != ConsoleKey.N)
                        {
                            Console.WriteLine("\nCzy chcesz dodać telefon? [T/N]");
                            takNie = Console.ReadKey();
                            if (takNie.Key == ConsoleKey.T)
                            {
                                var telefon = new Phone();
                                Console.WriteLine("\n\nPodaj telefon:");
                                try
                                {
                                    telefon.Numer = int.Parse(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("\n Bledny numer. Wracamy do menu.");
                                    break;
                                }
                                Console.WriteLine("\nPodaj operatora:");
                                telefon.operatorTel = Console.ReadLine();
                                person.Telefon.Add(telefon);
                            }
                        }
                    } while (takNie.Key != ConsoleKey.N);
                    
                    try
                    {
                        db.Store(person);
                        db.Commit();
                        Console.WriteLine();
                        Console.WriteLine("Dane zostaly zapisane poprawnie. Wracamy do menu.");
                        Console.ReadLine();
                        db.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Wpis nie zostal dodany");
                        Console.WriteLine(e);
                        Console.ReadLine();
                    }

                    finally
                    {
                        db.Close();
                    }
                }

                db.Close();
            } while (cki.Key != ConsoleKey.D9);

            wyjscie();


            //IObjectContainer db = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), "person.data");
            //try
            //{

            //    //Person stefan = new Person("Max", "Mucha", 25);
            //    //Person halina = new Person("Min", "Komar", 26);

            //    //db.Store(stefan);
            //    //db.Store(halina);

            //    IObjectSet osoby = db.QueryByExample(new Person(null, null, 0));
            //    IObjectSet result = db.QueryByExample(osoby);
            //    ListResult(osoby);



            //    Console.Read();
            //}
            //finally
            //{
            //    db.Close();
            //}
        }


    }
}
