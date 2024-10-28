using Datalagring_CarPark_EFCore.Data;
using Datalagring_CarPark_EFCore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using static System.Console;

namespace Datalagring_CarPark_EFCore
{
    class Program
    {
        //static string connectionString = "Server=(local);Database=CarPark;Trusted_Connection=True";
        //static CarParkContext Context = new CarParkContext();

        static ILoggerFactory MyLoggerFactory = LoggerFactory
            .Create(DbContextOptionsBuilder => { DbContextOptionsBuilder.AddConsole(); });

        static CarParkContext Context = new CarParkContext(MyLoggerFactory);

        static void Main(string[] args)
        {
            bool shouldExit = false;

            while (!shouldExit)
            {
                WriteLine("1. Register arrival");
                WriteLine("2. Register departure");
                WriteLine("3. Show registry");
                WriteLine("4. Exit");

                ConsoleKeyInfo keyPressed = ReadKey(true);

                Clear();

                switch (keyPressed.Key)
                {
                    // Register arrival
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        RegisterArrival();
                        ReadKey(true);
                        break;

                    // Register departure
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        RegisterDeparture();
                        ReadKey(true);
                        break;

                    // Show registry
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:

                        ShowRegistry();
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:

                        shouldExit = true;

                        break;
                }

                Clear();
            }
        }

        private static void ShowRegistry()
        {
            List<Parking> parkingList = FetchAllParkings();

            foreach (Parking parking in parkingList)
            {
                Write($"{parking.Person.FirstName} {parking.Person.LastName}".PadRight(20, ' '));
                Write(parking.Vehicle.RegistrationNumber.PadRight(10, ' '));
                Write(parking.ArrivedAt.ToString().PadRight(25, ' '));
                WriteLine(parking.DepartedAt);
            }

            ReadKey(true);
        }

        private static List<Parking> FetchAllParkings()
        {
            List<Parking> parkingList = Context.Parking
                .Include(x => x.Person)
                .Include(x => x.Vehicle)
                .ToList();

            return parkingList;
        }

        private static void RegisterArrival()
        {
            // Vehicle
            Write("Registration number: ");
            string registrationNumber = ReadLine();
            Write("Notes: ");
            string notes = ReadLine();
            Vehicle vehicle = new Vehicle(registrationNumber, notes);

            // Person
            Write("Social security number: ");
            string socialSecurityNumber = ReadLine();
            Write("First name: ");
            string firstName = ReadLine();
            Write("Last name: ");
            string lastName = ReadLine();
            Write("Phone number: ");
            string phoneNumber = ReadLine();
            Write("E-mail: ");
            string email = ReadLine();
            Person person = new Person(firstName, lastName, socialSecurityNumber, phoneNumber, email);

            Parking parking = new Parking(person, vehicle);
            SaveParking(parking);           

            Clear();
            WriteLine("Arrival registered");
            Thread.Sleep(2000);
        }


        private static void SaveParking(Parking parking)
        {
            if (parking.Id < 1)
            {
                Context.Parking.Add(parking);
            }

            Context.SaveChanges();
        }

        private static void RegisterDeparture()
        {
            Write("Registration number: ");

            string registrationNumber = ReadLine();

            Parking parking = FetchActiveParkingsByRegistrationNumber(registrationNumber);

            Clear();

            if (parking != null)
            {
                //parking.Departed();
                parking.DepartedAt = DateTime.Now;
                SaveParking(parking);

                WriteLine("Departure registered");
            }
            else
            {
                WriteLine("Parking not found");
            }

            Thread.Sleep(2000);
        }

        private static Parking FetchActiveParkingsByRegistrationNumber(string registrationNumber)
        {
            return Context.Parking
                .Include(x => x.Vehicle)
                .FirstOrDefault(x => x.Vehicle.RegistrationNumber == registrationNumber
                && x.DepartedAt == null);
        }
    }
}













//private static void ShowRegistry()
//{
//    List<Parking> parkingList = FetchAllActiveParkings();

