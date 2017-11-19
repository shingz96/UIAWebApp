using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIAWebApp.Models;

namespace UIAWebApp.Data
{
    public class FlightDbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Flights.Any())
            {
                return;   // DB has been seeded
            }

            var flights = new Flight[]
            {
            new Flight{Origin="Kuala Lumpur",Destination="Singapore",ArrivalDateTime=DateTime.Parse("2017-11-19 12:00 AM"), DepartureDateTime=DateTime.Parse("2017-11-19 2:00 AM"),Fare=200},
            new Flight{Origin="Kuala Lumpur",Destination="Singapore",ArrivalDateTime=DateTime.Parse("2017-11-19 1:00 AM"), DepartureDateTime=DateTime.Parse("2017-11-19 3:00 AM"),Fare=200},
            new Flight{Origin="Kuala Lumpur",Destination="Singapore",ArrivalDateTime=DateTime.Parse("2017-11-19 3:00 PM"), DepartureDateTime=DateTime.Parse("2017-11-19 4:00 PM"),Fare=200},

            new Flight{Origin="Kuala Lumpur",Destination="Beijing",ArrivalDateTime=DateTime.Parse("2017-11-19 12:00 AM"), DepartureDateTime=DateTime.Parse("2017-11-19 2:00 AM"),Fare=500},
            new Flight{Origin="Kuala Lumpur",Destination="Beijing",ArrivalDateTime=DateTime.Parse("2017-11-19 1:00 AM"), DepartureDateTime=DateTime.Parse("2017-11-19 3:00 AM"),Fare=500},

            new Flight{Origin="Singapore",Destination="Kuala Lumpur",ArrivalDateTime=DateTime.Parse("2017-11-29 3:00 PM"), DepartureDateTime=DateTime.Parse("2017-11-29 4:00 PM"),Fare=300},
            new Flight{Origin="Singapore",Destination="Kuala Lumpur",ArrivalDateTime=DateTime.Parse("2017-11-29 8:00 PM"), DepartureDateTime=DateTime.Parse("2017-11-29 10:00 PM"),Fare=300},

            };
            foreach (Flight f in flights)
            {
                context.Flights.Add(f);
            }
            context.SaveChanges();
        }
    }

}
