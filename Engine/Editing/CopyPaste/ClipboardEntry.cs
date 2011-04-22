using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public interface ClipboardEntry
    {
        /// <summary>
        /// This method will copy the source object. This will leave the original intact.
        /// </summary>
        /// <returns>An object that is a copy.</returns>
        object copy();

        /// <summary>
        /// This method will cut the source object. The original object should be deleted.
        /// </summary>
        /// <returns>An object that has been cut.</returns>
        object cut();

        /// <summary>
        /// Paste an object into this entry.
        /// </summary>
        /// <param name="pasted">The object being "pasted" it will never be something that is not supported in SupportedTypes.</param>
        void paste(Object pasted);

        /// <summary>
        /// Get the type of the object that will be copied through this interface.
        /// </summary>
        /// <returns></returns>
        Type ObjectType { get; }

        /// <summary>
        /// A listing of all types that this interface can have pasted into it.
        /// This can be null to support all types.
        /// </summary>
        IEnumerable<Type> SupportedTypes { get; }

        /// <summary>
        /// Return true if this entry can copy.
        /// </summary>
        bool SupportsCopy { get; }

        /// <summary>
        /// Return true if this entry can cut.
        /// </summary>
        bool SupportsCut { get; }

        /// <summary>
        /// Return true if this entry can paste.
        /// </summary>
        bool SupportsPaste { get; }
    }
}
