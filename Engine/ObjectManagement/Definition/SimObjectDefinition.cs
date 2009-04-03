using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class SimObjectDefinition
    {
        private LinkedList<SimComponentDefinition> definitions = new LinkedList<SimComponentDefinition>();

        void addSimComponentDefinition(SimComponentDefinition definition)
        {
            definitions.AddLast(definition);
        }

        void removeSimComponentDefinition(SimComponentDefinition definition)
        {
            definitions.Remove(definition);
        }

        void register()
        {
            foreach (SimComponentDefinition definition in definitions)
            {
                definition.register();
            }
        }

        void unregister()
        {
            foreach (SimComponentDefinition definition in definitions)
            {
                definition.unregister();
            }
        }
    }
}
