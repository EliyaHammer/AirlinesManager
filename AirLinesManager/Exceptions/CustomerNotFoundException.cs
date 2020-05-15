using System;
using System.Runtime.Serialization;

namespace AirLinesManager
{
    [Serializable]
    public class CustomerNotFoundException : AirlinesManagerException
    {
        public CustomerNotFoundException()
        {
        }

        public CustomerNotFoundException(string message) : base(message)
        {
        }

        public CustomerNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}