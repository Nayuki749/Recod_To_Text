using System;
using System.Runtime.Serialization;

namespace Nayuki749.Speech_to_Text
{
    [Serializable()]
    public class subscriptionKeyException : Exception
    {

        public subscriptionKeyException()
    : base()
        {
        }

        public subscriptionKeyException(string message)
            : base(message)
        {
        }

        public subscriptionKeyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        protected subscriptionKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
