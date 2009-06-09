using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    class BehaviorException : Exception
    {
        public BehaviorException(String message)
            :base(message)
        {

        }
    }
}
