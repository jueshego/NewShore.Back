using AutoMapper;
using NewShore.Flights.Application.Contracts;
using NewShore.Flights.Domain.DTO.Request;
using NewShore.Flights.Domain.Contracts.Repositories;
using NewShore.Flights.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewShore.Flights.Domain.DTO.Response;

namespace NewShore.Flights.Application.Services
{
    public class JourneyApplicationService : IJourneyApplicationService
    {
        private readonly IGenericRepository<DTOFlightResponse> _flightsRepository;
        private readonly IJourneyHelper _journeyHelper;

        public JourneyApplicationService(IGenericRepository<DTOFlightResponse> flightsRepository, IJourneyHelper journeyHelper)
        {
            _flightsRepository = flightsRepository;
            _journeyHelper = journeyHelper;
        }

        public async Task<DTOJourneyResponse> GetJourney(DTOJourneyRequest dtoJourneyRequest)
        {
            var res = await _flightsRepository.GetAll();

            var journey = _journeyHelper.ResolveJourney(dtoJourneyRequest, res);

            if(journey.Flights.Count == 0)
            {
                journey.Message = "La ruta no es posible.";
            }

            return journey;
        }
    }
}
