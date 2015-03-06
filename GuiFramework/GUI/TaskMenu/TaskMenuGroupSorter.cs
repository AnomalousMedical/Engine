using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework
{
    /// <summary>
    /// An interface to sort groups in the task menu.
    /// </summary>
    public interface TaskMenuGroupSorter
    {
        /// <summary>
        /// Called when a new group is added.
        /// </summary>
        /// <param name="name"></param>
        void groupAdded(String name);

        /// <summary>
        /// Sort function.
        /// </summary>
        /// <param name="x">The left item.</param>
        /// <param name="y">The right item.</param>
        /// <returns>Negative if x &lt; y Positive if x & gt; y, 0 if equal.</returns>
        int compareGroups(String x, String y);
    }
}
