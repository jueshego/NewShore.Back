﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Domain.DTO.Response
{
    public class DTOFlightResponse
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Price { get; set; }

        public DTOTransportResponse Transport { get; set; }
    }
}
