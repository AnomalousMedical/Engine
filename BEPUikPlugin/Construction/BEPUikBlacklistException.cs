using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUikBlacklistException : Exception
    {
        public BEPUikBlacklistException(String message)
            :base(message)
        {
            
        }

        public BEPUikBlacklistException(String message, params String[] args)
            : base(String.Format(message, args))
        {

        }
    }
}
