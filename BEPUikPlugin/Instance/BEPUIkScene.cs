using BEPUik;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikScene : SimElementManager
    {
        private String name;
        private BEPUIkFactory factory;
        private List<Bone> bones = new List<Bone>();
        private List<Control> controls = new List<Control>();

        public BEPUikScene(String name)
        {
            this.name = name;
            factory = new BEPUIkFactory(this);
        }

        public void Dispose()
        {

        }

        public SimElementFactory getFactory()
        {
            return factory;
        }

        internal BEPUIkFactory IkFactory
        {
            get
            {
                return factory;
            }
        }

        public Type getSimElementManagerType()
        {
            return typeof(BEPUikScene);
        }

        public string getName()
        {
            return name;
        }

        public SimElementManagerDefinition createDefinition()
        {
            return new BEPUikSceneDefinition(name);
        }

        internal void addBone(Bone bone)
        {
            bones.Add(bone);
        }

        internal void removeBone(Bone bone)
        {
            bones.Remove(bone);
        }

        internal void addControl(Control control)
        {
            controls.Add(control);
        }

        internal void removeControl(Control control)
        {
            controls.Add(control);
        }
    }
}
