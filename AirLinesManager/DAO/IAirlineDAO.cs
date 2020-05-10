using AirLinesManager.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public interface IAirlineDAO : IBasicDAO<AirlineCompany>, IDAOTest
    {
        IList<AirlineCompany> GetAirlinesByCountry(long countryID);
        AirlineCompany GetAirlineByName(string name);
        AirlineCompany GetAirlineByUsername(string username);
        bool CheckUsernameExistance(string username, long id);
        bool CheckNameExistance(string name, long id);
        void RemoveAllReplica();

    }
}
