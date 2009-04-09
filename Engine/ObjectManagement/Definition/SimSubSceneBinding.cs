using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    /// <summary>
    /// This class creates a binding between a SimSubScene and a given type of SimElementManager.
    /// </summary>
    public class SimSubSceneBinding
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SimSubSceneBinding()
        {

        }

        [Editable("The name of the SimElementManager.")]
        public String SimElementManager { get; set; }

        [Editable("The type of the SimElementManager.")]
        public String TypeName { get; set; }
    }
}
