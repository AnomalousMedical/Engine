using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Anomaly
{
    public class InstanceFileInterface : EditableFileInterface<Instance>
    {
        public InstanceFileInterface(String name, Object iconReferenceTag, String filename)
            :base(name, iconReferenceTag, filename)
        {
            Deleted = false;
        }

        public override EditInterface getObjectEditInterface(object obj)
        {
            Instance instance = obj as Instance;
            if (instance != null)
            {
                return instance.getEditInterface();
            }
            else
            {
                throw new Exception(String.Format("Cannot get edit interface for object {0} because it is not of type {1}", obj.ToString(), typeof(Instance).ToString()));
            }
        }

        public override void saveObject(object obj)
        {
            if (!Deleted)
            {
                base.saveObject(obj);
            }
        }

        public bool Deleted { get; set; }
    }
}
