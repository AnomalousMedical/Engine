using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class CallbackDebugEntry : DebugEntry
    {
        private Action<bool> enabledChanged;
        private bool enabled = false;

        public CallbackDebugEntry(String text, Action<bool> enabledChanged)
        {
            this.enabledChanged = enabledChanged;
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                this.enabled = value;
                enabledChanged(enabled);
            }
        }

        public string Text { get; private set; }
    }
}
