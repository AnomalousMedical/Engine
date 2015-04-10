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
        private RocketWidget rocketWidget;

        public ElementFocusListener(RocketWidget rocketWidget)
        {
            this.rocketWidget = rocketWidget;
        }

        public override void ProcessEvent(Event evt)
        {
            if (RocketWidgetOnscreenKeyboardManager.GetKeyboardModeForElement(evt.TargetElement) != Engine.Platform.OnscreenKeyboardMode.Hidden)
            {
                RocketWidget.fireElementFocused(rocketWidget, evt.TargetElement);
            }
        }
    }
}
