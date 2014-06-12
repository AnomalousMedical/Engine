using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// This is a repository for errors that occur when loading, you can call this from your factory or sim objects
    /// to report errors to this repository then a client program could choose to display them to help with debugging.
    /// 
    /// The lifecycle of the errors in this class must be handled by the client program. E.g. you must call Clear when
    /// loading a scene (or other object that could have errors you want to look for) and then check the HasErrors
    /// to display a message or handle the error.
    /// </summary>
    public static class SimObjectErrorManager
    {
        private static LinkedList<SimObjectError> errors = new LinkedList<SimObjectError>();

        /// <summary>
        /// Clear all errors in the repository.
        /// </summary>
        public static void Clear()
        {
            errors.Clear();
        }

        /// <summary>
        /// Add an error to the repository.
        /// </summary>
        /// <param name="error">The error to add.</param>
        public static void AddError(SimObjectError error)
        {
            errors.AddLast(error);
        }

        public static void AddAndLogError(SimObjectError error, LogLevel logLevel = LogLevel.Warning)
        {
            AddError(error);
            Log.Default.sendMessage("Error creating {0} named '{1}' in SimObject '{2}'. Reason: {3}", logLevel, error.Subsystem, error.Type, error.ElementName, error.SimObject, error.Message);
        }

        /// <summary>
        /// True if there are errors in the repository.
        /// </summary>
        public static bool HasErrors
        {
            get
            {
                return errors.First != null;
            }
        }

        /// <summary>
        /// An enumerator over all the errors.
        /// </summary>
        public static IEnumerable<SimObjectError> Errors
        {
            get
            {
                return errors;
            }
        }
    }
}
