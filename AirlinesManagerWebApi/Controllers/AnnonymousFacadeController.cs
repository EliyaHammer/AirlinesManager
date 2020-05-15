using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Description;
using AirLinesManager;
using AirlinesManagerWebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirlinesManagerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnonymousFacadeController : ControllerBase
    {
        private IAnnonymousUserFacade facade = new AnnonymousUserFacadeMSSQL();

        //GET api/annonymousfacade/companies
        //get all airline companies
        [ResponseType(typeof(List<AirlineCompany>))]
        [Route("api/annonymousfacade/getcompanies", Name = "GetAllCompanies")]
        [HttpGet]
        public ActionResult GetCompanies()
        {
            try
            {
                List<AirlineCompany> companies = (List<AirlineCompany>)facade.GetAllAirlineCompanies();
                if (companies.Count == 0)
                    return NotFound();
                else
                    return Ok(companies);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }
        } //CHECKED

        //GET api/annonymousfacade/flights
        //get all flights
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/getflights", Name = "GetAllFlights")]
        [HttpGet]
        public ActionResult GetFlights()
        {
            try
            {
                List<Flight> flights = (List<Flight>)facade.GetAllFlights();
                if (flights.Count == 0)
                    return NotFound();
                else
                    return Ok(flights);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }
        } //CHECKED

        //GET api/annonymousfacade/flights/id
        //get all flights
        [ResponseType(typeof(Flight))]
        [Route("api/annonymousfacade/flights/byid/{id}", Name = "GetFlightByID")]
        [HttpGet]
        public ActionResult GetFlightByID(long id)
        {
            try
            {
                Flight flight = facade.GetFlightByID(id);
                if (flight == null)
                    return NotFound();
                else
                    return Ok(flight);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }
        }//CHECKED

        //GET api/annonymousfacade/flightsvacancy
        //get all flights vacancy
        [ResponseType(typeof(Dictionary<Flight, int>))]
        [Route("api/annonymousfacade/flightsvacancy", Name = "GetAllFlightsVacancy")]
        [HttpGet]
        public ActionResult GetFlightsVacancy()
        {
            try
            {
                Dictionary<Flight, int> flightsVacancy = (Dictionary<Flight, int>)facade.GetAllFlightsVacancy();
                if (flightsVacancy.Count == 0)
                    return NotFound();
                else
                    return Ok(flightsVacancy);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }
        }//CHECKED

        //GET api/annonymousfacade/flights/byairline/id
        //get flights by airline id
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/byairline/{id}", Name = "GetFlightsByAirlineID")]
        [HttpGet]
        public ActionResult GetFlightsByAirlineID(long id)
        {
            try
            {
                List<Flight> flights = (List<Flight>)facade.GetFlightsByAirline(id);
                if (flights.Count == 0)
                    return NotFound();
                else
                    return Ok(flights);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }
        }//CHECKED

        //GET api/annonymousfacade/flights/bydestcountry/id
        //get flights by destination country id
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/bydestcountry/{id}", Name = "GetFlightsByDestinationCountryID")]
        [HttpGet]
        public ActionResult GetFlightsByDestinationCountryID(long id)
        {
            try
            {
                List<Flight> flights = (List<Flight>)facade.GetFlightsByDestinationCountry(id);
                if (flights.Count == 0)
                {
                    return NotFound();
                }

                else
                    return Ok(flights);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }
        }//CHECKED

        //GET api/annonymousfacade/flights/byorigcountry/id
        //get flights by origin country id
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/byorigcountry/{id}", Name = "GetFlightsByOriginCountryID")]
        [HttpGet]
        public ActionResult GetFlightsByOriginCountryID(long id)
        {
            try
            {
                List<Flight> flights = (List<Flight>)facade.GetFlightsByOriginCountry(id);
                if (flights.Count == 0)
                    return NoContent();
                else
                    return Ok(flights);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }
        }//CHECKED

        //POST api/annonymousfacade/flights/bydepartdateandtime
        //get flights by departure date and time
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/bydepartdateandtime", Name = "GetFlightsByDepartDateAndTime")]
        [HttpPost]
        public ActionResult GetFlightsByDepartureDateAndTime([FromBody] MDate dateAndTime)
        {
            try
            {
                DateTime dateTime = new DateTime(Convert.ToInt32(dateAndTime.mDate.Substring(0, 4)), Convert.ToInt32(dateAndTime.mDate.Substring(5, 2)), Convert.ToInt32(dateAndTime.mDate.Substring(8, 2)),
                    Convert.ToInt32(dateAndTime.mTime.Substring(0, 2)), Convert.ToInt32(dateAndTime.mTime.Substring(3, 2)), Convert.ToInt32(dateAndTime.mTime.Substring(6, 2)));

                List<Flight> result = (List<Flight>)facade.GetFlightsByDepartureDateAndTime(dateTime);

                if (result == null)
                    return NoContent();
                else
                    return Ok(result);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }

        }//CHECKED

        //POST api/annonymousfacace/flights/bydepartdate
        //get flights by departure date
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/bydepartdate", Name = "GetFlightsByDepartDate")]
        [HttpPost]
        public ActionResult GetFlightsByDepartureDate([FromBody] MDate dateAndTime)
        {
            try
            {
                DateTime dateTime = new DateTime(Convert.ToInt32(dateAndTime.mDate.Substring(0, 4)), Convert.ToInt32(dateAndTime.mDate.Substring(5, 2)), Convert.ToInt32(dateAndTime.mDate.Substring(8, 2)));

                List<Flight> result = (List<Flight>)facade.GetFlightsByDepartureDate(dateTime);

                if (result == null)
                    return NoContent();
                else
                    return Ok(result);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }

        }//CHECKED

        //POST api/annonymousfacace/flights/bylanddateandtime
        // get flight by landing date and time
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/bylanddateandtime", Name = "GetFlightsByLandDateAndTime")]
        [HttpPost]
        public ActionResult GetFlightsByLandingDateAndTime([FromBody] MDate dateAndTime)
        {
            try
            {
                DateTime dateTime = new DateTime(Convert.ToInt32(dateAndTime.mDate.Substring(0, 4)), Convert.ToInt32(dateAndTime.mDate.Substring(5, 2)), Convert.ToInt32(dateAndTime.mDate.Substring(8, 2)),
                    Convert.ToInt32(dateAndTime.mTime.Substring(0, 2)), Convert.ToInt32(dateAndTime.mTime.Substring(3, 2)), Convert.ToInt32(dateAndTime.mTime.Substring(6, 2)));

                List<Flight> result = (List<Flight>)facade.GetFlightsByLandingDateAndTime(dateTime);

                if (result == null)
                    return NoContent();
                else
                    return Ok(result);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }

        }//CHECKED

        //POST api/annonymousfacace/flights/bylanddate
        // get flights by landing date
        [ResponseType(typeof(List<Flight>))]
        [Route("api/annonymousfacade/flights/bylanddate", Name = "GetFlightsByLandDate")]
        [HttpPost]
        public ActionResult GetFlightsByLadningDate([FromBody] MDate dateAndTime)
        {
            try
            {
                DateTime dateTime = new DateTime(Convert.ToInt32(dateAndTime.mDate.Substring(0, 4)), Convert.ToInt32(dateAndTime.mDate.Substring(5, 2)), Convert.ToInt32(dateAndTime.mDate.Substring(8, 2)));

                List<Flight> result = (List<Flight>)facade.GetFlightsByLandingDate(dateTime);

                if (result == null)
                    return NoContent();
                else
                    return Ok(result);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return Problem("Unexpected error. Please contact support.");
            }

        }//CHECKED
    }
}