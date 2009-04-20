using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// This is an interface for a class that writes a specific value a specific
    /// way to the underlying stream.
    /// </summary>
    public interface ValueWriter
    {
        /// <summary>
        /// Write a value to the underlying stream.
        /// </summary>
        /// <param name="entry">The SaveEntry for the value to write.</param>
        void writeValue(SaveEntry entry);

        /// <summary>
        /// Get the type that this ValueWriter writes.
        /// </summary>
        /// <returns>The Type that this ValueWriter is responsible for writing.</returns>
        Type getWriteType();
    }
}
