using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using Logging;
using Engine.Saving;

namespace Engine
{
    public class BehaviorDefinition : SimElementDefinition
    {
        internal static BehaviorDefinition Create(String name)
        {
            return new BehaviorDefinition(name);
        }

        private Type behaviorType;
        EditInterface editInterface;

        public BehaviorDefinition(String name)
            :base(name)
        {

        }

        public BehaviorDefinition(Behavior behavior)
            :base(behavior.Name)
        {
            behaviorType = behavior.GetType();
        }

        //temp
        public BehaviorDefinition(Type behaviorType, String name)
            :base(name)
        {
            this.behaviorType = behaviorType;
        }

        public override void register(SimSubScene subscene, SimObject instance)
        {
            if (subscene.hasSimElementManagerType(typeof(BehaviorManager)))
            {
                BehaviorManager behaviorManager = subscene.getSimElementManager<BehaviorManager>();
                behaviorManager.getBehaviorFactory().addBehaviorDefinition(instance, this);
            }
            else
            {
                Log.Default.sendMessage("Cannot add BehaviorDefinition {0} to SimSubScene {1} because it does not contain a BehaviorManager.", LogLevel.Warning, "Behavior", Name, subscene.Name);
            }
        }

        public override EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(Name + " Behavior");
            }
            return editInterface;
        }

        internal Behavior createProduct(SimObject instance, BehaviorManager behaviorManager)
        {
            Behavior behavior = (Behavior)Activator.CreateInstance(behaviorType);
            behavior.setAttributes(Name, subscription, behaviorManager);
            instance.addElement(behavior);
            return behavior;
        }

        internal void createStaticProduct(SimObject instance, BehaviorManager scene)
        {
            throw new NotImplementedException();
        }

        #region Saveable

        private const string BEHAVIOR_TYPE = "Type";

        private BehaviorDefinition(LoadInfo info)
            :base(info)
        {
            String behaviorTypeName = info.GetString(BEHAVIOR_TYPE);
            behaviorType = Type.GetType(behaviorTypeName);
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue(BEHAVIOR_TYPE, behaviorType.AssemblyQualifiedName);
        }

        #endregion Saveable
    }
}
