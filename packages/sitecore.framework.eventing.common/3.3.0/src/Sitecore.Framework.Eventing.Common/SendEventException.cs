using System;
using System.Linq;

namespace Sitecore.Framework.Eventing
{
    public class SendEventException : AggregateException
    {   
        public SendEventException(string message, Exception innerException):base(message, innerException)
        {
            
        }

        public SendEventException(params Exception[] exceptions):base(exceptions)
        {
            
        }                
    }
}
