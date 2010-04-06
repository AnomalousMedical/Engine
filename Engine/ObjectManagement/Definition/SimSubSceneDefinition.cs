using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Logging;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is a definition for a SimSubScene.
    /// </summary>
    public class SimSubSceneDefinition : Saveable
    {
        #region Fields

        private LinkedList<SimSubSceneBinding> bindings = new LinkedList<SimSubSceneBinding>();
        private String name;
        private SimSceneDefinition scene;
        private EditInterface editInterface;

        #endregion Fields

        #region Constructors

        public SimSubSceneDefinition(String name)
        {
            this.name = name;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Get the SimElementManagerDefinition indicated by name. This will
        /// return null if there is no defintion with the given name.
        /// </summary>
        /// <param name="name">The name to test for.</param>
        /// <returns>The matching definition or null if it cannot be found.</returns>
        public SimElementManagerDefinition getSimElementManager(String name)
        {
            return scene.getSimElementManagerDefinition(name);
        }

        /// <summary>
        /// Determine if the given SimElementManager type is already contained
        /// in this SimSubSceneDefinition. SimSubScenes can only have one of
        /// each type of SimElementManager in them.
        /// </summary>
        /// <param name="check">The SimElementManagerDefinition to check types of.</param>
        /// <returns>True if this definition already contains a SimElementManagerDefinition of the given type.</returns>
        public bool hasTypeBindings(SimElementManagerDefinition check)
        {
            foreach (SimSubSceneBinding binding in bindings)
            {
                if (binding.SimElementManager != null && binding.SimElementManager.getSimElementManagerType() == check.getSimElementManagerType())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add a binding.
        /// </summary>
        /// <param name="toBind">The SimElementManagerDefinition to bind to this SimSubScene</param>
        public void addBinding(SimElementManagerDefinition toBind)
        {
            SimSubSceneBinding binding = new SimSubSceneBinding(this, toBind.Name);
            bindings.AddLast(binding);
            if (editInterface != null)
            {
                editInterface.addEditableProperty(binding);
            }
        }

        /// <summary>
        /// Remove a binding.
        /// </summary>
        /// <param name="toBind">The SimElementManagerDefinition to remove from this SimSubScene</param>
        public void removeBinding(SimElementManagerDefinition toBind)
        {
            SimSubSceneBinding found = null;
            foreach (SimSubSceneBinding binding in bindings)
            {
                if (binding.SimElementManager == toBind)
                {
                    found = binding;
                    break;
                }
            }
            if (found != null)
            {
                bindings.Remove(found);
                if (editInterface != null)
                {
                    editInterface.removeEditableProperty(found);
                }
            }
        }

        /// <summary>
        /// Get the EditInterface.
        /// </summary>
        /// <returns>The EditInterface.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name + " Subscene", addBinding, removeBinding, validate);
                EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
                propertyInfo.addColumn(new EditablePropertyColumn("Name", false));
                propertyInfo.addColumn(new EditablePropertyColumn("Type", true));
                editInterface.setPropertyInfo(propertyInfo);
                foreach (SimSubSceneBinding binding in bindings)
                {
                    editInterface.addEditableProperty(binding);
                }
            }
            return editInterface;
        }

        /// <summary>
        /// Create a new SimSubScene and add it to scene.
        /// </summary>
        /// <param name="scene">The scene to add the sub scene to.</param>
        /// <returns>The newly created sub scene.</returns>
        public SimSubScene createSubScene(SimScene scene)
        {
            SimSubScene subscene = new SimSubScene(Name, scene);
            foreach (SimSubSceneBinding elementManager in bindings)
            {
                SimElementManager manager = scene.getSimElementManager(elementManager.SimElementManager.Name);
                if (manager != null)
                {
                    subscene.addSimElementManager(manager);
                }
                else
                {
                    Log.Default.sendMessage("Could not find SimElementManager called {0}. This has not been added to the scene.", LogLevel.Warning, "Engine", elementManager);
                }
            }
            scene.addSimSubScene(subscene);
            return subscene;
        }

        /// <summary>
        /// Set the scene that this definition belongs to. This should only be
        /// called by SimSceneDefiniton.
        /// </summary>
        /// <param name="scene"></param>
        internal void setScene(SimSceneDefinition scene)
        {
            this.scene = scene;
        }

        private void addBinding(EditUICallback callback)
        {
            SimSubSceneBinding binding = new SimSubSceneBinding(this);
            bindings.AddLast(binding);
            if (editInterface != null)
            {
                editInterface.addEditableProperty(binding);
            }
        }

        private void removeBinding(EditUICallback callback, EditableProperty property)
        {
            bindings.Remove((SimSubSceneBinding)property);
            if (editInterface != null)
            {
                editInterface.removeEditableProperty(property);
            }
        }

        private bool validate(out String message)
        {
            foreach (SimSubSceneBinding binding in bindings)
            {
                if (binding.SimElementManager == null)
                {
                    message = "Empty binding found. Please fill out all bindings or remove the empty listings.";
                    return false;
                }
            }
            message = null;
            return true;
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The name of the SubScene.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }

        #endregion Properties

        #region Saveable Members

        private const string NAME = "Name";
        private const string BINDINGS_BASE = "Binding";

        private SimSubSceneDefinition(LoadInfo info)
        {
            name = info.GetString(NAME);

            for (int i = 0; info.hasValue(BINDINGS_BASE + i); i++)
            {
                bindings.AddLast(new SimSubSceneBinding(this, info.GetValue<SimElementManagerDefinition>(BINDINGS_BASE + i)));
            }
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, Name);
            int i = 0;
            foreach (SimSubSceneBinding binding in bindings)
            {
                info.AddValue(BINDINGS_BASE + i++, binding.SimElementManager);
            }
        }

        #endregion
    }
}
