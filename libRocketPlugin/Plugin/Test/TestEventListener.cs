using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libRocketPlugin
{
    class TestEventListener : EventListener
    {
        private string name;

        public TestEventListener(String name)
        {
            this.name = name;
        }

        public override void ProcessEvent(Event evt)
        {
            Logging.Log.Debug("{0} phase {1} | type {2}", name, evt.Phase, evt.Type);
        }
    }
}
