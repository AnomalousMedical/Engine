using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
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

        #region EditableProperty Members

        /// <summary>
        /// Get the value for a given column.
        /// </summary>
        /// <param name="column">The column to get the value for.</param>
        /// <returns></returns>
        public object getValue(int column)
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

        /// <summary>
        /// Set the value of this property.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value">The value to set. Must be the correct type.</param>
        public void setValue(int column, object value)
        {
            if (column == NAME_COL)
            {
                setValueStr(column, value.ToString());
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
                if (simElementManager != null)
                {
                    subScene.removeBinding(simElementManager);
                }
                simElementManager = subScene.getSimElementManager(value);
                if (simElementManager != null)
                {
                    subScene.addBinding(simElementManager);
                }
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
                if (def != simElementManager &&
                    (simElementManager == null || def.getSimElementManagerType() != simElementManager.getSimElementManagerType()))
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
  
        #endregion
    }
}
