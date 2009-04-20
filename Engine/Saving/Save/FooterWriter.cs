using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// Writes the trailing section of a object in a stream.
    /// </summary>
    public interface FooterWriter
    {
        /// <summary>
        /// Write the trailing section of an object.
        /// </summary>
        /// <param name="objectId">The ObjectIdentifier of the object being written.</param>
        void writeFooter(ObjectIdentifier objectId);
    }
}
