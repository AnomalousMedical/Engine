using Engine;
using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikBoneDefinition : BEPUikElementDefinition
    {
        [DoNotSave]
        private EditInterface editInterface;

        public BEPUikBoneDefinition(String name)
            :base(name)
        {
            Radius = 1.0f;
            Height = 3.0f;
            Mass = 1.0f;
        }

        [Editable]
        public bool Pinned { get; set; }

        [Editable]
        public float Radius { get; set; }

        [Editable]
        public float Height { get; set; }

        [EditableMinMax(0.0000001, float.MaxValue, 1.0f)]
        public float Mass { get; set; }

        [Editable]
        public Vector3? LocalRot
        {
            get
            {
                if (LocalRotQuat.HasValue)
                {
                    return LocalRotQuat.Value.getEuler() * Degree.FromRadian;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    LocalRotQuat = new Quaternion(value.Value * Radian.FromDegrees);
                }
                else
                {
                    LocalRotQuat = null;
                }
            }
        }

        public Quaternion? LocalRotQuat { get; set; }

        public override void registerScene(SimSubScene subscene, SimObjectBase instance)
        {
            if (subscene.hasSimElementManagerType(typeof(BEPUikScene)))
            {
                BEPUikScene sceneManager = subscene.getSimElementManager<BEPUikScene>();
                sceneManager.IkFactory.addBone(this, instance);
            }
            else
            {
                Log.Default.sendMessage("Cannot add BEPUikBone {0} to SimSubScene {1} because it does not contain a BEPUikScene.", LogLevel.Warning, BEPUikInterface.PluginName, Name, subscene.Name);
            }
        }

        protected override EditInterface createEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, String.Format("{0} - IK Bone", Name));
                editInterface.Renderer = new BoneRenderer(this);
            }
            return editInterface;
        }

        internal override void createProduct(SimObjectBase instance, BEPUikScene scene)
        {
            BEPUikBone bone;
            if (LocalRotQuat.HasValue)
            {
                bone = new BEPUikBoneLocalRot(this, instance, scene);
            }
            else
            {
                bone = new BEPUikBone(this, instance, scene);
            }
            instance.addElement(bone);
        }

        protected BEPUikBoneDefinition(LoadInfo info)
            :base(info)
        {
            Pinned = info.GetBoolean("Pinned");
            Radius = info.GetFloat("Radius");
            Height = info.GetFloat("Height");
            Mass = info.GetFloat("Mass", 1.0f);
            if(info.hasValue("LocalRotQuat"))
            {
                LocalRotQuat = info.GetQuaternion("LocalRotQuat");
            }
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("Pinned", Pinned);
            info.AddValue("Radius", Radius);
            info.AddValue("Height", Height);
            info.AddValue("Mass", Mass);
            if(LocalRotQuat.HasValue)
            {
                info.AddValue("LocalRotQuat", LocalRotQuat.Value);
            }
        }

        internal static void Create(string name, EditUICallback callback, CompositeSimObjectDefinition simObjectDefinition)
        {
            simObjectDefinition.addElement(new BEPUikBoneDefinition(name));
        }
    }
}
