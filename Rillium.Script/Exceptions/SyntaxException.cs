using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Rillium.Script.Exceptions
{

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class SyntaxException : ScriptException
    {
        public SyntaxException() : base()
        {
        }

        public SyntaxException(string message) : base(message)
        {
        }

        public SyntaxException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SyntaxException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            base.GetObjectData(info, context);
        }
    }
}
