using Engine.Platform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    public class MyGUIOnscreenKeyboardManager
    {
        private OnscreenKeyboardManager onscreenKeyboardManager;

        public MyGUIOnscreenKeyboardManager(OnscreenKeyboardManager onscreenKeyboardManager)
        {
            this.onscreenKeyboardManager = onscreenKeyboardManager;
            InputManager.Instance.ChangeKeyFocus += changeKeyFocus;
        }

        void changeKeyFocus(Widget widget)
        {
            EditBox editBox = widget as EditBox;
            if (editBox != null && !editBox.EditStatic)
            {
                onscreenKeyboardManager.KeyboardMode = editBox.EditPassword ? OnscreenKeyboardMode.Secure : OnscreenKeyboardMode.Normal;
            }
            else
            {
                onscreenKeyboardManager.KeyboardMode = OnscreenKeyboardMode.Hidden;
            }
        }
    }
}
