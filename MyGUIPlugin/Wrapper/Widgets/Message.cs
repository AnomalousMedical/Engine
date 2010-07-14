using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public enum MessageBoxStyle
    {
        None = 0,
        Ok = 1 << (0),
        Yes = 1 << (1),
        No = 1 << (2),
        Abort = 1 << (3),
        Retry = 1 << (4),
        Ignore = 1 << (5),
        Cancel = 1 << (6),
        Try = 1 << (7),
        Continue = 1 << (8),

        _IndexUserButton1 = 9, // èíäåêñ ïåðâîé êíîïêè þçåðà

        Button1 = 1 << (_IndexUserButton1),
        Button2 = 1 << (_IndexUserButton1 + 1),
        Button3 = 1 << (_IndexUserButton1 + 2),
        Button4 = 1 << (_IndexUserButton1 + 3),

        _CountUserButtons = 4, // êîëëè÷åñòâî êíîïîê þçåðà
        _IndexIcon1 = _IndexUserButton1 + _CountUserButtons, // èíäåêñ ïåðâîé èêîíêè

        IconDefault = 1 << (_IndexIcon1),

        IconInfo = 1 << (_IndexIcon1),
        IconQuest = 1 << (_IndexIcon1 + 1),
        IconError = 1 << (_IndexIcon1 + 2),
        IconWarning = 1 << (_IndexIcon1 + 3),

        Icon1 = 1 << (_IndexIcon1),
        Icon2 = 1 << (_IndexIcon1 + 1),
        Icon3 = 1 << (_IndexIcon1 + 2),
        Icon4 = 1 << (_IndexIcon1 + 3),
        Icon5 = 1 << (_IndexIcon1 + 4),
        Icon6 = 1 << (_IndexIcon1 + 5),
        Icon7 = 1 << (_IndexIcon1 + 6),
        Icon8 = 1 << (_IndexIcon1 + 7)
    };

    public class Message : Window
    {
        public event MyGUIEvent MessageBoxResult;

        public Message(IntPtr message)
            : base(message)
        {
            //Events work a bit different for this class. See the messageBoxClosed function for more info.
            eventManager.addDelegate<EventMessageBoxResultTranslator>(messageBoxClosed);
        }

        public void setMessageText(String value)
        {
            Message_setMessageText(widget, value);
        }

        public void addButtonName(String name)
        {
            Message_addButtonName(widget, name);
        }

        public void setSmoothShow(bool value)
        {
            Message_setSmoothShow(widget, value);
        }

        public String getDefaultLayer()
        {
            return Marshal.PtrToStringAnsi(Message_getDefaultLayer(widget));
        }

        public void setMessageIcon(MessageBoxStyle value)
        {
            Message_setMessageIcon(widget, value);
        }

        public void setWindowFade(bool value)
        {
            Message_setWindowFade(widget, value);
        }

        public void endMessage(MessageBoxStyle result)
        {
            Message_endMessage(widget, result);
        }

        public void endMessage()
        {
            Message_endMessage2(widget);
        }

        public void setMessageButton(MessageBoxStyle value)
        {
            Message_setMessageButton(widget, value);
        }

        public void setMessageStyle(MessageBoxStyle value)
        {
            Message_setMessageStyle(widget, value);
        }

        public void setMessageModal(bool value)
        {
            Message_setMessageModal(widget, value);
        }

        public static Message createMessageBox(
         String skin,  //regular
         String caption,   //ustring
         String message,   //ustring
         MessageBoxStyle style,
         String layer,    //regular
         bool modal,
         String button1,  //regular
         String button2,  //regular
         String button3,  //regular
         String button4) //regular
        {
            return WidgetManager.getWidget(Message_createMessageBox(skin, caption, message, style, layer, modal, button1, button2, button3, button4)) as Message;
        }

        //Variations of createMessageBox

        public static Message createMessageBox(
         String skin,
         String caption,
         String message,
         MessageBoxStyle style,
         String layer,
         bool modal,
         String button1,
         String button2,
         String button3)
        {
            return createMessageBox(skin, caption, message, style, layer, modal, button1, button2, button3, "");
        }

        public static Message createMessageBox(
         String skin,
         String caption,
         String message,
         MessageBoxStyle style,
         String layer,
         bool modal,
         String button1,
         String button2)
        {
            return createMessageBox(skin, caption, message, style, layer, modal, button1, button2, "", "");
        }

        public static Message createMessageBox(
         String skin,
         String caption,
         String message,
         MessageBoxStyle style,
         String layer,
         bool modal,
         String button1)
        {
            return createMessageBox(skin, caption, message, style, layer, modal, button1, "", "", "");
        }

        public static Message createMessageBox(
         String skin,
         String caption,
         String message,
         MessageBoxStyle style,
         String layer,
         bool modal)
        {
            return createMessageBox(skin, caption, message, style, layer, modal, "", "", "", "");
        }

        public static Message createMessageBox(
         String skin,
         String caption,
         String message,
         MessageBoxStyle style,
         String layer)
        {
            return createMessageBox(skin, caption, message, style, layer, true, "", "", "", "");
        }

        public static Message createMessageBox(
         String skin,
         String caption,
         String message,
         MessageBoxStyle style)
        {
            return createMessageBox(skin, caption, message, style, "", true, "", "", "", "");
        }

        public static Message createMessageBox(
         String skin,
         String caption,
         String message)
        {
            return createMessageBox(skin, caption, message, MessageBoxStyle.Ok | MessageBoxStyle.IconDefault, "", true, "", "", "", "");
        }

        /// <summary>
        /// The message box will destroy itself inside MyGUI when it is closed.
        /// This function will hear that event and broadcast it to the event
        /// held by this class. It will also destroy the wrappers.
        /// </summary>
        /// <param name="source">The source widget.</param>
        /// <param name="args">The event args.</param>
        private void messageBoxClosed(Widget source, EventArgs args)
        {
            if (MessageBoxResult != null)
            {
                MessageBoxResult.Invoke(source, args);
            }
            WidgetManager.deleteWrapperAndChildren(this);
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern void Message_setMessageText(IntPtr message, [MarshalAs(UnmanagedType.LPWStr)] String value);

        [DllImport("MyGUIWrapper")]
        private static extern void Message_addButtonName(IntPtr message, [MarshalAs(UnmanagedType.LPWStr)] String name);

        [DllImport("MyGUIWrapper")]
        private static extern void Message_setSmoothShow(IntPtr message, bool value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Message_getDefaultLayer(IntPtr message);

        [DllImport("MyGUIWrapper")]
        private static extern void Message_setMessageIcon(IntPtr message, MessageBoxStyle value);

        [DllImport("MyGUIWrapper")]
        private static extern void Message_setWindowFade(IntPtr message, bool value);

        [DllImport("MyGUIWrapper")]
        private static extern void Message_endMessage(IntPtr message, MessageBoxStyle result);

        [DllImport("MyGUIWrapper")]
        private static extern void Message_endMessage2(IntPtr message);

        [DllImport("MyGUIWrapper")]
        private static extern void Message_setMessageButton(IntPtr message, MessageBoxStyle value);

        [DllImport("MyGUIWrapper")]
        private static extern void Message_setMessageStyle(IntPtr message, MessageBoxStyle value);

        [DllImport("MyGUIWrapper")]
        private static extern void Message_setMessageModal(IntPtr message, bool value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Message_createMessageBox(
         String skin,
         [MarshalAs(UnmanagedType.LPWStr)] String caption,
         [MarshalAs(UnmanagedType.LPWStr)] String message,
         MessageBoxStyle style,
         String layer,
         bool modal,
         String button1,
         String button2,
         String button3,
         String button4);

#endregion
    }
}