using NewShore.Flights.Domain.DTO.Request;
using NewShore.Flights.Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Application.Contracts
{
    public interface IJourneyApplicationService
    { 
        Task<DTOJourneyResponse> GetJourney(DTOJourneyRequest dtoJourneyRequest);
    }
}
