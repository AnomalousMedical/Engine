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
    /// <summary>
    /// This is the default behavior definition that will be used. It will
    /// provide behavior data as described by the Behavior class.
    /// </summary>
    public class BehaviorDefinition : SimElementDefinition
    {
        #region Static

        private static BehaviorBrowser behaviorBrowser = null;

        /// <summary>
        /// Static create function.
        /// </summary>
        /// <param name="name">The name of the Definition to create.</param>
        /// <param name="callback">A UICallback.</param>
        /// <returns></returns>
        internal static void Create(String name, EditUICallback callback, CompositeSimObjectDefinition simObjectDef)
        {
            if (behaviorBrowser == null)
            {
                behaviorBrowser = new BehaviorBrowser();
            }
            callback.showBrowser<Type>(behaviorBrowser, delegate(Type behaviorType, ref String errorMessage)
            {
                if (behaviorType != null)
                {
                    simObjectDef.addElement(new BehaviorDefinition(name, (Behavior)Activator.CreateInstance(behaviorType)));
                    return true;
                }
                return false;
            });
        }

        #endregion Static

        private Behavior behaviorTemplate;
        private EditInterface editInterface;

        public BehaviorDefinition(String name, Behavior behaviorTemplate)
            : base(name)
        {
            this.behaviorTemplate = behaviorTemplate;
        }

        public override void registerScene(SimSubScene subscene, SimObjectBase instance)
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

        protected override EditInterface createEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(behaviorTemplate, BehaviorEditMemberScanner.Scanner, Name + " - " + behaviorTemplate.GetType().Name, null);
                behaviorTemplate.callCustomizeEditInterface(editInterface);
                editInterface.IconReferenceTag = EngineIcons.Behavior;
            }
            return editInterface;
        }

        internal Behavior createProduct(SimObjectBase instance, BehaviorManager behaviorManager)
        {
            Behavior behavior = MemberCopier.CreateCopy<Behavior>(behaviorTemplate);
            behavior.setAttributes(Name, subscription, behaviorManager);
            instance.addElement(behavior);
            return behavior;
        }

        #region Saveable

        private const String NAME_FORMAT = "{0}, {1}";
        private String BEHAVIOR_TYPE = "BehaviorDataType";

        private BehaviorDefinition(LoadInfo info)
            :base(info)
        {
            String behaviorType = info.GetString(BEHAVIOR_TYPE);
            Type type = info.TypeFinder.findType(behaviorType);
            if (type != null)
            {
                behaviorTemplate = (Behavior)Activator.CreateInstance(type);
                ReflectedSaver.RestoreObject(behaviorTemplate, info, BehaviorSaveMemberScanner.Scanner);
                behaviorTemplate.callCustomLoad(info);
            }
            else
            {
                throw new BehaviorException(String.Format("Could not load behavior of type {0}. The type could not be found.", behaviorType));
            }
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue(BEHAVIOR_TYPE, createShortTypeString(behaviorTemplate.GetType()));
            ReflectedSaver.SaveObject(behaviorTemplate, info, BehaviorSaveMemberScanner.Scanner);
            behaviorTemplate.callCustomSave(info);
        }

        private static String createShortTypeString(Type type)
        {
            String shortAssemblyName = type.Assembly().FullName;
            return String.Format(NAME_FORMAT, type.FullName, shortAssemblyName.Remove(shortAssemblyName.IndexOf(',')));
        }

        #endregion Saveable
    }
}
