using Engine;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUikFactoryEntry
    {
        SimObjectBase instance;
        BEPUikElementDefinition element;

        public BEPUikFactoryEntry(SimObjectBase instance, BEPUikElementDefinition element)
        {
            this.instance = instance;
            this.element = element;
        }

        public void createProduct(BEPUikScene scene)
        {
            try
            {
                element.createProduct(instance, scene);
            }
            catch(BEPUikBlacklistException ex)
            {
                SimObjectErrorManager.AddAndLogError(new SimObjectError()
                {
                    Subsystem = BEPUikInterface.PluginName,
                    ElementName = element.Name,
                    Type = element.GetType().Name,
                    SimObject = instance.Name,
                    Message = ex.Message
                });
            }
        }
    }
}
