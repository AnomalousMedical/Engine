using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Saving;
using Engine.Reflection;
using Engine.Attributes;
using Engine.Editing;
using EngineMath;

namespace PhysXPlugin
{
    public class ShapeDefinitionBase<Shape> : ShapeDefinition
        where Shape : PhysShapeDesc
    {
        private static MemberScanner shapeMemberScanner;

        [DoNotCopy]
        protected Shape shapeDesc;
        private Vector3 localTranslation = Vector3.Zero;
        private Quaternion localRotation = Quaternion.Identity;

        static ShapeDefinitionBase()
        {
            shapeMemberScanner = new MemberScanner();
            shapeMemberScanner.ProcessFields = false;
            shapeMemberScanner.Filter = new EditableAttributeFilter();
        }

        protected ShapeDefinitionBase(Shape shapeDesc)
        {
            this.shapeDesc = shapeDesc;
        }

        public override EditInterface getEditInterface()
        {
            return ReflectedEditInterface.createEditInterface(this, shapeMemberScanner, shapeDesc.GetType().Name, null);
        }

        internal override PhysShapeDesc PhysShapeDesc
        {
            get
            {
                return shapeDesc;
            }
        }

        [Editable]
        public override float Density
        {
            get
            {
                return shapeDesc.Density;
            }
            set
            {
                shapeDesc.Density = value;
            }
        }

        [Editable]
        public override ushort Group
        {
            get
            {
                return shapeDesc.Group;
            }
            set
            {
                shapeDesc.Group = value;
            }
        }

        [Editable]
        public override float Mass
        {
            get
            {
                return shapeDesc.Mass;
            }
            set
            {
                shapeDesc.Mass = value;
            }
        }

        [Editable]
        public override ushort MaterialIndex
        {
            get
            {
                return shapeDesc.MaterialIndex;
            }
            set
            {
                shapeDesc.MaterialIndex = value;
            }
        }

        [Editable]
        public override uint NonInteractingCompartmentTypes
        {
            get
            {
                return shapeDesc.NonInteractingCompartmentTypes;
            }
            set
            {
                shapeDesc.NonInteractingCompartmentTypes = value;
            }
        }

        [Editable]
        public override ShapeFlag ShapeFlags
        {
            get
            {
                return shapeDesc.ShapeFlags;
            }
            set
            {
                shapeDesc.ShapeFlags = value;
            }
        }

        [Editable]
        public override float SkinWidth
        {
            get
            {
                return shapeDesc.SkinWidth;
            }
            set
            {
                shapeDesc.SkinWidth = value;
            }
        }

        [Editable]
        public override Vector3 LocalTranslation
        {
            get
            {
                return localTranslation;
            }
            set
            {
                localTranslation = value;
                shapeDesc.setLocalPose(localTranslation, localRotation);
            }
        }

        [Editable]
        public override Quaternion LocalRotation
        {
            get
            {
                return localRotation;
            }
            set
            {
                localRotation = value;
                shapeDesc.setLocalPose(localTranslation, localRotation);
            }
        }

        #region Saveable Members

        private const String DENSITY = "Density";
        private const String GROUP = "Group";
        private const String MASS = "Mass";
        private const String MATERIAL_INDEX = "MaterialIndex";
        private const String NON_INTERACTING_COMPARTMENT = "NonInteractingCompartmentTypes";
        private const String SHAPE_FLAG = "ShapeFlags";
        private const String SKIN_WIDTH = "SkinWidth";
        private const String LOCAL_TRANSLATION = "LocalTranslation";
        private const String LOCAL_ROTATION = "LocalRotation";

        protected ShapeDefinitionBase(Shape shapeDesc, LoadInfo info)
        {
            this.shapeDesc = shapeDesc;
            ReflectedSaver.RestoreObject(shapeDesc, info, shapeMemberScanner);
            Density = info.GetFloat(DENSITY);
            Group = info.GetUInt16(GROUP);
            Mass = info.GetFloat(MASS);
            MaterialIndex = info.GetUInt16(MATERIAL_INDEX);
            NonInteractingCompartmentTypes = info.GetUInt32(NON_INTERACTING_COMPARTMENT);
            ShapeFlags = info.GetValue<ShapeFlag>(SHAPE_FLAG);
            SkinWidth = info.GetFloat(SKIN_WIDTH);
            LocalTranslation = info.GetVector3(LOCAL_TRANSLATION);
            LocalRotation = info.GetQuaternion(LOCAL_ROTATION);
        }

        public override void getInfo(SaveInfo info)
        {
            info.AddValue(DENSITY, Density);
            info.AddValue(GROUP, Group);
            info.AddValue(MASS, Mass);
            info.AddValue(MATERIAL_INDEX, MaterialIndex);
            info.AddValue(NON_INTERACTING_COMPARTMENT, NonInteractingCompartmentTypes);
            info.AddValue(SHAPE_FLAG, ShapeFlags);
            info.AddValue(SKIN_WIDTH, SkinWidth);
            info.AddValue(LOCAL_TRANSLATION, LocalTranslation);
            info.AddValue(LOCAL_ROTATION, LocalRotation);
        }

        #endregion
    }
}
