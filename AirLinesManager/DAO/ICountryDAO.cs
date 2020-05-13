using AirLinesManager.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public interface ICountryDAO : IBasicDAO<Country>, IDAOTest 
    {
        Country GetCountryByName(string name);
        bool CheckNameExistance(string name, long id);
    }
}
