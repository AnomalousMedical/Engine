using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class EditException : Exception
    {
        public EditException(String message)
            :base(message)
        {

        }
    }
}
