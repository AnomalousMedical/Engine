using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public class InputException : Exception
    {
        public InputException(String message)
            : base(message)
        {

        }
    }
}
