using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medical.Controller
{
    public class MDIException : Exception
    {
        public MDIException(String message)
            :base(message)
        {
        }
    }
}
