using NewShore.Flights.Application.Contracts;
using NewShore.Flights.Domain.DTO.Request;
using NewShore.Flights.Domain.DTO.Response;
using NewShore.Flights.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Application.Services
{
    public class JourneyHelper : IJourneyHelper
    {
        private string _origin;
        private string _destination;
        private List<DTOFlightResponse> _finalFlights = new List<DTOFlightResponse>();
        private IEnumerable<DTOFlightResponse> _allFlights;
        private List<string> _allFlightsNumbers = new List<string>();

        public DTOJourneyResponse ResolveJourney(DTOJourneyRequest dtoJourneyRequest, IEnumerable<DTOFlightResponse> allFlights)
        {
            _origin = dtoJourneyRequest.Origin;
            _destination= dtoJourneyRequest.Destination;
            _allFlights = allFlights.OrderBy(f => f.Transport.FlightNumber);

            var dtoJourney = new DTOJourneyResponse
            {
                Origin = _origin,
                Destination = _destination
            };

            if (FindDirectRoute(_origin, _destination, _allFlights))
            {
                dtoJourney.Flights = _finalFlights;
                dtoJourney.Price = _finalFlights.First().Price;

                return dtoJourney;
            }

            var flightsByOrigin = _allFlights.Where(f => f.Origin == _origin);
            if (!flightsByOrigin.Any())
            {
                dtoJourney.Flights = _finalFlights;
                dtoJourney.Price = 0;

                return dtoJourney;
            }

            var journey = FindRoutesByOrigin(flightsByOrigin);

            return journey;
        }

        private DTOJourneyResponse FindRoutesByOrigin(IEnumerable<DTOFlightResponse> flightsByOrigin)
        {
            _allFlightsNumbers = new List<string>();

            var journey = new DTOJourneyResponse
            {
                Origin = _origin,
                Destination = _destination,
                Flights = _finalFlights
            };

            foreach (var fl in flightsByOrigin)
            {
                _finalFlights = new List<DTOFlightResponse>();

                if (FindRoutes(_origin, _destination, _allFlights)) 
                {
                    journey.Flights = _finalFlights;
                    journey.Price = _finalFlights.Sum(f => f.Price);
                    break;
                }
            }

            return journey;
        }

        private bool FindRoutes(string departure, string arrival, IEnumerable<DTOFlightResponse> flights)
        {
            if (FindDirectRoute(departure, arrival, flights))
            {
                return true;
            }

            var firstFlight = flights.FirstOrDefault(f => f.Origin == departure && !_allFlightsNumbers.Contains(f.Transport.FlightNumber));

            if (firstFlight == null)
            {
                return false;
            }
            else if (Convert.ToInt32(firstFlight.Transport.FlightNumber) < Convert.ToInt32(_allFlightsNumbers.Max()))
            {
                return false;
            }
           
            int.TryParse(firstFlight.Transport.FlightNumber, out int firstFlightNumber);

            flights = flights.Where(f => int.Parse(f.Transport.FlightNumber) >= firstFlightNumber && f.Destination != departure
                && !_allFlightsNumbers.Contains(f.Transport.FlightNumber));

            foreach (var fl in flights)
            {
                var nextDeparture = string.Empty;
                var nextArrival = string.Empty;

                if (fl.Origin == departure)
                {
                    _finalFlights.Add(fl);
                    _allFlightsNumbers.Add(fl.Transport.FlightNumber);

                    if (fl.Destination == arrival)
                    {
                        return true;
                    }
                    else
                    {
                        nextDeparture = fl.Destination;
                        nextArrival = arrival;
                    }
                }
                else
                {
                    continue;
                }

                if(FindRoutes(nextDeparture, nextArrival, flights)) 
                {
                    return true;
                }

                _finalFlights.Remove(fl);
            }

            return false;
        } 

        private bool FindDirectRoute(string departure, string arrival, IEnumerable<DTOFlightResponse> flights)
        {
            double price = 0;

            var directFlight = flights.Where(f => f.Origin == departure && f.Destination == arrival &&
                !_allFlightsNumbers.Contains(f.Transport.FlightNumber)).OrderBy(f => f.Price);

            if (directFlight.Any())
            {
                price += directFlight.First().Price;
                _finalFlights.Add(directFlight.First());
                _allFlightsNumbers.Add(directFlight.First().Transport.FlightNumber);
                return true;
            }

            return false;
        }
    }
}
