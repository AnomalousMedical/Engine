using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;

namespace BulletPlugin
{
    public class Generic6DofConstraintDefinition : TypedConstraintDefinition
    {
        [Editable] internal TranslationalLimitMotorDefinition translationMotor;
        [Editable] internal RotationalLimitMotorDefinition xRotMotor;
        [Editable] internal RotationalLimitMotorDefinition yRotMotor;
        [Editable] internal RotationalLimitMotorDefinition zRotMotor;

        public Generic6DofConstraintDefinition(String name)
            :base(name)
        {
            translationMotor = new TranslationalLimitMotorDefinition();
            xRotMotor = new RotationalLimitMotorDefinition();
            yRotMotor = new RotationalLimitMotorDefinition();
            zRotMotor = new RotationalLimitMotorDefinition();
        }

        protected override TypedConstraintElement createConstraint(RigidBody rbA, RigidBody rbB, SimObjectBase instance, BulletScene scene)
        {
            if(rbA != null && rbB != null)
	        {
		        return new Generic6DofConstraintElement(this, instance, rbA, rbB, scene);
	        }
	        return null;
        }

        public override string JointType
        {
            get
            {
                return "Generic 6 Dof Constraint";
            }
        }

        public TranslationalLimitMotorDefinition TranslationMotor
        {
            get
            {
                return translationMotor;
            }
        }

        public RotationalLimitMotorDefinition XRotMotor
        {
            get
            {
                return xRotMotor;
            }
        }

        public RotationalLimitMotorDefinition YRotMotor
        {
            get
            {
                return yRotMotor;
            }
        }

        public RotationalLimitMotorDefinition ZRotMotor
        {
            get
            {
                return zRotMotor;
            }
        }

        internal static Generic6DofConstraintDefinition Create(String name, EditUICallback callback)
        {
            return new Generic6DofConstraintDefinition(name);
        }

        protected Generic6DofConstraintDefinition(LoadInfo info)
            :base(info)
        {
	        translationMotor = info.GetValue<TranslationalLimitMotorDefinition>("TranslationMotor");
	        xRotMotor = info.GetValue<RotationalLimitMotorDefinition>("XRotMotor");
	        yRotMotor = info.GetValue<RotationalLimitMotorDefinition>("YRotMotor");
	        zRotMotor = info.GetValue<RotationalLimitMotorDefinition>("ZRotMotor");
        }

        public override void getInfo(SaveInfo info)
        {
	        base.getInfo(info);
	        info.AddValue("TranslationMotor", translationMotor);
	        info.AddValue("XRotMotor", xRotMotor);
	        info.AddValue("YRotMotor", yRotMotor);
	        info.AddValue("ZRotMotor", zRotMotor);
        }
    }
}
