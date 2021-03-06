﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Logging;
using Engine.Saving;
using Engine.Command;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is a definiton class for a SimSubScene.
    /// </summary>
    public class SimSceneDefinition : Saveable
    {
        #region Fields

        private List<SimElementManagerDefinition> elementManagers = new List<SimElementManagerDefinition>();
        private Dictionary<String, SimSubSceneDefinition> subSceneDefinitions = new Dictionary<string, SimSubSceneDefinition>();
        private EditInterface editInterface;
        private EditInterface simElementEditor;
        private EditInterface subScenes;

        private String defaultScene;
        
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SimSceneDefinition()
        {
            
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add a SimElementManagerDefinition.
        /// </summary>
        /// <param name="def">The definition to add.</param>
        public void addSimElementManagerDefinition(SimElementManagerDefinition def)
        {
            elementManagers.Add(def);
            if (editInterface != null)
            {
                createEditInterface(def);
            }
        }

        /// <summary>
        /// Remove a SimElementManagerDefinition.
        /// </summary>
        /// <param name="def">The definition to remove.</param>
        public void removeSimElementManagerDefinition(SimElementManagerDefinition def)
        {
            elementManagers.Remove(def);
            if (editInterface != null)
            {
                simElementEditor.removeSubInterface(def);
            }
        }

        /// <summary>
        /// Get a SimElementManagerDefinition specified by name. Will return
        /// null if it cannot be found.
        /// </summary>
        /// <param name="name">The name of the definition to find.</param>
        /// <returns>The matching defintion or null if it cannot be found.</returns>
        public SimElementManagerDefinition getSimElementManagerDefinition(String name)
        {
            foreach (SimElementManagerDefinition def in elementManagers)
            {
                if (def.Name == name)
                {
                    return def;
                }
            }
            return null;
        }

        /// <summary>
        /// Check to see if a SimElementManagerDefinition is part of this
        /// definition.
        /// </summary>
        /// <param name="name">The name of the definition.</param>
        /// <returns>True if the name is one of the definitons for this scene.</returns>
        public bool hasSimElementManagerDefinition(String name)
        {
            foreach (SimElementManagerDefinition def in elementManagers)
            {
                if (def.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add a SimSubSceneDefinition.
        /// </summary>
        /// <param name="def">The definition to add.</param>
        public void addSimSubSceneDefinition(SimSubSceneDefinition def)
        {
            subSceneDefinitions.Add(def.Name, def);
            def.setScene(this);
            if (editInterface != null)
            {
                createEditInterface(def);
            }
        }

        /// <summary>
        /// Remove a SimSubSceneDefinition.
        /// </summary>
        /// <param name="def">The definition to remove.</param>
        public void removeSimSubSceneDefinition(SimSubSceneDefinition def)
        {
            subSceneDefinitions.Remove(def.Name);
            def.setScene(null);
            if (editInterface != null)
            {
                subScenes.removeSubInterface(def);
            }
        }

        /// <summary>
        /// Determine if this SimSceneDefinition has any subSceneDefinitions.
        /// </summary>
        /// <returns>True if there are some SubSceneDefinitions.</returns>
        public bool hasSimSubSceneDefinitions()
        {
            return subSceneDefinitions.Count != 0;
        }

        /// <summary>
        /// Check to see if a named definition exists.
        /// </summary>
        /// <param name="name">The name to check for.</param>
        /// <returns>True if the definition is part of this class.</returns>
        public bool hasSimSubSceneDefinition(String name)
        {
            return subSceneDefinitions.ContainsKey(name);
        }

        /// <summary>
        /// Get the EditInterface for this SimSceneDefinition.
        /// </summary>
        /// <returns>The EditInterface for this SimSceneDefinition.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, ReflectedEditInterface.DefaultScanner, "Sim Scene", () =>
                    {
                        if (hasSimSubSceneDefinitions())
                        {
                            if (DefaultSubScene == null)
                            {
                                throw new ValidationException("Please specify one of the Subscenes as the default.");
                            }
                            if (!hasSimSubSceneDefinition(DefaultSubScene))
                            {
                                throw new ValidationException("{0} is not a valid Subscene. Please specify an existing scene.", DefaultSubScene);
                            }
                        }
                    });
                editInterface.IconReferenceTag = EngineIcons.Scene;

                simElementEditor = new EditInterface("Sim Element Managers");
                var elementManagerInterfaces = simElementEditor.createEditInterfaceManager<SimElementManagerDefinition>();
                elementManagerInterfaces.addCommand(new EditInterfaceCommand("Destroy", destroySimElementManagerDefinition));
                foreach (AddSimElementManagerCommand command in PluginManager.Instance.getCreateSimElementManagerCommands())
                {
                    simElementEditor.addCommand(new EditInterfaceCommand(command.Name, callback =>
                    {
                        callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
                        {
                            if (input == null || input == "")
                            {
                                errorPrompt = "Please enter a non empty name.";
                                return false;
                            }
                            if (this.hasSimElementManagerDefinition(input))
                            {
                                errorPrompt = "That name is already in use. Please provide another.";
                                return false;
                            }

                            SimElementManagerDefinition def = command.execute(input, callback);
                            this.addSimElementManagerDefinition(def);

                            return true;
                        });
                    }));
                }
                editInterface.addSubInterface(simElementEditor);

                subScenes = new EditInterface("Subscenes");
                var subSceneInterfaces = subScenes.createEditInterfaceManager<SimSubSceneDefinition>();
                subSceneInterfaces.addCommand(new EditInterfaceCommand("Destroy", destroySimSubSceneDefinition));
                EditInterfaceCommand createSubSceneCommand = new EditInterfaceCommand("Create Subscene", createSimSubSceneDefinition);
                subScenes.addCommand(createSubSceneCommand);
                editInterface.addSubInterface(subScenes);

                foreach (SimElementManagerDefinition elementDef in elementManagers)
                {
                    createEditInterface(elementDef);
                }
                foreach (SimSubSceneDefinition subSceneDef in subSceneDefinitions.Values)
                {
                    createEditInterface(subSceneDef);
                }
            }
            return editInterface;
        }

        /// <summary>
        /// Create and return a new SimScene from this definition.
        /// </summary>
        /// <returns>A new scene configured like this definition.</returns>
        public SimScene createScene()
        {
            SimScene scene = new SimScene();
            foreach (SimElementManagerDefinition elementManagerDef in elementManagers)
            {
                scene.addSimElementManager(elementManagerDef.createSimElementManager());
            }
            foreach (SimSubSceneDefinition subSceneDef in subSceneDefinitions.Values)
            {
                subSceneDef.createSubScene(scene);
            }
            if (DefaultSubScene != null)
            {
                SimSubScene subScene = scene.getSubScene(DefaultSubScene);
                if (subScene != null)
                {
                    scene.setDefaultSubScene(subScene);
                }
                else
                {
                    Log.Default.sendMessage("The defined default scene {0} can not be found in the created scene. No default set.", LogLevel.Warning, "Engine", DefaultSubScene);
                }
            }
            else
            {
                Log.Default.sendMessage("No default scene defined. No default set.", LogLevel.Warning, "Engine");
            }
            return scene;
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The name of the SimSubScene to use as the default scene for creating SimObjects.
        /// </summary>
        [Editable("The name of the SimSubScene to use as the default scene for creating SimObjects.")]
        public String DefaultSubScene
        {
            get
            {
                return defaultScene;
            }
            set
            {
                defaultScene = value;
            }
        }

        #endregion Properties

        #region Helper Functions

        /// <summary>
        /// Callback to destroy a SimElementManagerDefinition.
        /// </summary>
        /// <param name="callback">The EditUICallback to get more info from the user.</param>
        /// <param name="caller">The command that initiated this funciton call.</param>
        private void destroySimElementManagerDefinition(EditUICallback callback)
        {
            EditInterface currentInterface = callback.getSelectedEditInterface();
            removeSimElementManagerDefinition(simElementEditor.resolveSourceObject<SimElementManagerDefinition>(currentInterface));
        }

        /// <summary>
        /// Callback to create a SimSubSceneDefinition.
        /// </summary>
        /// <param name="callback">The EditUICallback to get more info from the user.</param>
        /// <param name="caller">The command that initiated this funciton call.</param>
        private void createSimSubSceneDefinition(EditUICallback callback)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
            {
                if (input == null || input == "")
                {
                    errorPrompt = "Please enter a non empty name.";
                    return false;
                }
                if (this.hasSimSubSceneDefinition(input))
                {
                    errorPrompt = "That name is already in use. Please provide another.";
                    return false;
                }

                SimSubSceneDefinition def = new SimSubSceneDefinition(input);
                this.addSimSubSceneDefinition(def);
                return true;
            });
        }

        /// <summary>
        /// Callback to destroy a SimSubSceneDefinition.
        /// </summary>
        /// <param name="callback">The EditUICallback to get more info from the user.</param>
        /// <param name="caller">The command that initiated this funciton call.</param>
        private void destroySimSubSceneDefinition(EditUICallback callback)
        {
            EditInterface currentInterface = callback.getSelectedEditInterface();
            removeSimSubSceneDefinition(subScenes.resolveSourceObject<SimSubSceneDefinition>(currentInterface));
        }

        /// <summary>
        /// Helper function to create an edit interface.
        /// </summary>
        /// <param name="def"></param>
        private void createEditInterface(SimElementManagerDefinition def)
        {
            simElementEditor.addSubInterface(def, def.getEditInterface());
        }

        /// <summary>
        /// Helper function to create an edit interface.
        /// </summary>
        /// <param name="def"></param>
        private void createEditInterface(SimSubSceneDefinition def)
        {
            subScenes.addSubInterface(def, def.getEditInterface());
        }

        #endregion Helper Functions

        #region Saveable Members

        private const String ELEMENT_MANAGERS_BASE = "ElementManager";
        private const String SUB_SCENE_BASE = "SubScene";
        private const String DEFAULT_SUB_SCENE = "DefaultSubScene";

        private SimSceneDefinition(LoadInfo info)
        {
            DefaultSubScene = info.GetString(DEFAULT_SUB_SCENE);
            for (int i = 0; info.hasValue(ELEMENT_MANAGERS_BASE + i); i++)
            {
                addSimElementManagerDefinition(info.GetValue<SimElementManagerDefinition>(ELEMENT_MANAGERS_BASE + i));
            }
            for (int i = 0; info.hasValue(SUB_SCENE_BASE + i); i++)
            {
                addSimSubSceneDefinition(info.GetValue<SimSubSceneDefinition>(SUB_SCENE_BASE + i));
            }
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(DEFAULT_SUB_SCENE, DefaultSubScene);
            int i = 0;
            foreach (SimElementManagerDefinition manager in elementManagers)
            {
                info.AddValue(ELEMENT_MANAGERS_BASE + i++, manager);
            }
            i = 0;
            foreach (SimSubSceneDefinition subScene in subSceneDefinitions.Values)
            {
                info.AddValue(SUB_SCENE_BASE + i++, subScene);
            }
        }

        #endregion
    }
}
