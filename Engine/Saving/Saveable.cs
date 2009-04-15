using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// This interface allows a class to be saved using the Engine serializer.
    /// It must implement getInfo and also provide a constructor that takes the
    /// same arguments as getInfo.
    /// </summary>
    public interface Saveable
    {
        /// <summary>
        /// Get the info to save for the implementing class.
        /// </summary>
        /// <param name="info">The SaveInfo class to save into.</param>
        void getInfo(SaveInfo info);
    }
}
