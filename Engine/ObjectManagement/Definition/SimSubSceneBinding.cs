using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    public class SimSubSceneBinding
    {
        public SimSubSceneBinding()
        {

        }

        [Editable("The name of the SimElementManager.")]
        public String SimElementManager { get; set; }

        [Editable("The type of the SimElementManager.")]
        public String TypeName { get; set; }
    }
}
