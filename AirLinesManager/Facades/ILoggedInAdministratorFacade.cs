using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager.Facades
{
    interface ILoggedInAdministratorFacade
    {
        AirlineCompany CreateNewAirline(AirlineCompany toAdd);
        Customer CreateNewCustomer(Customer toAdd);
        Country CreateNewCountry(Country toAdd);
        void RemoveAirline(AirlineCompany toRemove);
        void RemoveCustomer(Customer toRemove);
        void RemoveCountry(Country toRemove);
        void UpdateAirline(AirlineCompany toUpdate);
        void UpdateCustomer(Customer toUpdate);
        void UpdateCountry(Country toUpdate);


    }
}
