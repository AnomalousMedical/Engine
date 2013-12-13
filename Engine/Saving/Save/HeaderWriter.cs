using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// Writes the leading section of an object to the stream.
    /// </summary>
    public interface HeaderWriter
    {
        /// <summary>
        /// Write the leading section of an object to a stream.
        /// </summary>
        /// <param name="objectId">The ObjectIdentifier of the object to write.</param>
        void writeHeader(ObjectIdentifier objectId, int version);
    }
}
