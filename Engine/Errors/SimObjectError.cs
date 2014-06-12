using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// The container for errors.
    /// </summary>
    public class SimObjectError
    {
        /// <summary>
        /// The name of the subsystem that generated the error.
        /// </summary>
        public String Subsystem { get; set; }

        /// <summary>
        /// The name of the SimElement (or its definition) that generated the error.
        /// </summary>
        public String ElementName { get; set; }

        /// <summary>
        /// The type of the SimElement that generated the error.
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// The name of the SimObject that contains the element that generated the error.
        /// </summary>
        public String SimObject { get; set; }

        /// <summary>
        /// A message about what happened.
        /// </summary>
        public String Message { get; set; }
    }
}
