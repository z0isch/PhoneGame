using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneGameService.Logging
{
    public class PhoneGameClientException : Exception
    {
        public PhoneGameClientException(string message)
            : base(message)
        {
        }

        public PhoneGameClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
