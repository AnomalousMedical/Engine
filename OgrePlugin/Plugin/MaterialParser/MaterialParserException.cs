using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    class MaterialParserException : Exception
    {
        public MaterialParserException(String message)
            :base(message)
        { 
        }

        public MaterialParserException(String format, params Object[] args)
            : base(String.Format(format, args))
        {
        }
    }
}
