using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class OgreException : Exception
    {
        public OgreException(String message)
            :base(message)
        {

        }
    }
}
