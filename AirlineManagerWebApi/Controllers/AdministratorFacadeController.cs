﻿using AirLinesManager;
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
    //AUTHORIZATION FOR ADMINISTRATOR ROLE . NO NEED FOR TOKEN DETAILS .
    [Route("api/[controller]")]
    public class AdministratorFacadeController : ApiController
    {
        private ILoggedInAdministratorFacade facade = new LoggedInAdministratorFacadeMSSQL();

        //GET api/administratorfacade/companies/byid/{id}
        //get airline company by id
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/administratorfacade/companies/byid/{id}", Name = "GetCompanyByID")]
        [HttpGet]
        public IHttpActionResult GetCompanyByID(long id)
        {
            try
            {
                AirlineCompany company = facade.GetCompanyByID(id);
                if (company == null)
                    return NotFound();
                else
                    return Ok(company);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }
        }//CHECKED


        //GET api/administratorfacade/customers/byid/{id}
        //get customer by id
        [ResponseType(typeof(Customer))]
        [Route("api/administratorfacade/customers/byid/{id}", Name = "GetCustomerByID")]
        [HttpGet]
        public IHttpActionResult GetCustomerByID(long id)
        {
            try
            {
                Customer customer = facade.GetCustomerByID(id);
                if (customer == null)
                    return NotFound();
                else
                    return Ok(customer);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }
        }//CHECKED


        //GET api/administratorfacade/countries/byid/{id}
        //get country by id
        [ResponseType(typeof(AirlineCompany))]
        [Route("api/administratorfacade/countries/byid/{id}", Name = "GetCountryByID")]
        [HttpGet]
        public IHttpActionResult GetCountryByID(long id)
        {
            try
            {
                Country country = facade.GetCountryByID(id);
                if (country == null)
                    return NotFound();
                else
                    return Ok(country);
            }

            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }
        }//CHECKED


        //POST api/administratorfacade/companies/create
        //create new airline
        [Route("api/administatorfacade/companies/create")]
        [HttpPost]
        public IHttpActionResult CreateAirline([FromBody] AirlineCompany airline)
        {
            try
            {
                AirlineCompany createdAirline = facade.CreateNewAirline(airline);
                return CreatedAtRoute("GetCompanyByID", new { id = createdAirline.ID }, createdAirline);
            }
            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }
        }//CHECKED


        //POST api/administratorfacade/countries/create
        //create new country
        [Route("api/administatorfacade/countries/create")]
        [HttpPost]
        public IHttpActionResult CreateCountry([FromBody] Country country)
        {
            try
            {
                Country createdCountry = facade.CreateNewCountry(country);
                return CreatedAtRoute("GetCountryByID", new { id = createdCountry.ID }, createdCountry);
            }
            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }
        }//CHECKED


        //POST api/administratorfacade/customers/create
        //create new customer
        [Route("api/administatorfacade/customers/create")]
        [HttpPost]
        public IHttpActionResult CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                Customer createdCustomer = facade.CreateNewCustomer(customer);
                return CreatedAtRoute("GetCustomerByID", new { id = createdCustomer.ID }, createdCustomer);
            }
            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }
        }//CHECKED


        //POST api/administratorfacade/companies/remove
        //remove airline company
        [Route("api/administatorfacade/companies/remove")]
        [HttpPost]
        public IHttpActionResult RemoveAirline([FromBody] AirlineCompany company)
        {
            try
            {
                facade.RemoveAirline(company);
                return Ok();
            }
            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }

        }//CHECKED


        //POST api/administratorfacade/countries/remove
        //remove country
        [Route("api/administatorfacade/countries/remove")]
        [HttpPost]
        public IHttpActionResult RemoveCountry([FromBody] Country country)
        {
            try
            {
                facade.RemoveCountry(country);
                return Ok();
            }
            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }

        }//CHECKED


        //POST api/administratorfacade/customers/remove
        //remove customer
        [Route("api/administatorfacade/customers/remove")]
        [HttpPost]
        public IHttpActionResult RemoveCustomer([FromBody] Customer customer)
        {
            try
            {
                facade.RemoveCustomer(customer);
                return Ok();
            }
            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }

        }//CHECKED


        //PUT api/administratorfacade/companies/update
        //update airline company
        [Route("api/administratorfacade/companies/update")]
        [HttpPut]
        public IHttpActionResult UpdateCompany ([FromBody] AirlineCompany company)
        {
            try
            {
                facade.UpdateAirline(company);
                return Ok();
            }
            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }
        }//CHECKED


        //PUT api/administratorfacade/countries/update
        //update country
        [Route("api/administratorfacade/countries/update")]
        [HttpPut]
        public IHttpActionResult UpdateCountry([FromBody] Country country)
        {
            try
            {
                facade.UpdateCountry(country);
                return Ok();
            }
            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }
        }//CHECKED


        //PUT api/administratorfacade/customers/update
        //update customer
        [Route("api/administratorfacade/customers/update")]
        [HttpPut]
        public IHttpActionResult UpdateCustomer([FromBody] Customer customer)
        {
            try
            {
                facade.UpdateCustomer(customer);
                return Ok();
            }
            catch (AirlinesManagerException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (SystemException ex)
            {
                return InternalServerError(new Exception("Unexpected error. Please contact support."));
            }
        }//CHECKED
    }
}
