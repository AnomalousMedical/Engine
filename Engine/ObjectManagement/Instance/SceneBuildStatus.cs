using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ObjectManagement
{
    public class SceneBuildStatus
    {
        public String Subsystem { get; set; }

        public int NumItems { get; set; }

        public int CurrentItem { get; set; }

        public String Message { get; set; }
    }
}
