using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class FocusEventArgs : EventArgs
    {
        /// <summary>
        /// The other widget for the focus event. If the event is set focus the
        /// other widget is the last widget with focus. If the event is lost
        /// focus the other widget is the one gaining focus.
        /// </summary>
        public Widget OtherWidget { get; internal set; }
    }
}
