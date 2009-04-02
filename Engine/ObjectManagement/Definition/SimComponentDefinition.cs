using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ObjectManagement.Definition
{
    interface SimComponentDefinition
    {
        void register();

        void unregister();
    }
}
