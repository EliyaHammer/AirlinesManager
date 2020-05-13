using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLinesManager.POCO
{
    public class Administrator : IUser, IPoco
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Administrator ()
        { }

        public Administrator (string username, string password)
        {
            if (username != null)
                Username = username;
            if (password != null)
                Password = password;
        }

        public static bool operator ==(Administrator x, Administrator y)
        {
            if (x is null && y is null)
                return true;
            if (x is null || y is null)
                return false;
            if (x.ID == y.ID)
                return true;
            else
                return false;
        }

        public static bool operator !=(Administrator x, Administrator y)
        {
            return !(x == y);
        }

        public override bool Equals(object obj)
        {

            if (obj is null)
                return false;
            if (obj as Administrator is null)
                return false;
            if (this.ID == (obj as Administrator).ID)
                return true;
            else
                return false;

        }

        public override int GetHashCode()
        {
            return (int)this.ID;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Username: {Username}";
        }
    }
}
