using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This is an exception that is thrown if there is an error with a coroutine.
    /// </summary>
    public class CoroutineException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">The error message.</param>
        public CoroutineException(String message)
            :base(message)
        {

        }
    }
}
