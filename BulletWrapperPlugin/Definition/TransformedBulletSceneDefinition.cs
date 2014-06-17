using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    public class TransformedBulletSceneDefinition : BulletSceneDefinition
    {
        internal static SimElementManagerDefinition CreateTransformed(String name, EditUICallback callback)
        {
            return new TransformedBulletSceneDefinition(name);
        }

        public TransformedBulletSceneDefinition(String name)
            :base(name)
        {
            
        }

        public override SimElementManager createSimElementManager()
        {
            return new TransformedBulletScene(this, BulletInterface.Instance.UpdateTimer);
        }

        [Editable]
        public String TransformSimObjectName { get; set; }

        protected TransformedBulletSceneDefinition(LoadInfo info)
            :base(info)
        {
            TransformSimObjectName = info.GetString("TransformSimObjectName");
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("TransformSimObjectName", TransformSimObjectName);
        }
    }
}
