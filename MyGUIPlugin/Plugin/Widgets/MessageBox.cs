﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class wraps the Message class and makes using them a bit easier.
    /// </summary>
    public class MessageBox
    {
        public delegate void MessageClosedDelegate(MessageBoxStyle result);

        /// <summary>
        /// This dictionary of delegates keeps the delegates alive and is VERY IMPORTANT, don't try to get cute
        /// with this class.
        /// </summary>
        private static Dictionary<Message, MessageClosedDelegate> delegates = new Dictionary<Message, MessageClosedDelegate>();

        private MessageBox()
        {

        }

        /// <summary>
        /// This function will show a message box with the given options and call callback when it is closed.
        /// </summary>
        /// <param name="message">The message to go in the box.</param>
        /// <param name="caption">The caption of the message box.</param>
        /// <param name="style">The style to use for the message box.</param>
        public static void show(String message, String caption, MessageBoxStyle style)
        {
            show(message, caption, style, null);
        }

        /// <summary>
        /// This function will show a message box with the given options and call callback when it is closed.
        /// </summary>
        /// <param name="message">The message to go in the box.</param>
        /// <param name="caption">The caption of the message box.</param>
        /// <param name="style">The style to use for the message box.</param>
        /// <param name="callback">The callback to call when the box is closed.</param>
        public static void show(String message, String caption, MessageBoxStyle style, MessageClosedDelegate callback)
        {
            Message box = Message.createMessageBox("MyGUIPlugin.Resources.MessageBox.MessageBox.layout", caption, message, style);
            box.MessageBoxResult += box_MessageBoxResult;
            delegates.Add(box, callback);
        }

        static void box_MessageBoxResult(Message source, EventArgs e)
        {
            Delegate del = delegates[source];
            if (del != null)
            {
                del.DynamicInvoke(((MessageEventArgs)e).Result);
            }
            delegates.Remove(source);
        }
    }
}
