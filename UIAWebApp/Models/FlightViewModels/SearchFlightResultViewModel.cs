using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIAWebApp.Models.FlightViewModels
{
    public class SearchFlightResultViewModel
    {
        public int Passenger { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public bool RoundTrip { get; set; }

        [DataType(DataType.Date)]
        public DateTime DepartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        public IEnumerable<Flight> Depart { get; set; }

        public IEnumerable<Flight> Return { get; set; }
    }
}
