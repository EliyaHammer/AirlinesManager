using AirLinesManager.DAO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public abstract class BasicDAO<T> where T : IPoco
    {
        public static string _connectionString = MyConfig._connectionString;
    }
}
