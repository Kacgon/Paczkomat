using System;
using System.Security.Cryptography.X509Certificates;
using Paczkomat;
using Microsoft.Data.SqlClient;
using ProjektPaczkomat;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        DateTime aktualnaGodzina = DateTime.Now;
        DBmanager managerBazy = new DBmanager();
        Console.WriteLine($"System zarządzania punktem odbioru paczek              Aktualna data: {aktualnaGodzina}");

        bool toggle = true; 

        while (toggle)
        {
            Console.WriteLine("\nWybierz Operację");
            Console.WriteLine("1) Tryb klienta");
            Console.WriteLine("2) Tryb pracownika");
            Console.WriteLine("3) Wyjście");

            string wyborMenu = Console.ReadLine();

            if (wyborMenu == "1") 
            {
                bool klientMenu = true;
                while (klientMenu)
                {
                    Console.WriteLine("\nTryb klienta:");
                    Console.WriteLine("1) Obsluga klienta");
                    Console.WriteLine("2) Powrót do głównego menu");

                    string klientWybor = Console.ReadLine();

                    if (klientWybor == "1")
                    {
                        Console.WriteLine("Podaj swoje ID użytkownika:");
                        int id = Convert.ToInt32(Console.ReadLine());
                        if (id > managerBazy.Licznik("select count(*) from uzytkownik"))
                        {
                            Console.WriteLine("Nie ma takiego id");
                            klientMenu = false;
                        }
                        else
                        {
                            Osoba uzytkownik = new Osoba(id);
                            uzytkownik.ObslugaKlienta();
                        }
 
                    }
                    else if (klientWybor == "2")
                    {
                        klientMenu = false; 
                    }
                    else
                    {
                        Console.WriteLine("Niepoprawny wybór, spróbuj ponownie.");
                    }
                }
            }
            else if (wyborMenu == "2") 
            {
                bool pracownikMenu = true;
                while (pracownikMenu)
                {
                    Console.WriteLine("\nTryb pracownika:");
                    Console.WriteLine("1) Kurier");
                    Console.WriteLine("2) Administrator");
                    Console.WriteLine("3) Powrót do głównego menu");

                    string pracownikWybor = Console.ReadLine();

                    if (pracownikWybor == "1") 
                    {
                        bool kurierMenu = true;
                        while (kurierMenu)
                        {
                            Console.WriteLine("\nTryb kuriera:");
                            Console.WriteLine("1)Obsluga kuriera");
                            Console.WriteLine("2) Powrót do menu pracownika");

                            string kurierWybor = Console.ReadLine();

                            if (kurierWybor == "1")
                            {
                                Pracownik kurier = new Pracownik();
                                if (kurier.idPracownika > managerBazy.Licznik("select count(*) from uzytkownik"))
                                {
                                    Console.WriteLine("Nie ma takiego id");
                                    kurierMenu = false;
                                }
                                else
                                {
                                    kurier.ObslugaKuriera();
                                }
                            }
                            else if (kurierWybor == "2")
                            {
                                kurierMenu = false; 
                            }
                            else
                            {
                                Console.WriteLine("Niepoprawny wybór, spróbuj ponownie.");
                            }
                        }
                    }
                    else if (pracownikWybor == "2") 
                    {
                        Console.WriteLine("Podaj swoje ID w celu weryfikacji dostępu do trybu administratora:");
                        int idAdmina = Convert.ToInt32(Console.ReadLine());
                        Admin admin = new Admin(idAdmina);
                        admin.ObslugaAdmina(); 
                    }
                    else if (pracownikWybor == "3")
                    {
                        pracownikMenu = false; 
                    }
                    else
                    {
                        Console.WriteLine("Niepoprawny wybór, spróbuj ponownie.");
                    }
                }
            }
            else if (wyborMenu == "3") 
            {
                toggle = false;
                Console.WriteLine("Program zakonczony");
            }
            else
            {
                Console.WriteLine("Niepoprawny wybór, spróbuj ponownie.");
            }
        }
    }
}
