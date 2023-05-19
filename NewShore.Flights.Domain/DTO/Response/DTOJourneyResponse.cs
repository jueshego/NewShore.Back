using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Domain.DTO.Response
{
    public class DTOJourneyResponse
    {
        public DTOJourneyResponse()
        {
            Price = 0;
        }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Price { get; set; }

        public List<DTOFlightResponse> Flights { get; set; }

        public string Message { get; set; }
    }
}
