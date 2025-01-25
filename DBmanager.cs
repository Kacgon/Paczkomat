using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Paczkomat;
using System.Security.Cryptography.X509Certificates;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;

public class DBmanager
{
    string connectionString = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=Paczkomat;Integrated Security=True;Trust Server Certificate=True";

    public void NadaniePaczki(int IdUzytkownik)
    {
        if (IdUzytkownik > Licznik("select count(*) from uzytkownik"))
        {
            throw new ArgumentException("Nie ma takiego Id.");
        }

        string query = "INSERT INTO zamowienie(id_zamowienie,nazwa, miasto,nr_domu, uzytkownik, id_kuriera, kod_odbioru) VALUES (@id,@nazwa, @miasto,@nr_domu, @uzytkownik, @kurier,@kod)";
        Zamowienie zamowienie = new Zamowienie();
        var listaDanych = zamowienie.DaneDoPrzekazania();
        try
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", listaDanych[0]);
                    command.Parameters.AddWithValue("@nazwa", listaDanych[1]);
                    command.Parameters.AddWithValue("@miasto", listaDanych[2]);
                    command.Parameters.AddWithValue("@nr_domu", listaDanych[3]);
                    command.Parameters.AddWithValue("@uzytkownik", IdUzytkownik);
                    command.Parameters.AddWithValue("@kurier", listaDanych[5]);
                    command.Parameters.AddWithValue("@kod", listaDanych[6]);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }
    }

    public void CsvImport()
    {
        using (var reader = new StreamReader("NajlepszyPracownik.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Pracownik>();
            foreach (var record in records)
            {
                Console.WriteLine($"Id: {record.idPracownika}, Imie:{ record.imie}, Nazwisko: { record.nazwisko}");
            }
        }
    }

    public void CsvExport()
    {
        Console.WriteLine("Podaj dane najlepszego pracownika miesiąca");
        Console.WriteLine("Id:");
        int id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Imie:");
        string imie = Console.ReadLine();
        Console.WriteLine("Nazwisko:");
        string nazwisko = Console.ReadLine();
        

        var osoby = new List<Pracownik>
         {
            new Pracownik(id,imie,nazwisko)
         };
        using (var writer = new StreamWriter("NajlepszyPracownik.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
        csv.WriteRecords(osoby);
        }   
        Console.WriteLine("Dane zapisane do pliku CSV.");
    }

    public void getUserData(int UserId)
    {
        string QueryForUser = "select * from uzytkownik where id_uzytkownik = @id";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(QueryForUser, connection))
            {
                command.Parameters.AddWithValue("@id", UserId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int? id = reader.IsDBNull(0) ? null : reader.GetInt32(0);
                        string? imie = reader.IsDBNull(1) ? null : reader.GetString(1);
                        string? nazwisko = reader.IsDBNull(2) ? null : reader.GetString(2);

                        Console.WriteLine($"Id: {id}, Imie: {imie}, Nazwisko:{nazwisko}");
                    }
                }
            }
        }
    }
    public int Licznik(string queryLiczenie)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryLiczenie, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int suma = reader.GetInt32(0);
                            return suma;
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }
        return 0;
    }

    public bool? Weryfikacja(int kurierId)
    {
        string queryWeryfikacja = "SELECT poziom_dostepu FROM kurier WHERE id_kuriera = @id";
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryWeryfikacja, connection))
                {
                    command.Parameters.AddWithValue("@id", kurierId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool? poziomDostępu = reader.IsDBNull(0) ? null : reader.GetBoolean(0);
                            return poziomDostępu;
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }
        return false;
    }

    public void WyswietlZamowieniaUzytkownika(int idUzytkownika)
    {
        string queryZamowUzytkownik = "select * from zamowienie where uzytkownik = @idUzytkownika";
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryZamowUzytkownik, connection))
                {
                    command.Parameters.AddWithValue("@idUzytkownika", idUzytkownika);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? id = reader.IsDBNull(0) ? null : reader.GetInt32(0);
                            string? nazwa = reader.IsDBNull(1) ? null : reader.GetString(1);
                            string? miasto = reader.IsDBNull(2) ? null : reader.GetString(2);
                            int? uzytkownik = reader.IsDBNull(8) ? null : reader.GetInt32(8);
                            int? kurier = reader.IsDBNull(9) ? null : reader.GetInt32(9);

                            Console.WriteLine($"Id: {id}, Nazwa: {nazwa}, Miasto:{miasto}, Nadawca: {uzytkownik}, kurier:{kurier}");
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }
    }

    public void WyswietlZamowieniaPracownika(int idKuriera)
    {
        string queryZamowienKuriera = "select * from zamowienie where id_kuriera = @idKuriera";
        try
        {


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryZamowienKuriera, connection))
                {
                    command.Parameters.AddWithValue("@idKuriera", idKuriera);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? id = reader.IsDBNull(0) ? null : reader.GetInt32(0);
                            string? nazwa = reader.IsDBNull(1) ? null : reader.GetString(1);
                            string? miasto = reader.IsDBNull(2) ? null : reader.GetString(2);
                            int? uzytkownik = reader.IsDBNull(8) ? null : reader.GetInt32(8);
                            int? kurier = reader.IsDBNull(9) ? null : reader.GetInt32(9);

                            Console.WriteLine($"Id: {id}, Nazwa: {nazwa}, Miasto:{miasto}, Nadawca: {uzytkownik}, kurier:{kurier}");
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }
    }

    public void OdbierzPaczkeKurier(int idKuriera)
    {
            string query = "DELETE from zamowienie where id_zamowienie = @Id and id_kuriera = @IdKuriera";
            Console.Write("Podaj id zamowienie");
            int zamowienieDoWybrania = Convert.ToInt32(Console.ReadLine());

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", zamowienieDoWybrania);
                        command.Parameters.AddWithValue("@IdKuriera", idKuriera);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("Nie poprawne id paczki");
                        }
                        Console.WriteLine($"Odebrano: {rowsAffected} paczek");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("nie masz dostepu do tego zamowienia"); }  
    }

    public void AdminWyswietl()
    {
        string QueryDlaAdmin = "SELECT * from zamowienie";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(QueryDlaAdmin, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int? id = reader.IsDBNull(0) ? null : reader.GetInt32(0);
                            string? nazwa = reader.IsDBNull(1) ? null : reader.GetString(1);
                            string? miasto = reader.IsDBNull(2) ? null : reader.GetString(2);
                            int? uzytkownik = reader.IsDBNull(8) ? null : reader.GetInt32(8);
                            int? kurier = reader.IsDBNull(9) ? null : reader.GetInt32(9);

                            Console.WriteLine($"Id: {id}, Nazwa: {nazwa}, Miasto:{miasto}, Nadawca: {uzytkownik}, kurier:{kurier}");
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }
    }

    public void ZmianaStatusuPaczki()
    {
        Console.WriteLine("Wybierz zamówienie którego status chcesz zmienić");
        int zamowienieDoZmiany = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Podaj nowy status");
        string status = Console.ReadLine();

        string query = "UPDATE zamowienie SET status_zamowienia = @status WHERE id_zamowienie = @Id";

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", zamowienieDoZmiany);
                    command.Parameters.AddWithValue("@status", status);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }
    }
    public void DodajUzytkownika(Osoba osoba)
    {
        string query = "INSERT INTO uzytkownik(id_uzytkownik, nazwisko, imie) VALUES (@id_uzytkownik, @nazwisko, @imie)";
        var listaDanych = osoba.DaneDoPrzekazania();
        try
        {


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_uzytkownik", listaDanych[0]);
                    command.Parameters.AddWithValue("@nazwisko", listaDanych[1]);
                    command.Parameters.AddWithValue("@imie", listaDanych[2]);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }
    }
    public void DodajPracownika(Pracownik nowyKurier)
    {
        string query = "INSERT INTO kurier(id_kuriera, nazwisko, imie) VALUES (@id_kuriera, @nazwisko, @imie)";
        var listaDanych = nowyKurier.DaneDoPrzekazania();

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_kuriera", listaDanych[0]);
                    command.Parameters.AddWithValue("@nazwisko", listaDanych[1]);
                    command.Parameters.AddWithValue("@imie", listaDanych[2]);

                    int rowsAffected = command.ExecuteNonQuery();
                    
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }
    }

    public void UsunPracownika()
    {
        {
            string query = "DELETE from kurier where id_kuriera = @id_kuriera";
            Console.Write("Podaj id kuriera do usunięcia");
            int idUsunietego = Convert.ToInt32(Console.ReadLine());

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id_kuriera", idUsunietego);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("Nie poprawne id pracownika");
                        }
                        Console.WriteLine($"Usunieto: {rowsAffected} pracownikow");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Błąd operacji: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Operacja zakończona.");
            }
        }
    }
    
    public void OdebraniePaczki()
    {
        string query = "DELETE FROM zamowienie WHERE id_zamowienie = @Id AND kod_odbioru=@kodWeryfikacji";
        Console.WriteLine("Wpisz id zamowienia które chcesz wybrać z paczkomatu");
        int idPaczki = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("W celu weryfikacji podaj swój kod uzytkownika");
        int kodWeryfikacji = Convert.ToInt32(Console.ReadLine());

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", idPaczki);
                    command.Parameters.AddWithValue("@kodWeryfikacji", kodWeryfikacji);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("Nie poprawny kod lub id");
                    }
                    Console.WriteLine($"Odebrano: {rowsAffected} paczek");
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd z połączeniem do bazy danych: " + ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Błąd operacji: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nieobslugiwany bład: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Operacja zakończona.");
        }

    }
}




