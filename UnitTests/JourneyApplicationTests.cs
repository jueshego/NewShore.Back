using Moq;
using NewShore.Flights.Application.Contracts;
using NewShore.Flights.Application.Services;
using NewShore.Flights.Domain.Contracts.Repositories;
using NewShore.Flights.Domain.DTO.Request;
using NewShore.Flights.Domain.DTO.Response;
using Newtonsoft.Json;

namespace UnitTests
{
    [TestFixture]
    public class JourneyApplicationTests
    {
        private readonly Mock<IGenericRepository<DTOFlightResponse>> _mockRepository;
        private readonly List<DTOFlightResponse> _apiFlights= new List<DTOFlightResponse>();
        private readonly JourneyHelper _journeyHelper;
        private readonly JourneyApplicationService _appService;

        public JourneyApplicationTests()
        {
            _mockRepository = new Mock<IGenericRepository<DTOFlightResponse>>(MockBehavior.Strict);

            string json =
                @"[{""origin"":""MZL"",""destination"":""MDE"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8001""}},{""origin"":""MZL"",""destination"":""CTG"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8002""}},{""origin"":""PEI"",""destination"":""BOG"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8003""}},{""origin"":""MDE"",""destination"":""BCN"",""price"":500.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8004""}},{""origin"":""CTG"",""destination"":""CAN"",""price"":300.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8005""}},{""origin"":""BOG"",""destination"":""MAD"",""price"":500.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8006""}},{""origin"":""BOG"",""destination"":""MEX"",""price"":300.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8007""}},{""origin"":""MZL"",""destination"":""PEI"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8008""}},{""origin"":""MDE"",""destination"":""CTG"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8009""}},{""origin"":""BOG"",""destination"":""CTG"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""8010""}},{""origin"":""MDE"",""destination"":""MZL"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""9001""}},{""origin"":""CTG"",""destination"":""MZL"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""9002""}},{""origin"":""BOG"",""destination"":""PEI"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""9003""}},{""origin"":""BCN"",""destination"":""MDE"",""price"":500.0,""transport"":{""flightCarrier"":""ES"",""FlightNumber"":""9004""}},{""origin"":""CAN"",""destination"":""CTG"",""price"":300.0,""transport"":{""flightCarrier"":""MX"",""FlightNumber"":""9005""}},{""origin"":""MAD"",""destination"":""BOG"",""price"":500.0,""transport"":{""flightCarrier"":""ES"",""FlightNumber"":""9006""}},{""origin"":""MEX"",""destination"":""BOG"",""price"":300.0,""transport"":{""flightCarrier"":""MX"",""FlightNumber"":""9007""}},{""origin"":""PEI"",""destination"":""MZL"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""9008""}},{""origin"":""CTG"",""destination"":""MDE"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""9009""}},{""origin"":""CTG"",""destination"":""BOG"",""price"":200.0,""transport"":{""flightCarrier"":""CO"",""FlightNumber"":""9010""}}]";

            _apiFlights.AddRange(JsonConvert.DeserializeObject<List<DTOFlightResponse>>(json));

            _mockRepository.Setup(r => r.GetAll()).Returns(Task.FromResult<List<DTOFlightResponse>>(_apiFlights));

            _journeyHelper = new JourneyHelper();

            _appService = new JourneyApplicationService(_mockRepository.Object, _journeyHelper);
        }

        [Test]
        public async Task WhenAJourneyBCN_CTGCannotBeResolved()
        {
            var dtoJourneyRequest = new DTOJourneyRequest
            {
                Origin = "BCN",
                Destination = "CTG"
            };

            var res = await _appService.GetJourney(dtoJourneyRequest);

            Assert.True(res.Flights.Count == 0);
        }

        [Test]
        public async Task WhenAJourneyMAD_BOGIsResolvedWithDirectFlight()
        {
            _mockRepository.Setup(r => r.GetAll()).Returns(Task.FromResult<List<DTOFlightResponse>>(_apiFlights));

            var dtoJourneyRequest = new DTOJourneyRequest
            {
                Origin = "MAD",
                Destination = "BOG"
            };

            var res = await _appService.GetJourney(dtoJourneyRequest);

            Assert.True(res.Flights.FirstOrDefault().Price == 500);
        }

        [Test]
        public async Task WhenAJourneyIsResolvedWithThreeFlights()
        {
            var dtoJourneyRequest = new DTOJourneyRequest
            {
                Origin = "MZL",
                Destination = "BOG"
            };

            var res = await _appService.GetJourney(dtoJourneyRequest);

            Assert.True(res.Price == 600);
        }
    }
}