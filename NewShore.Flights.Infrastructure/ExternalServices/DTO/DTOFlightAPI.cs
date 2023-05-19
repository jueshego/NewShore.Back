using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Infrastructure.ExternalServices.DTO
{
    public class DTOFlightAPI
    {
        public string DepartureStation { get; set; }

        public string ArrivalStation { get; set; }

        public string FlightCarrier { get; set; }

        public string FlightNumber { get; set; }

        public double Price { get; set; }
    }
}
