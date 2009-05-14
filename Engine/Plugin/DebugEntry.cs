using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This is a single item that can be displayed for debugging.
    /// </summary>
    public interface DebugEntry
    {
        /// <summary>
        /// Set this entry as enabled or disabled.
        /// </summary>
        /// <param name="enabled">True to enable, false to disable.</param>
        void setEnabled(bool enabled);

        /// <summary>
        /// Get the textual name for this entry.
        /// </summary>
        String Text { get; }
    }
}
