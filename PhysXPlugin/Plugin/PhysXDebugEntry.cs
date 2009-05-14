using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;

namespace PhysXPlugin
{
    class PhysXDebugEntry : DebugEntry
    {
        private String text;
        private PhysParameter parameter;

        public PhysXDebugEntry(String text, PhysParameter parameter)
        {
            this.text = text;
            this.parameter = parameter;
        }

        public void setEnabled(bool enabled)
        {
            PhysSDK.Instance.setParameter(parameter, enabled ? 1.0f : 0.0f);
        }

        public string Text
        {
            get
            {
                return text;
            }
        }

        public override string ToString()
        {
            return text;
        }
    }
}
