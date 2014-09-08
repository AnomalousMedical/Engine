using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    class GuiGestures : Gesture
    {
        private bool didGesture;

        public GuiGestures(Object eventLayerKey)
            :base(eventLayerKey)
        {

        }

        protected override bool processFingers(EventLayer eventLayer, Touches touches)
        {
            var fingers = touches.Fingers;
            if (Gui.Instance.HandledMouseButtons)
            {
                didGesture = true;
                if (fingers.Count == 1)
                {
                    InputManager.Instance.injectScrollGesture(fingers[0].PixelX, fingers[0].PixelY, fingers[0].PixelDeltaX, fingers[0].PixelDeltaY);
                }
            }
            return didGesture;
        }

        protected override void additionalProcessing(EventLayer eventLayer, Clock clock)
        {
            didGesture = false;
        }
    }
}
