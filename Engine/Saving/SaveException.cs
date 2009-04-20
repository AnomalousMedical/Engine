using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Saving
{
    /// <summary>
    /// This is an exception that is thrown if there is an error saving.
    /// </summary>
    class SaveException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">A message for the exception.</param>
        public SaveException(String message)
            : base(message)
        {

        }
    }
}
