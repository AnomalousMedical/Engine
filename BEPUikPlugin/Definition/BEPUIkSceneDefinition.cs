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
    public class BEPUikSceneDefinition : SimElementManagerDefinition
    {
        internal static SimElementManagerDefinition Create(string name, EditUICallback callback)
        {
            return new BEPUikSceneDefinition(name);
        }

        private String name;

        [DoNotSave]
        private EditInterface editInterface;

        public BEPUikSceneDefinition(String name)
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
            return BEPUikInterface.Instance.createScene(this);
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
            return typeof(BEPUikSceneDefinition);
        }

        public void Dispose()
        {
            
        }

        protected BEPUikSceneDefinition(LoadInfo info)
        {
            ReflectedSaver.RestoreObject(this, info); 
        }

        public void getInfo(SaveInfo info)
        {
            ReflectedSaver.SaveObject(this, info);
        }
    }
}
