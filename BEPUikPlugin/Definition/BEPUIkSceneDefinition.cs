using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUIkSceneDefinition : SimElementManagerDefinition
    {
        internal static SimElementManagerDefinition Create(string name, EditUICallback callback)
        {
            return new BEPUIkSceneDefinition(name);
        }

        private String name;

        [DoNotSave]
        private EditInterface editInterface;

        public BEPUIkSceneDefinition(String name)
        {
            this.name = name;
        }

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, ReflectedEditInterface.DefaultScanner, name + " - BEPUIk Scene", null);
            }
            return editInterface;
        }

        public SimElementManager createSimElementManager()
        {
            return new BEPUIkScene(name);
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Type getSimElementManagerType()
        {
            return typeof(BEPUIkSceneDefinition);
        }

        public void Dispose()
        {
            
        }

        protected BEPUIkSceneDefinition(LoadInfo info)
        {
            ReflectedSaver.RestoreObject(this, info); 
        }

        public void getInfo(SaveInfo info)
        {
            ReflectedSaver.SaveObject(this, info);
        }
    }
}