//    foreach (Parking parking in parkingList)
//    {
//        Write($"{parking.Person.FirstName} {parking.Person.LastName}".PadRight(20, ' '));
//        Write(parking.Vehicle.RegistrationNumber.PadRight(10, ' '));
//        Write(parking.ArrivedAt.ToString().PadRight(25, ' '));
//        WriteLine(parking.DepartedAt);
//    }
//}

//private static List<Parking> FetchAllActiveParkings()
//{
//    // TODO: Fetch parkings from database

//    return new List<Parking>();
//}

//private static void RegisterArrival()
//{
//    // Vehicle
//    Write("Registration number: ");

//    string registrationNumber = ReadLine();

//    Write("Notes: ");

//    string notes = ReadLine();

//    Vehicle vehicle = new Vehicle(registrationNumber, notes);

//    SaveVehicle(vehicle);



//    // Person
//    Write("Social security number: ");

//    string socialSecurityNumber = ReadLine();

//    Write("First name: ");

//    string firstName = ReadLine();

//    Write("Last name: ");

//    string lastName = ReadLine();

//    Write("Phone number: ");

//    string phoneNumber = ReadLine();

//    Write("E-mail: ");

//    string email = ReadLine();

//    Person person = new Person(firstName, lastName, socialSecurityNumber, phoneNumber, email);

//    SavePerson(person);


//    Parking parking = new Parking(person, vehicle);


//    SaveParking(parking);

//    Clear();

//    WriteLine("Arrival registered");

//    Thread.Sleep(2000);
//}

//private static Person SavePerson(Person person)
//{
//    Person newPerson = null;

//    // 1. Vi behöver själv skapa den SQL som behöver köras
//    string cmdText = @"
//                INSERT INTO Person (
//                    FirstName, 
//                    LastName, 
//                    SocialSecurityNumber, 
//                    PhoneNumber, 
//                    Email
//                ) VALUES (
//                    @FirstName,
//                    @LastName,
//                    @SocialSecurityNumber,
//                    @PhoneNumber,
//                    @Email
//                )
//            ";

//    // 2. Vi behöver upprätta anslutning till RDBMS.
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    using (SqlCommand command = new SqlCommand(cmdText, connection))
//    {
//        connection.Open();

//        SqlParameter IDParameter = new SqlParameter("@Id", SqlDbType.Int);
//        IDParameter.Direction = ParameterDirection.Output;

//        // 3. Behöver mappa parameterized query parameters
//        command.Parameters.AddWithValue("FirstName", person.FirstName);
//        command.Parameters.AddWithValue("LastName", person.LastName);
//        command.Parameters.AddWithValue("SocialSecurityNumber", person.SocialSecurityNumber);
//        command.Parameters.AddWithValue("PhoneNumber", person.PhoneNumber);
//        command.Parameters.AddWithValue("Email", person.Email);
//        command.Parameters.Add(IDParameter);

//        var test = command.ExecuteScalar();

//        //command.ExecuteNonQuery();

//        // (int)IDParameter.Value;

//        connection.Close();
//    }

//    //new Person(1, person.FirstName, person.LastName, person.SocialSecurityNumber, person.PhoneNumber, person.Email);
//    return newPerson;
//}

//private static void SaveVehicle(Vehicle vehicle)
//{
//    // 1. Vi behöver själv skapa den SQL som behöver köras
//    string cmdText = @"
//                INSERT INTO Vehicle (RegistrationNumber, Notes)
//                VALUES (@RegistrationNumber, @Notes)
//            ";

//    // 2. Vi behöver upprätta anslutning till RDBMS.
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    using (SqlCommand command = new SqlCommand(cmdText, connection))
//    {
//        connection.Open();

//        // 3. Behöver mappa parameterized query parameters
//        command.Parameters.AddWithValue("RegistrationNumber", vehicle.RegistrationNumber);
//        command.Parameters.AddWithValue("Notes", vehicle.Notes);

//        command.ExecuteNonQuery();

//        connection.Close();
//    }
//}

