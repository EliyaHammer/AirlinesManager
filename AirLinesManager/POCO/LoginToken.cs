using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager
{
    public class LoginToken<T> : ILoginToken where T : IUser 
    {
        public T user { get; set; }

        public LoginToken () { }

    }
}
