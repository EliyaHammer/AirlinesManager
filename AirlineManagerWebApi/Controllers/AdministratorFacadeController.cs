using AirLinesManager;
using AirLinesManager.Facades;
using AirLinesManager.Facades.MSSQL;
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
    public class AdministratorFacadeController : ApiController
    {
        private ILoggedInAdministratorFacade facade = new LoggedInAdministratorFacadeMSSQL();

        //POST api/administratorfacade/airline/create
        //create new airline
        [Route("api/administatorfacade/airline/create")]
        [HttpPost]
        public IHttpActionResult CreateAirline ([FromBody] string name, string username, string password, long countryCode)
        {
            //from body -> the object itself, or only the properties?
            //here also checkups like in facade? 
            // or here only try catch for the checkups ?
            // return the new airline ?
            try
            {
                facade.CreateNewAirline(new AirlineCompany(name, username, password, countryCode));
                return Ok();
            }

            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
