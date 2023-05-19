using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Domain.Entities
{
    public class Journey
    {
        public Journey()
        {
            Id = Guid.NewGuid();
            Flights = new HashSet<Flight>();
        }

        public Guid Id { get; init; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Price { get; set; }

        public virtual ICollection<Flight> Flights { get; }
    }
}
