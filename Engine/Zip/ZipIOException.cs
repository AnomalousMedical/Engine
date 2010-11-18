using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZipAccess
{
    public class ZipIOException : Exception
    {
        public ZipIOException(String message, params object[] args)
            :base(String.Format(message, args))
        {

        }
    }
}
