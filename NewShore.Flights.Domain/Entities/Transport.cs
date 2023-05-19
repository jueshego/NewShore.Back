using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Domain.Entities
{
    public class Transport
    {
        public Transport()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; init; }

        public string FlightCarrier { get; set; }

        public string FlightNumber { get; set; }
    }
}
