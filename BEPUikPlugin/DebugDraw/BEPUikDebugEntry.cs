using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUikDebugEntry : DebugEntry
    {
        String text;
        DebugDrawMode mode;
        bool enabled = false;
        BEPUikDebugInterface debugInterface;

        public BEPUikDebugEntry(String text, DebugDrawMode mode, BEPUikDebugInterface debugInterface)
        {
            this.text = text;
            this.mode = mode;
            this.debugInterface = debugInterface;
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    if (enabled)
                    {
                        debugInterface.enableGlobalDebugMode(mode);
                    }
                    else
                    {
                        debugInterface.disableGlobalDebugMode(mode);
                    }
                }
            }
        }

        public override String ToString()
        {
            return text;
        }

        public String Text
        {
            get
            {
                return text;
            }
        }
    }
}
