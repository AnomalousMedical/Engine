using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    class SaveException : Exception
    {
        public SaveException(String message)
            : base(message)
        {

        }
    }
}
