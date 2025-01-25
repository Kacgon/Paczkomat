using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paczkomat
{
    public class Zamowienie
    {
        DBmanager dbmanager = new DBmanager();
        private int idZamowienia { get; set; }
        private string nazwa { get; set; }
        private string miasto { get; set; }
        private int numerDomu { get; set; }
        private int numerLokalu { get; set; }
        private int idKuriera { get; set; }

        private int kodOdbioru { get; set; }

        List<object> mixedList = new List<object>();

        public List<object> DaneDoPrzekazania()
        {
            mixedList.Add(idZamowienia);
            mixedList.Add(nazwa);
            mixedList.Add(miasto);  
            mixedList.Add(numerDomu);
            mixedList.Add(numerLokalu);
            mixedList.Add(idKuriera);
            mixedList.Add(kodOdbioru);

            return mixedList;
        }

        public Zamowienie()
        {
            idZamowienia = (dbmanager.Licznik("select count(id_zamowienie) from zamowienie") + 1);

            Console.WriteLine("Podaj nazwe");
            nazwa = Console.ReadLine();

            Console.WriteLine("Podaj miasto");
            miasto = Console.ReadLine();

            Console.WriteLine("Podaj numer domu");
            numerDomu = int.Parse(Console.ReadLine());

            Console.WriteLine("Podaj numer lokalu");
            numerLokalu = int.Parse(Console.ReadLine());

            Random losowa = new Random();
            int wylosowanyKurier = losowa.Next(1, dbmanager.Licznik("select count(id_kuriera) from kurier"));
            idKuriera = wylosowanyKurier;

            Console.WriteLine("Podaj kod odbioru");
            kodOdbioru = int.Parse(Console.ReadLine());
        }
    }
}
