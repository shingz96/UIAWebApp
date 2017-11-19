using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UIAWebApp.Models
{
    public class FlightBooking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Booking Id")]
        public int FlightBookingId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Passenger Number")]
        public int Passenger { get; set; }

        public int DepartFlightId { get; set; }

        public Nullable<int> ReturnFlightId { get; set; }

        [Display(Name = "Depart Flight")]
        public Flight DepartFlight { get; set; }

        [Display(Name = "Return Flight")]
        public Flight ReturnFlight { get; set; }
    }
}
