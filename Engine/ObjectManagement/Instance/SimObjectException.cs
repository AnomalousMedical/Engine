using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    class SimObjectException : Exception
    {
        public SimObjectException(String message)
            :base(message)
        {

        }
    }
}
