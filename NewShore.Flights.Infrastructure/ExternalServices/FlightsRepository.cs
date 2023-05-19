using AutoMapper;
using Microsoft.Extensions.Configuration;
using NewShore.Flights.Domain.Contracts.Repositories;
using NewShore.Flights.Domain.DTO.Response;
using NewShore.Flights.Domain.Entities;
using NewShore.Flights.Infrastructure.ExternalServices.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NewShore.Flights.Infrastructure.ExternalServices
{
    public class FlightsRepository : IGenericRepository<DTOFlightResponse>
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;
        private readonly Serilog.ILogger _logger;

        public FlightsRepository(HttpClient httpClient, Serilog.ILogger logger)
        {
            _httpClient = httpClient;

            var builder = new ConfigurationBuilder().
                                SetBasePath(Directory.GetCurrentDirectory()).
                                AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();

            string _baseUrl = config.GetValue<string>("FlightsEndpoint:baseUrl");
            _logger = logger;
        }

        public async Task<List<DTOFlightResponse>> GetAll()
        {
            List<DTOFlightResponse> flights= new List<DTOFlightResponse>();

            try
            {
                _logger.Information($"Obteniendo del endpoint: {_httpClient.BaseAddress} en FlightsRepository.GetAll()");

                var res = await _httpClient.GetFromJsonAsync<List<DTOFlightAPI>>(_httpClient.BaseAddress);

                foreach (var item in res)
                {
                    flights.Add(new DTOFlightResponse
                    {
                        Origin = item.DepartureStation,
                        Destination = item.ArrivalStation,
                        Price = item.Price,
                        Transport = new DTOTransportResponse
                        {
                            FlightCarrier = item.FlightCarrier,
                            FlightNumber = item.FlightNumber
                        }
                    });
                }
            }
            catch
            {
                _logger.Warning($"Error al obtener informacion del endpoint: {_httpClient.BaseAddress} en FlightsRepository.GetAll()");
                throw;
            }

            _logger.Information("Datos obtenidos del endpont de vuelos");

            return flights;
        }
    }
}
