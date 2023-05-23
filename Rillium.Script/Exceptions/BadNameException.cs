using System.Runtime.Serialization;

namespace Rillium.Script.Exceptions
{
    public class BadNameException : ScriptException
    {
        public BadNameException() : base()
        {
        }

        public BadNameException(string message) : base(message)
        {
        }

        public BadNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            base.GetObjectData(info, context);
        }
    }
}
