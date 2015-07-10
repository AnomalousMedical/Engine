using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Threads
{
    /// <summary>
    /// A convience class to cancel a thread with an exception. You can catch this in your outermost
    /// thread function and use it to cancel those calls.
    /// </summary>
    public class CancelThreadException : Exception
    {
        public CancelThreadException()
        {

        }
    }
}
