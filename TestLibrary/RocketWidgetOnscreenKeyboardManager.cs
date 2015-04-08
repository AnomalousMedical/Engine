using Anomalous.libRocketWidget;
using Anomalous.OSPlatform;
using libRocketPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Minimus
{
    class RocketWidgetOnscreenKeyboardManager
    {
        private RocketWidget currentRocketWidget;
        private TouchMouseGuiForwarder guiForwarder;

        public RocketWidgetOnscreenKeyboardManager(TouchMouseGuiForwarder guiForwarder)
        {
            this.guiForwarder = guiForwarder;

            RocketWidget.ElementFocused += HandleElementFocused;
            RocketWidget.ElementBlurred += HandleElementBlurred;
            RocketWidget.RocketWidgetDisposing += HandleRocketWidgetDisposing;
        }

        void HandleElementFocused(RocketWidget rocketWidget, Element element)
        {
            if (element != null)
            {
                currentRocketWidget = rocketWidget;
                String tag = element.TagName;
                switch (tag)
                {
                    case "input":
                        String type = element.GetAttributeString("type");
                        switch (type)
                        {
                            case "password":
                                guiForwarder.KeyboardMode = OnscreenKeyboardMode.Secure;
                                break;
                            case "text":
                                guiForwarder.KeyboardMode = OnscreenKeyboardMode.Normal;
                                break;
                            default:
                                guiForwarder.KeyboardMode = OnscreenKeyboardMode.Hidden;
                                break;
                        }
                        break;
                    case "textarea":
                        guiForwarder.KeyboardMode = OnscreenKeyboardMode.Normal;
                        break;
                    default:
                        guiForwarder.KeyboardMode = OnscreenKeyboardMode.Hidden;
                        break;
                }
            }
        }

        void HandleElementBlurred(RocketWidget widget, Element element)
        {
            if (widget == currentRocketWidget)
            {
                guiForwarder.KeyboardMode = OnscreenKeyboardMode.Hidden;
            }
        }

        void HandleRocketWidgetDisposing(RocketWidget widget)
        {
            if (widget == currentRocketWidget)
            {
                currentRocketWidget = null;
                guiForwarder.KeyboardMode = OnscreenKeyboardMode.Hidden;
                //Handle these for keyboard toggle right away or it won't work
                guiForwarder.toggleKeyboard();
            }
        }
    }
}
