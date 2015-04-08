using Anomalous.libRocketWidget;
using Engine.Platform;
using libRocketPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.libRocketWidget
{
    public class RocketWidgetOnscreenKeyboardManager
    {
        private RocketWidget currentRocketWidget;
        private OnscreenKeyboardManager onscreenKeyboardManager;

        public RocketWidgetOnscreenKeyboardManager(OnscreenKeyboardManager onscreenKeyboardManager)
        {
            this.onscreenKeyboardManager = onscreenKeyboardManager;

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
                                onscreenKeyboardManager.KeyboardMode = OnscreenKeyboardMode.Secure;
                                break;
                            case "text":
                                onscreenKeyboardManager.KeyboardMode = OnscreenKeyboardMode.Normal;
                                break;
                            default:
                                onscreenKeyboardManager.KeyboardMode = OnscreenKeyboardMode.Hidden;
                                break;
                        }
                        break;
                    case "textarea":
                        onscreenKeyboardManager.KeyboardMode = OnscreenKeyboardMode.Normal;
                        break;
                    default:
                        onscreenKeyboardManager.KeyboardMode = OnscreenKeyboardMode.Hidden;
                        break;
                }
            }
        }

        void HandleElementBlurred(RocketWidget widget, Element element)
        {
            if (widget == currentRocketWidget)
            {
                onscreenKeyboardManager.KeyboardMode = OnscreenKeyboardMode.Hidden;
            }
        }

        void HandleRocketWidgetDisposing(RocketWidget widget)
        {
            if (widget == currentRocketWidget)
            {
                currentRocketWidget = null;
                onscreenKeyboardManager.KeyboardMode = OnscreenKeyboardMode.Hidden;
                //Handle these for keyboard toggle right away or it won't work
                onscreenKeyboardManager.toggleKeyboard();
            }
        }
    }
}
