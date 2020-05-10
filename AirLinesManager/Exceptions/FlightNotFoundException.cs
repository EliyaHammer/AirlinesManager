using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class FlightNotFoundException : ApplicationException
    {
        public FlightNotFoundException()
        {
        }

        public FlightNotFoundException(string message) : base(message)
        {
        }

        public FlightNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FlightNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}