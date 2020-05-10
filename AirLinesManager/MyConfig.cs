using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    static class MyConfig
    {
        // TODO:
        // place in config project maybe json? ... later ....


        public static string _connectionString = @"Data Source = DESKTOP-NTU5DNC\SQLEXPRESS; Initial Catalog = AirLinesManager; Integrated Security = true;";
        public static TimeSpan _dailyWakeUpTime = new TimeSpan(12, 0, 0);
        public const int CLEANING_TIME_GAP = 24 * 60 * 60 * 1000; 
    }
}
