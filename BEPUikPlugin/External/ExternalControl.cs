using BEPUik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public abstract class ExternalControl
    {
        internal abstract Control IKControl { get; }
    }
}
