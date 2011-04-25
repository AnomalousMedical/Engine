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
        /// Determine if this ClipboardEntry can have objects of Type type pasted into it.
        /// </summary>
        /// <param name="type">The type to try and paste.</param>
        /// <returns>True if that type can be pasted.</returns>
        bool supportsPastingType(Type type);

        /// <summary>
        /// Get the type of the object that will be copied through this interface.
        /// </summary>
        /// <returns></returns>
        Type ObjectType { get; }

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
