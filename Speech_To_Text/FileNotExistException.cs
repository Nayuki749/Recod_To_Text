using System;
using System.Runtime.Serialization;

namespace Nayuki749.Speech_to_Text
{
    [Serializable()]
    public class FileNotExistyException : Exception
    {

        public FileNotExistyException()
    : base()
        {
        }

        public FileNotExistyException(string message)
            : base(message)
        {
        }

        public FileNotExistyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        protected FileNotExistyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
