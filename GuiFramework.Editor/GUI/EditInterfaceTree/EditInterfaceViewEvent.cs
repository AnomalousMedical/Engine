using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Anomalous.GuiFramework.Editor
{
    /// <summary>
    /// The delegate for EditInterfaceViews.
    /// </summary>
    /// <param name="evt">The EditInterfaceViewEvent.</param>
    public delegate void EditInterfaceViewEvent(EditInterfaceViewEventArgs evt);

    /// <summary>
    /// This event is passed on EditInterfaceViewEvents.
    /// </summary>
    public class EditInterfaceViewEventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="editInterface"></param>
        public EditInterfaceViewEventArgs(EditInterface editInterface)
        {
            this.EditInterface = editInterface;
            this.Cancel = false;
        }

        /// <summary>
        /// The EditInterface that has been changed to.
        /// </summary>
        public EditInterface EditInterface { get; set; }

        /// <summary>
        /// Set to true to cancel an event that is about to occur. Does not work
        /// on events that occur after a change has been made.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
