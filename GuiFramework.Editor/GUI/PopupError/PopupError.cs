using Engine;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Editor
{
    class PopupError : PopupContainer
    {
        public static void ShowPopup(String text, IntVector2 location)
        {
            PopupError error = new PopupError(text);
            error.show(location.x, location.y);
        }

        PopupError(String message)
            :base("Anomalous.GuiFramework.Editor.GUI.PopupError.PopupError.layout")
        {
            this.Hidden += PopupError_Hidden;
            EditBox messageText = widget.findWidget("Message") as EditBox;
            IntSize2 sizeDiff = new IntSize2(widget.Width - messageText.Width, widget.Height - messageText.Height);
            messageText.Caption = message;
            IntSize2 messageSize = messageText.getTextSize();
            messageText.setSize(messageSize.Width, messageSize.Height);
            widget.setSize(messageSize.Width + sizeDiff.Width, messageSize.Height + sizeDiff.Height);
        }

        void PopupError_Hidden(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
