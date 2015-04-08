using Anomalous.OSPlatform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Minimus
{
    class MyGUIOnscreenKeyboardManager
    {
        private TouchMouseGuiForwarder guiForwarder;

        public MyGUIOnscreenKeyboardManager(TouchMouseGuiForwarder guiForwarder)
        {
            this.guiForwarder = guiForwarder;
            InputManager.Instance.ChangeKeyFocus += changeKeyFocus;
        }

        void changeKeyFocus(Widget widget)
        {
            //if (currentRocketWidget == null || !currentRocketWidget.isHostWidget(widget))
            {
                EditBox editBox = widget as EditBox;
                if (editBox != null)
                {
                    guiForwarder.KeyboardMode = editBox.EditPassword ? OnscreenKeyboardMode.Secure : OnscreenKeyboardMode.Normal;
                }
                else
                {
                    guiForwarder.KeyboardMode = OnscreenKeyboardMode.Hidden;
                }
            }
        }
    }
}
