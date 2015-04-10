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

        /// <summary>
        /// Determine the OnscreenKeyboardMode for a given element. This does not have
        /// anything to do with blur / focus, it will return the type of keyboard that should
        /// be visible. This means you can also use this as a test funciton if OnscreenKeyboardMode.Hidden
        /// then you know the element should have a keyboard shown.
        /// </summary>
        /// <param name="element">The element to test</param>
        /// <returns>The mode for the onscreen keyboard if this element was focused.</returns>
        internal static OnscreenKeyboardMode GetKeyboardModeForElement(Element element)
        {
            if (element != null)
            {
                String tag = element.TagName;
                switch (tag)
                {
                    case "input":
                        String type = element.GetAttributeString("type");
                        switch (type)
                        {
                            case "password":
                                return OnscreenKeyboardMode.Secure;
                            case "text":
                                return OnscreenKeyboardMode.Normal;
                            default:
                                return OnscreenKeyboardMode.Hidden;
                        }
                    case "textarea":
                        return OnscreenKeyboardMode.Normal;
                    default:
                        return OnscreenKeyboardMode.Hidden;
                }
            }
            else
            {
                return OnscreenKeyboardMode.Hidden;
            }
        }

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
                onscreenKeyboardManager.KeyboardMode = GetKeyboardModeForElement(element);
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
