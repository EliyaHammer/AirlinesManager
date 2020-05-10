using AirLinesManager.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager.DAO.MSSQL
{
    public interface IAdministratorDAO : IBasicDAO<Administrator>
    {
        Administrator GetAdminByUsername(string username);
        bool CheckUsernameExistance(string username, long id);
    }
}
