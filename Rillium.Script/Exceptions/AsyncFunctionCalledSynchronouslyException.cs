using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Rillium.Script.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class AsyncFunctionCalledSynchronouslyException : ScriptException
    {
        public AsyncFunctionCalledSynchronouslyException() : base()
        {
        }

        public AsyncFunctionCalledSynchronouslyException(string message) : base(message)
        {
        }

        public AsyncFunctionCalledSynchronouslyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AsyncFunctionCalledSynchronouslyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
