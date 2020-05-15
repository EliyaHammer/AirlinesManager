using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class TicketNotFoundException : AirlinesManagerException
    {
        public TicketNotFoundException()
        {
        }

        public TicketNotFoundException(string message) : base(message)
        {
        }

        public TicketNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TicketNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}