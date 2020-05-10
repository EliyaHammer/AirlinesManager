
using AirLinesManager.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public interface ICustomerDAO : IBasicDAO<Customer>
    {
        Customer GetCustomerByUsername(string username);
        List<Customer> GetCustomerByFullName(string firstName, string lastName);
        bool CheckUsernameExistance(string username, long id);
    }
}
