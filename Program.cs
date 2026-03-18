using Microsoft.EntityFrameworkCore;
using Project4.Models;
using System.ComponentModel.DataAnnotations;

namespace Project4
{
    class Program
    {
        static void Main(string[] args)
        {
            AirportDbContext dbContext = new AirportDbContext();

            dbContext.Clients.Add(new Client()
            {
                Name = "Olga",
                Email = "olga@gmail.com",
                Birthdate = new DateTime(1995, 5, 14),

            });
            //dbContext.SaveChanges();
            //foreach (var client in dbContext.Clients)
            //{
            //    Console.WriteLine($"{client.Name}. Email: {client.Email}");
            //}

            // Include data loading : Include(relation data) - JOIN SQL
            var flights = dbContext.Flights
                .Include(f=>f.Airplane)
                .Where(f => f.ArrivalCity == "Kyiv")
                .OrderBy(f => f.ArrivalTime);
            foreach (var flight in flights)
            {
                Console.WriteLine($"From : {flight.ArrivalCity}. To : {flight.DepartureCity}.\n" +
                    $"Date : {flight.ArrivalTime}\n" +
                    $"AirplaneId : {flight.AirplaneId}" +
                    $" Name Airplane : {flight.Airplane?.Model}\n" +
                    $"Max count passangers : {flight.Airplane?.MaxCountPassangers}\n");
                //if (flight.Airplane != null)
                //    Console.WriteLine(flight.Airplane.Model);
                //else
                //    Console.WriteLine("Airplane not set");
            }


            var client = dbContext.Clients.Find(1);
            // Reference    Collection
            dbContext?.Entry(client).Collection(c=>c.Flights).Load();
            Console.WriteLine($"Name : {client?.Name}. Rating : {client?.Rating}");
            Console.WriteLine($"All flights : {client?.Flights.Count}");

            foreach (var flight in client!.Flights)
            {
                Console.WriteLine($"From : {flight.ArrivalCity}. To : {flight.DepartureCity}.\n" +
                    $"Date : {flight.ArrivalTime}\n");
            }

            //var airplanes = dbContext.Airplanes
            //    .Where(a => a.MaxCountPassangers < 100)
            //    .OrderBy(a => a.MaxCountPassangers);


            //foreach (var item in airplanes)
            //{
            //    Console.WriteLine($"{item.Id}. {item.Model}. {item.MaxCountPassangers}");
            //}

            Console.Clear();

            // 1. ВИВІД КРАЇН ТА МІСТ (Зв'язок One-to-Many)
            Console.WriteLine("--- Countries and their Cities ---");
            var countries = dbContext.Set<Project6_DataAccessAirport.Models.Countries>()
                                     .Include(c => c.Cities)
                                     .ToList();

            foreach (var country in countries)
            {
                Console.WriteLine($"Country: {country.Name}");
                foreach (var city in country.Cities)
                {
                    Console.WriteLine($"  -> City: {city.Name}");
                }
            }
            Console.WriteLine();

            // 2. ВИВІД ТИПІВ ЛІТАКІВ ТА МОДЕЛЕЙ
            Console.WriteLine("--- Airplane Types and Models ---");
            var types = dbContext.Set<Project6_DataAccessAirport.Models.AirplaneTypes>()
                                 .Include(t => t.Airplane) // Використовуємо назву колекції з вашого класу
                                 .ToList();

            foreach (var type in types)
            {
                Console.WriteLine($"Type: {type.Name}");
                foreach (var plane in type.Airplane)
                {
                    Console.WriteLine($"  -> Model: {plane.Model} (Capacity: {plane.MaxCountPassangers})");
                }
            }
            Console.WriteLine();

            // 3. ВИВІД РЕЙСІВ (З повною інформацією про літак та місто прибуття)
            Console.WriteLine("--- All Flights Information ---");
            var flights1 = dbContext.Flights
                .Include(f => f.Airplane)
                .Include(f => f.Cities) // Місто з таблиці Cities
                .OrderBy(f => f.ArrivalTime)
                .ToList();

            foreach (var flight in flights1)
            {
                Console.WriteLine($"Flight #{flight.Number}");
                Console.WriteLine($"Route: {flight.DepartureCity} -> {flight.ArrivalCity} (Base City: {flight.Cities?.Name})");
                Console.WriteLine($"Time: {flight.DepartureTime} - {flight.ArrivalTime}");
                Console.WriteLine($"Airplane: {flight.Airplane?.Model} (Type ID: {flight.Airplane?.AirplaneTypesId})");
                Console.WriteLine(new string('-', 30));
            }
            Console.WriteLine();

            // 4. ВИВІД ПАСАЖИРІВ (Клієнтів)
            Console.WriteLine("--- Registered Passengers ---");
            var clients = dbContext.Clients.ToList();
            foreach (var c in clients)
            {
                Console.WriteLine($"ID: {c.Id} | Name: {c.Name} | Email: {c.Email} | Rating: {c.Rating}");
            }
        }
    }
}
