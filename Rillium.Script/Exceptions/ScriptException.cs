using System.Runtime.Serialization;

namespace Rillium.Script
{

    [Serializable]
    public class ScriptException : Exception, ISerializable
    {
        public ScriptException() : base()
        {
        }

        public ScriptException(string message) : base(message)
        {
        }

        public ScriptException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ScriptException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
