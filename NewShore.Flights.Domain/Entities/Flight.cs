using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Domain.Entities
{
    public class Flight
    {
        public Flight()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; init; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Price { get; set; }

        public Guid TransportId { get; init; }

        public Guid JourneyId { get; init; }

        public virtual Transport Transport { get; set; }

        public virtual Journey Journey { get; set; }
    }
}
