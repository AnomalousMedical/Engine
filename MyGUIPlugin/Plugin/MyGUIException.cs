using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    class MyGUIException : Exception
    {
        public MyGUIException(String message)
            :base(message)
        {

        }
    }
}
