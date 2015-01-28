using libRocketPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.libRocketWidget
{
    class ElementFocusListener : EventListener
    {
        public override void ProcessEvent(Event evt)
        {
            RocketWidget.fireElementFocused(evt.TargetElement);
        }
    }
}
