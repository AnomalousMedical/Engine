using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    /// <summary>
    /// This exception is thrown when there is a validation error during a Validate callback in an EditInterface.
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(String message)
            :base(message)
        {

        }

        public ValidationException(String message, params String[] args)
            : base(String.Format(message, args))
        {

        }
    }
}
