using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Platform
{
    /// <summary>
    /// Represents the current onscreen keyboard mode.
    /// </summary>
    public enum OnscreenKeyboardMode
    {
        Hidden = 0,
        Normal = 1,
        Secure = 2,
    }

    /// <summary>
    /// This interface provides a way for subsystems to manage the onscreen keyboard.
    /// </summary>
    public interface OnscreenKeyboardManager
    {
        /// <summary>
        /// In rare instances you might have to toggle the onscreen keyboard manually right away, this function will
        /// do that, but most times this is handled automatically with no problems. If you are creating your own onscreen
        /// keyboard manager for a subsystem you might not ever actually have to call this function.
        /// </summary>
        void toggleKeyboard();

        /// <summary>
        /// The keyboard mode that will be set the next time togglekeyboard is called. This does not reflect
        /// the actual keyboard status.
        /// </summary>
        OnscreenKeyboardMode KeyboardMode { get; set; }
    }
}
