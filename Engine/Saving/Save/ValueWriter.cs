using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// This interface allows a subclass to be alerted by the SaveControl that a
    /// value is to be written.
    /// </summary>
    public interface ValueWriter
    {
        /// <summary>
        /// Write an entry to the underlying stream.
        /// </summary>
        /// <param name="entry">The entry to write.</param>
        void writeValue(SaveEntry entry);
    }
}
