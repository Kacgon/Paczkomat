using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paczkomat
{
    public class Pracownik 
    {
        public int idPracownika {  get; set; }
        public string imie {  get; set; }
        public string nazwisko {  get; set; }

        DBmanager dBmanager = new DBmanager();

        List<object> mixedList = new List<object>();

        public List<object> DaneDoPrzekazania()
        {
            mixedList.Add(idPracownika);
            mixedList.Add(imie);
            mixedList.Add(nazwisko);

            return mixedList;
        }

        public Pracownik()
        {
            Console.WriteLine("Podaj swoje ID pracownika");
            int idPracownika = Convert.ToInt32(Console.ReadLine());
            this.idPracownika = idPracownika;
        }

        public Pracownik(int idPracownika)
        {
            this.idPracownika = idPracownika;
        }
        
        public Pracownik(int idPracownika, string imie, string nazwisko)
        {
            this.idPracownika= idPracownika;
            this.imie = imie;
            this.nazwisko = nazwisko;
        }

        public void PokazMojeZamowienia()
        {
            dBmanager.WyswietlZamowieniaPracownika(idPracownika);
        }

        public void NadajPaczkeKurier()
        {
            Console.WriteLine("Podaj odbiorce");
            int idUzytkownika = Convert.ToInt32(Console.ReadLine());    
            dBmanager.NadaniePaczki(idUzytkownika);
        }

        public void OdbierzPaczkeKurier()
        {
            dBmanager.OdbierzPaczkeKurier(idPracownika);
        }

        public void NajlepszyPracownik()
        {
            dBmanager.CsvImport();
        }
        public void ObslugaKuriera()
        {
            Console.WriteLine("Wybierz operacje: \n 1) Wyswietl twoje zamowienia  \n 2)Nadaj paczkę \n 3) Odbierz paczkę \n4) najlepszy pracownik miesiąca:");
            int operacja = Convert.ToInt32(Console.ReadLine());
            switch (operacja)
            {
                case 1:
                    PokazMojeZamowienia();
                    break;
                case 2:
                    NadajPaczkeKurier();
                    break;
                case 3:
                    OdbierzPaczkeKurier();
                    break;
                case 4:
                    NajlepszyPracownik();
                    break;
                default:
                    Console.WriteLine("Nie poprawna operacja");
                    break;
            }


        }
    }
}
