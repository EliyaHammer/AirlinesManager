using AirLinesManager;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AirlineManagerWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AnnonymousFacadeController : ApiController
    {
        private IAnnonymousUserFacade facade = new AnnonymousUserFacadeMSSQL(); //static?

        //GET api/annonymousfacade/companies
        //get all airline companies
        [ResponseType(typeof(List<AirlineCompany>))]
        [Route("api/annonymousfacade/companies", Name = "GetAllCompanies")]
        [HttpGet]
        public IHttpActionResult GetCompanies ()
        {
            List<AirlineCompany> companies = (List<AirlineCompany>)facade.GetAllAirlineCompanies();
            if (companies.Count == 0)
                return NotFound();
            else
                return Ok(companies);
        }

        //GET api/annonymousfacade/flights
        //get all flights
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights", Name = "GetAllFlights")] // check problem
        [HttpGet]
        public IHttpActionResult GetFlights ()
        {
            List<Flight> flights = (List<Flight>)facade.GetAllFlights();
            if (flights.Count == 0)
                return NotFound();
            else
                return Ok(flights);
        }

        //GET api/annonymousfacade/flights/id
        //get all flights
        [ResponseType(typeof(Flight))]
        [Route("api/annonymousfacade/flights/{id}", Name = "GetFlightByID")] // check problem
        [HttpGet]
        public IHttpActionResult GetFlightByID([FromUri] long id)
        {
            Flight flight = facade.GetFlightByID(id);
            if (flight == null)
                return NotFound();
            else
                return Ok(flight);
        }

        //GET api/annonymousfacade/flightsvacancy
        //get all flights vacancy
        [ResponseType(typeof(Dictionary<Flight, int>))]
        [Route("api/annonymousfacade/flightsvacancy", Name = "GetAllFlightsVacancy")] // check problem
        [HttpGet]
        public IHttpActionResult GetFlightsVacancy()
        {
            Dictionary<Flight, int> flightsVacancy = (Dictionary<Flight, int>)facade.GetAllFlightsVacancy();
            if (flightsVacancy.Count == 0)
                return NotFound();
            else
                return Ok(flightsVacancy);
        }

        //GET api/annonymousfacade/flights/byairline/id
        //get flights by airline id
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/byairline/{id}", Name = "GetFlightsByAirlineID")] // check problem
        [HttpGet]
        public IHttpActionResult GetFlightsByAirlineID([FromUri] long id)
        {
            List<Flight> flights = (List<Flight>)facade.GetFlightsByAirline(id);
            if (flights.Count == 0)
                return NotFound();
            else
                return Ok(flights);
        }

        //GET api/annonymousfacade/flights/bydestcountry/id
        //get flights by destination country id
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/bydestcountry/{id}", Name = "GetFlightsByDestinationCountryID")] // check prob
        [HttpGet]
        public IHttpActionResult GetFlightsByDestinationCountryID([FromUri] long id)
        {
            List<Flight> flights = (List<Flight>)facade.GetFlightsByDestinationCountry(id);
            if (flights.Count == 0)
            {
                return NotFound();
            }

            else
                return Ok(flights);
        }

        //GET api/annonymousfacade/flights/byorigcountry/id
        //get flights by origin country id
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/byorigcountry/{id}", Name = "GetFlightsByOriginCountryID")] // check prob
        [HttpGet]
        public IHttpActionResult GetFlightsByOriginCountryID([FromUri] long id)
        {
            List<Flight> flights = (List<Flight>)facade.GetFlightsByOriginCountry(id);
            if (flights.Count == 0)
                return NotFound();
            else
                return Ok(flights);
        }
    }
} 
