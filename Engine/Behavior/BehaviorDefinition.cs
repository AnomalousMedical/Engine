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

        private BehaviorData data;
        EditInterface editInterface;

        public BehaviorDefinition(String name)
            :base(name)
        {

        }

        public BehaviorDefinition(String name, BehaviorData behaviorData)
            : base(name)
        {
            data = behaviorData;
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
            return data.getEditInterface();
        }

        internal Behavior createProduct(SimObject instance, BehaviorManager behaviorManager)
        {
            Behavior behavior = data.createNewInstance();
            behavior.setAttributes(Name, subscription, behaviorManager);
            instance.addElement(behavior);
            return behavior;
        }

        internal void createStaticProduct(SimObject instance, BehaviorManager scene)
        {
            throw new NotImplementedException();
        }

        #region Saveable

        private const string BEHAVIOR_DATA = "BehaviorData";

        private BehaviorDefinition(LoadInfo info)
            :base(info)
        {
            data = info.GetValue<BehaviorData>(BEHAVIOR_DATA);
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue(BEHAVIOR_DATA, data);
        }

        #endregion Saveable
    }
}
