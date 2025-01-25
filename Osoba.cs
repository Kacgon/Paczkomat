using ProjektPaczkomat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paczkomat
{
    public class Osoba
    {
        private int id { get; set; }
        private string imie { get; set; }
        private string nazwisko { get; set; }

        DBmanager dBmanager = new DBmanager();

        public Osoba()
        {
            string queryLicznika = "select count(*) from uzytkownik";
            this.id = dBmanager.Licznik(queryLicznika) +1;
            Console.WriteLine("Podaj imie osoby");
            this.imie = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko osoby");
            this.nazwisko = Console.ReadLine();
        }

        public Osoba(int id)
        {
            this.id = id;   
        }

        List<object> mixedList = new List<object>();

        public List<object> DaneDoPrzekazania()
        {
            mixedList.Add(id);
            mixedList.Add(imie);
            mixedList.Add(nazwisko);

            return mixedList;
        }

        public void NadajPaczke()
        {
            dBmanager.NadaniePaczki(id);
        }
        public void OdbierzPaczke() 
        {
            dBmanager.OdebraniePaczki();
        }

        public void WyswietlZamowienia()
        {
            dBmanager.WyswietlZamowieniaUzytkownika(id);
        }
        public void WyswietlDane()
        {
            Console.WriteLine("Dane uzytkownika");
            dBmanager.getUserData(this.id);
        }

        public void ObslugaKlienta()
        {
            Console.WriteLine("Wybierz operację: \n1)Wyświetl swoje dane \n2)Wyświetl swoje zamówienia \n3)Nadaj paczkę \n4)Odbierz paczkę");
            int wybranaOperacja = Convert.ToInt32(Console.ReadLine());

            switch (wybranaOperacja)
            {
                case 1:
                    WyswietlDane();
                    break;
                case 2:
                    WyswietlZamowienia();
                    break;
                case 3:
                    NadajPaczke();
                    break;
                case 4:
                    OdbierzPaczke();
                    break;

            }
   }    }
}
