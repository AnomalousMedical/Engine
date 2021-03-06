﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This class creates a binding between a SimSubScene and a given type of SimElementManager.
    /// </summary>
    public class SimSubSceneBinding : EditableProperty
    {
        private const int NAME_COL = 0;
        private const int TYPE_COL = 1;
        private const int NUM_COLS = 2;

        private SimSubSceneDefinition subScene;
        private SimElementManagerDefinition simElementManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public SimSubSceneBinding(SimSubSceneDefinition subScene)
        {
            this.subScene = subScene;
        }

        /// <summary>
        /// Constructor takes a name and a subScene.
        /// </summary>
        /// <param name="subScene">The SubScene to add bindings to.</param>
        /// <param name="managerName">The name of the SimElementManager.</param>
        public SimSubSceneBinding(SimSubSceneDefinition subScene, String managerName)
        {
            this.subScene = subScene;
            simElementManager = subScene.getSimElementManager(managerName);
        }

        /// <summary>
        /// Constructor takes a SubScene and a SimElementManagerDefinition.
        /// </summary>
        /// <param name="subScene">The SubSceneDefinition.</param>
        /// <param name="simElementManager">The SimElementManagerDefinition.</param>
        public SimSubSceneBinding(SimSubSceneDefinition subScene, SimElementManagerDefinition simElementManager)
        {
            this.subScene = subScene;
            this.simElementManager = simElementManager;
        }

        /// <summary>
        /// Get the value for a given column.
        /// </summary>
        /// <param name="column">The column to get the value for.</param>
        /// <returns></returns>
        public String getValue(int column)
        {
            if (simElementManager != null)
            {
                switch (column)
                {
                    case NAME_COL:
                        return simElementManager.Name;
                    case TYPE_COL:
                        return simElementManager.GetType().Name;
                }
            }
            return null;
        }

        public Object getRealValue(int column)
        {
            if (simElementManager != null)
            {
                switch (column)
                {
                    case NAME_COL:
                        return simElementManager.Name;
                    case TYPE_COL:
                        return simElementManager.GetType().Name;
                }
            }
            return null;
        }

        public void setValue(int column, Object value)
        {
            if (column == NAME_COL)
            {
                simElementManager = subScene.getSimElementManager((String)value);
            }
        }

        /// <summary>
        /// Set the value of this property from a string.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void setValueStr(int column, string value)
        {
            if (column == NAME_COL)
            {
                simElementManager = subScene.getSimElementManager(value);
            }
        }

        /// <summary>
        /// Determine if the given string is in the correct format for this
        /// property to parse.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value">The value to try to parse.</param>
        /// <param name="errorMessage">An error message if the function returns false.</param>
        /// <returns>True if the string can be parsed.</returns>
        public bool canParseString(int column, string value, out string errorMessage)
        {
            if (column == NAME_COL && value != null && value != String.Empty)
            {
                SimElementManagerDefinition def = subScene.getSimElementManager(value);
                if (simElementManager == null || def != simElementManager)
                {
                    if (def == null)
                    {
                        errorMessage = String.Format("Could not find a SimElementManagerDefinition named {0}", value);
                        return false;
                    }
                    if (subScene.hasTypeBindings(def))
                    {
                        errorMessage = String.Format("This SubSceneDefinition already has a binding for the type {0}", def.GetType().Name);
                        return false;
                    }
                }
            }
            errorMessage = null;
            return true;
        }

        /// <summary>
        /// Get the type of this property's target object.
        /// </summary>
        /// <param name="column"></param>
        /// <returns>The Type of the object this property will set.</returns>
        public Type getPropertyType(int column)
        {
            switch (column)
            {
                case NAME_COL:
                    return typeof(String);
                case TYPE_COL:
                    return typeof(String);
            }
            return null;
        }

        public Browser getBrowser(int column, EditUICallback uiCallback)
        {
            return null;
        }

        public bool hasBrowser(int column)
        {
            return false;
        }

        public bool readOnly(int column)
        {
            return column != NAME_COL;
        }

        /// <summary>
        /// Set this to true to indicate to the ui that this property is advanced.
        /// </summary>
        public bool Advanced { get; set; }

        /// <summary>
        /// The SimElementManagerDefinition for this binding.
        /// </summary>
        public SimElementManagerDefinition SimElementManager
        {
            get
            {
                return simElementManager;
            }
        }
    }
}
