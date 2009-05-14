using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace Editor
{
    /// <summary>
    /// This interface allows an implementor to provide access to a DockPanel.
    /// </summary>
    public interface IDockProvider
    {
        /// <summary>
        /// Add content to the main docking interface.
        /// </summary>
        /// <param name="content">The content to add.</param>
        void showDockContent(DockContent content);

        /// <summary>
        /// Remove content from the main docking interface.
        /// </summary>
        /// <param name="content">The content to remove.</param>
        void hideDockContent(DockContent content);
    }
}
