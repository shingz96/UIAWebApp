using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UIAWebApp.Models
{
    public class Flight
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Flight Id")]
        public int FlightId { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd h:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Departure Date Time")]
        public DateTime DepartureDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd h:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Arrival Date Time")]
        public DateTime ArrivalDateTime { get; set; }
        public decimal Fare { get; set; }
    }
}
