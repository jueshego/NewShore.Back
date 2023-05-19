using NewShore.Flights.Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Domain.DTO.Request
{
    public  class DTOJourneyRequest
    {
        public string Origin { get; set; }

        public string Destination { get; set; }
    }
}
