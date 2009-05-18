using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This exception is thrown if a plugin is invalid for some reason.
    /// </summary>
    public class InvalidPluginException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The reason the plugin failed to load.</param>
        public InvalidPluginException(String message)
            :base(message)
        {

        }

        public InvalidPluginException(String message, Exception innerException)
            :base(message, innerException)
        {

        }
    }
}
