using AirLinesManager.DAO.MSSQL;
using AirLinesManager.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager.Facades.MSSQL
{
    public class LoggedInAdministratorFacadeMSSQL : AnnonymousUserFacadeMSSQL, ILoggedInAdministratorFacade
    {
        public AirlineCompany CreateNewAirline(AirlineCompany toAdd)
        {
            if (toAdd is null)
                throw new NullReferenceException("Airline company is empty.");
            if (toAdd.AirLineName is null || toAdd.UserName is null || toAdd.Password is null || toAdd.CountryCode <= 0)
                throw new IllegalValueException($"One or more of the fields are not set: 'Username/Password/AirlineName/CountryCode'.");
            if (_airlineDAO.CheckUsernameExistance(toAdd.UserName, toAdd.ID))
                throw new AlreadyExistsException($"Username provided: '{toAdd.UserName}' already exists.");
            if (_airlineDAO.CheckNameExistance(toAdd.AirLineName, toAdd.ID))
                throw new AlreadyExistsException($"Name provided: '{toAdd.AirLineName}' already exists.");
            if (_countryDAO.Get(toAdd.CountryCode) is null)
                throw new CountryNotFoundException($"There is no country with the provided ID: '{toAdd.CountryCode}'.");

            return _airlineDAO.Add(toAdd);
        }

        public Country CreateNewCountry(Country toAdd)
        {
            if (toAdd is null)
                throw new NullReferenceException("Country provided is empty.");
            if (toAdd.CountryName is null)
                throw new IllegalValueException("Country name is not set.");
            if (_countryDAO.CheckNameExistance(toAdd.CountryName, toAdd.ID))
                throw new AlreadyExistsException($"Name provided: '{toAdd.CountryName}' already exists.");

            return _countryDAO.Add(toAdd);
        }

        public Customer CreateNewCustomer(Customer toAdd)
        {
            if (toAdd is null)
                throw new NullReferenceException("Customer provided is empty.");
            if (toAdd.Address is null || toAdd.CreditCardNumber is null || toAdd.FirstName is null || toAdd.LastName is null || toAdd.Password is null || toAdd.PhoneNumber is null || toAdd.UserName is null)
                throw new IllegalValueException("One of the fields or more are not filled.");
            if (_customerDAO.CheckUsernameExistance(toAdd.UserName, toAdd.ID))
                throw new AlreadyExistsException($"The username: '{toAdd.UserName}' already exists.");

            return _customerDAO.Add(toAdd);
        }

        public void RemoveAirline(AirlineCompany toRemove)
        {
            if (toRemove is null)
                throw new NullReferenceException("Airline provided is empty or does not exist.");
            if (toRemove.ID <= 0)
                throw new IllegalValueException($"Airline's ID: '{toRemove.ID}' is not valid, less or equal to zero.");
            if (_airlineDAO.Get(toRemove.ID) == null)
                throw new AirlineNotFoundException($"Airline of ID: '{toRemove.ID}' could not be found.");

            _airlineDAO.Remove(toRemove);
        }

        public void RemoveCountry(Country toRemove)
        {
            if (toRemove is null)
                throw new NullReferenceException("Country provided is empty.");
            if (toRemove.ID <= 0)
                throw new IllegalValueException($"Country's ID: '{toRemove.ID}' is not valid, less or equal to zero.");
            if (_countryDAO.Get(toRemove.ID) == null)
                throw new CountryNotFoundException($"Country of ID: '{toRemove.ID}' could not be found.");

            _countryDAO.Remove(toRemove);
        }

        public void RemoveCustomer(Customer toRemove)
        {
            if (toRemove is null)
                throw new NullReferenceException("Customer provided is empty.");
            if (toRemove.ID <= 0)
                throw new IllegalValueException($"Customer's ID: '{toRemove.ID}' is not valid; less or equal to zero.");
            if (_customerDAO.Get(toRemove.ID) == null)
                throw new CustomerNotFoundException($"The customer of ID: '{toRemove.ID}' could not be found.");

            _customerDAO.Remove(toRemove);
        }

        public void UpdateAirline(AirlineCompany toUpdate)
        {
            if (toUpdate is null)
                throw new NullReferenceException("Airline provided is empty or does not exist.");
            if (toUpdate.ID <= 0)
                throw new IllegalValueException($"Airline's ID: '{toUpdate.ID}' is not valid; equal or less than zero.");
            if (toUpdate.CountryCode <= 0)
                throw new IllegalValueException($"Country code provided: '{toUpdate.CountryCode}' is not valid; equal or less than zero.");
            if (_countryDAO.Get(toUpdate.CountryCode) is null)
                throw new CountryNotFoundException($"Country of ID: '{toUpdate.CountryCode}' could not be found.");
            if (toUpdate.AirLineName is null || toUpdate.Password is null || toUpdate.UserName is null)
                throw new NullReferenceException("One or more of the fields are not filled.");
            if (_airlineDAO.CheckNameExistance(toUpdate.AirLineName, toUpdate.ID))
                throw new AlreadyExistsException($"The name provided: '{toUpdate.AirLineName}' already exists.");
            if (_airlineDAO.CheckUsernameExistance(toUpdate.UserName, toUpdate.ID))
                throw new AlreadyExistsException($"The username provided '{toUpdate.UserName}' already exists.");
            if (_airlineDAO.Get(toUpdate.ID) is null)
                throw new AirlineNotFoundException($"Airline of ID: '{toUpdate.ID}' could not be found.");

            _airlineDAO.Update(toUpdate);
        }

        public void UpdateCountry(Country toUpdate)
        {
            if (toUpdate is null)
                throw new NullReferenceException("Country provided is empty.");
            if (toUpdate.ID <= 0)
                throw new IllegalValueException($"Country's ID: '{toUpdate.ID}' is not valid, less or equal to zero.");
            if (_countryDAO.Get(toUpdate.ID) == null)
                throw new CountryNotFoundException($"Country of ID: '{toUpdate.ID}' could not be found.");
            if (toUpdate.CountryName == null)
                throw new IllegalValueException("Country's name is empty.");
            if (_countryDAO.CheckNameExistance(toUpdate.CountryName, toUpdate.ID))
                throw new AlreadyExistsException($"Name provided: '{toUpdate.CountryName}' already exists.");

            _countryDAO.Update(toUpdate);
        }

        public void UpdateCustomer(Customer toUpdate)
        {
            if (toUpdate is null)
                throw new NullReferenceException("Customer provided is empty.");
            if (toUpdate.ID <= 0)
                throw new IllegalValueException($"Customer's ID: '{toUpdate.ID}' is not valid: equal or less than zero.");
            if (_customerDAO.Get(toUpdate.ID) == null)
                throw new CustomerNotFoundException($"Customer of ID: '{toUpdate.ID}' could not be found.");
            if (toUpdate.Address is null || toUpdate.CreditCardNumber is null || toUpdate.FirstName is null || toUpdate.LastName is null || toUpdate.Password is null || toUpdate.PhoneNumber is null || toUpdate.UserName is null)
                throw new IllegalValueException("Not all fields are filled.");
            if (_customerDAO.CheckUsernameExistance(toUpdate.UserName, toUpdate.ID))
                throw new AlreadyExistsException($"Username provided: '{toUpdate.UserName}' already exists.");

            _customerDAO.Update(toUpdate);
        }
    }
}
