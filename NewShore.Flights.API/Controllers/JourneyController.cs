using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewShore.Flights.Application.Contracts;
using NewShore.Flights.Domain.DTO.Request;
using NewShore.Flights.Domain.DTO.Response;
using System.Net.Http;

namespace NewShore.Flights.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class JourneyController : ControllerBase
    {
        private readonly IJourneyApplicationService _journeyApplicationService;
        private readonly Serilog.ILogger _logger;

        public JourneyController(IJourneyApplicationService journeyApplicationService, Serilog.ILogger logger)
        {
            _journeyApplicationService = journeyApplicationService;
            _logger = logger;
        }

        [HttpGet("from/{origin}/to/{destination}")]
        public async Task<ActionResult<List<DTOJourneyResponse>>> Get(string origin, string destination)
        {
            _logger.Debug($"Entrando al API/Journeys GET con parametros: {origin} - {destination}");

            var journeys = await _journeyApplicationService.GetJourney(new DTOJourneyRequest
            {
                Origin= origin,
                Destination= destination
            });

            return Ok(journeys);
        }
    }
}
