using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    public interface Triggerable
    {
        event Action<Triggerable> Triggered;
    }
}