//private static void SaveParking(Parking parking)
//{
//    // 1. Vi behöver själv skapa den SQL som behöver köras
//    string cmdText = @"
//                INSERT INTO Parking (
//                    PersonId, 
//                    VehicleId, 
//                    ArrivedAt, 
//                    DepartedAt
//                ) VALUES (
//                    @PersonId, 
//                    @VehicleId,
//                    @ArrivedAt,
//                    @DepartedAt
//                )
//            ";

//    // 2. Vi behöver upprätta anslutning till RDBMS.
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    using (SqlCommand command = new SqlCommand(cmdText, connection))
//    {
//        connection.Open();

//        // 3. Behöver mappa parameterized query parameters
//        command.Parameters.AddWithValue("PersonId", parking.Person.Id);
//        command.Parameters.AddWithValue("VehicleId", parking.Vehicle.Id);
//        command.Parameters.AddWithValue("ArrivedAt", parking.ArrivedAt);
//        //command.Parameters.AddWithValue("DepartedAt", parking.DepartedAt);

//        command.ExecuteNonQuery();

//        connection.Close();
//    }
//}

//private static void RegisterDeparture()
//{
//    Write("Registration number: ");

//    string registrationNumber = ReadLine();

//    Parking parking = FetchActiveParkingsByRegistrationNumber(registrationNumber);

//    if (parking != null)
//    {
//        parking.Departed();

//        SaveParking(parking);
//    }
//    else
//    {
//        Clear();

//        WriteLine("Parking not found");

//        Thread.Sleep(2000);
//    }
//}

//private static Parking FetchActiveParkingsByRegistrationNumber(string registrationNumber)
//{
//    Parking parking = null;

//    // 1. Vi behöver själv skapa den SQL som behöver köras
//    string cmdText = @"
//                    SELECT Parking.Id as ParkingId,
//	                       Parking.ArrivedAt,
//	                       Parking.DepartedAt,
//	                       Parking.VehicleId,
//	                       Vehicle.RegistrationNumber,
//	                       Vehicle.Notes,
//                           Parking.PersonId,
//	                       Person.FirstName,
//	                       Person.LastName,
//	                       Person.SocialSecurityNumber,
//	                       Person.PhoneNumber,
//	                       Person.Email
//                      FROM Parking
//                    INNER JOIN Vehicle ON Parking.VehicleId = Vehicle.Id
//                    INNER JOIN Person ON Parking.PersonId = Person.Id
//                    WHERE Vehicle.RegistrationNumber = @RegistrationNumber
//                      AND DepartedAt IS NULL
//            ";

//    // 2. Vi behöver upprätta anslutning till RDBMS.
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    using (SqlCommand command = new SqlCommand(cmdText, connection))
//    {
//        connection.Open();

//        // 3. Behöver mappa parameterized query parameters
//        command.Parameters.AddWithValue("RegistrationNumber", registrationNumber);

//        SqlDataReader dataReader = command.ExecuteReader();

//        if (dataReader.Read())
//        {
//            // Person
//            int personId = int.Parse(dataReader["PersonId"].ToString());
//            string firstName = dataReader["FirstName"].ToString();
//            string lastName = dataReader["LastName"].ToString();
//            string socialSecurityNumber = dataReader["SocialSecurityNumber"].ToString();
//            string phoneNumber = dataReader["PhoneNumber"].ToString();
//            string email = dataReader["Email"].ToString();

//            Person person = new Person(personId, firstName, lastName, socialSecurityNumber, phoneNumber, email);

//            // Vehicle
//            int vehicleId = int.Parse(dataReader["VehicleId"].ToString());
//            string registrationNumber2 = dataReader["registrationNumber"].ToString();
//            string notes = dataReader["notes"].ToString();

//            Vehicle vehicle = new Vehicle(vehicleId, registrationNumber2, notes);

//            // Parking
//            int parkingId = int.Parse(dataReader["ParkingId"].ToString());
//            DateTime arrivedAt = DateTime.Parse(dataReader["ArrivedAt"].ToString());
//            DateTime departedAt = DateTime.Parse(dataReader["DepartedAt"].ToString());

//            parking = new Parking(person, vehicle, arrivedAt, departedAt);
//        }

//        connection.Close();
//    }

//    return parking;
//}