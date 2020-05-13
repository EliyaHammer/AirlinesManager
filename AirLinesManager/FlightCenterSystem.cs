using AirLinesManager.Facades.MSSQL;
using AirLinesManager.LoginService;
using AirLinesManager.POCO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class FlightCenterSystem
    {
        private static FlightCenterSystem _instance;//SINGELTON
        private static object _key = new object();//SINGELTON
        FlightCenterSystemFanctionallityFacadeMSSQL _functionallityFacade;
        private LoginServiceBase _loginService;
        private TimeSpan _dailyWakeUpTime = MyConfig._dailyWakeUpTime;
        private int _counter;
        private FlightCenterSystem()
        {
            _loginService = new LoginServiceMSSQL();
            _functionallityFacade = new FlightCenterSystemFanctionallityFacadeMSSQL();

            Task.Run(() =>
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 1;
                timer.Elapsed += new System.Timers.ElapsedEventHandler((o, e) => { _counter++; });

                while (true)
                {
                    if (DateTime.Now.TimeOfDay == _dailyWakeUpTime)
                    {
                        timer.Start();
                        _functionallityFacade.MoveFlightAndTicketsToHistory();
                        timer.Stop();
                        Thread.Sleep(MyConfig.CLEANING_TIME_GAP - _counter);
                        _counter = 0;
                    }

                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            });
        }//SINGELTON
        public static FlightCenterSystem GetInstance ()
        {
            lock (_key)
            { 

            if (_instance is null)
                _instance = new FlightCenterSystem();

            }

            return _instance;
        }//SINGELTON
        public bool Login(string username, string password, out FacadeBase facade, out ILoginToken loginToken)
        {
            bool result = false;
            loginToken = null;
            facade = null;

            try
            {
                result = _loginService.TryLogin(username, password, out facade, out loginToken);
            }

            catch (WrongPasswordException ex)
            {
                // TODO
                // write later into log file - log4net
                result = false;
            }


            return result;
        }//RETURNS USER AND ITS FACADE
    }
}
