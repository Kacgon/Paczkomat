using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paczkomat
{
    internal class Admin : Pracownik
    {
        DBmanager dbmanager = new DBmanager();
        private bool? poziomDostepu { get; set; }
  

        public Admin(int idPracownika) : base(idPracownika)
        {
            this.poziomDostepu = dbmanager.Weryfikacja(idPracownika);
        }

        public void ObslugaAdmina()
        {
            if (poziomDostepu == true)
            {
                try
                {
                    Console.WriteLine("Wybierz operacje: \n 1) Wyświetl Paczki  \n 2) Zmień status paczki \n 3) Dodaj osobę \n 4) Dodaj pracownika \n 5) Usuń pracownika \n6) zapisz najlepszego pracownika");
                    int operacja = Convert.ToInt32(Console.ReadLine());

                    switch (operacja)
                    {
                        case 1:
                            WyswietlWszystkieZamowienia();
                            break;
                        case 2:
                            ZmienStatusPaczki();
                            break;
                        case 3:
                            DodajOsobe();
                            break;
                        case 4:
                            DodajPracownika();
                            break;
                        case 5:
                            UsunPracownika();
                            break;
                        case 6:
                            ZapiszNajlepszegoPracownika();
                            break;
                        default:
                            Console.WriteLine("Niepoprawna operacja, wybierz jedną z podanych");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Błąd: Wprowadzono złe dane");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Nie masz dostępu do tego trybu.");
            }

        }

        public void DodajPracownika()
        {
            string queryLicznika = "select count(*) from kurier";

            Console.WriteLine("Podaj imie");
            string imieNowego  = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko");
            string nazwiskoNowego = Console.ReadLine();   


            Pracownik nowyPracownik = new Pracownik(dbmanager.Licznik(queryLicznika) +1, imieNowego, nazwiskoNowego);  
            dbmanager.DodajPracownika(nowyPracownik);
        }
        public void DodajOsobe()
        {
            Osoba nowaOsoba = new Osoba();
            dbmanager.DodajUzytkownika(nowaOsoba);
        }

        public void WyswietlWszystkieZamowienia()
        {
            dbmanager.AdminWyswietl();
        }

        public void ZmienStatusPaczki()
        {
            dbmanager.ZmianaStatusuPaczki();
        }

        public void UsunPracownika()
        {
            dbmanager.UsunPracownika();
        }

        public void ZapiszNajlepszegoPracownika()
        {
            dbmanager.CsvExport();
        }
    }
}
