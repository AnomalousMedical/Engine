﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is a definition base class for SimElements.
    /// </summary>
    public abstract class SimElementDefinition : Saveable
    {   
        #region Fields

        private String name;
        protected SimObjectDefinition simObjectDef;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Takes the name of the element.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        public SimElementDefinition(String name)
        {
            this.name = name;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Register this element with its factory so it can be built.
        /// </summary>
        /// <param name="subscene">The SimSubScene that will get the built product.</param>
        /// <param name="instance">The SimObject that will get the newly created element.</param>
        public abstract void registerScene(SimSubScene subscene, SimObjectBase instance);

        /// <summary>
        /// Get an EditInterface for the SimElementDefinition so it can be
        /// modified.
        /// </summary>
        /// <returns>The EditInterface for this SimElementDefinition.</returns>
        public EditInterface getEditInterface()
        {
            EditInterface editInterface = createEditInterface();

            GenericClipboardEntry clipboardEntry = new GenericClipboardEntry(typeof(SimElementDefinition));
            clipboardEntry.CopyFunction = copy;
            clipboardEntry.PasteFunction = paste;
            editInterface.ClipboardEntry = clipboardEntry;

            return editInterface;
        }

        protected abstract EditInterface createEditInterface();

        /// <summary>
        /// Set the SimObjectDefinition for this element.
        /// </summary>
        /// <param name="simObjectDef">The definition to set.</param>
        internal void setSimObjectDefinition(SimObjectDefinition simObjectDef)
        {
            this.simObjectDef = simObjectDef;
        }

        protected Object copy()
        {
            return EngineClipboard.copyObject(this);
        }

        protected void paste(Object pasted)
        {
            if (simObjectDef != null)
            {
                simObjectDef.pasteElement((SimElementDefinition)pasted);
            }
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// Get the name of this SimElement.
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

        private const string NAME = "SimElementName";

        /// <summary>
        /// Deserialize constructor.
        /// </summary>
        /// <param name="loadInfo">The LoadInfo that contains the deserialized data.</param>
        protected SimElementDefinition(LoadInfo loadInfo)
        {
            name = loadInfo.GetString(NAME);
        }

        /// <summary>
        /// Save the fields of this class to the serialization stream.
        /// </summary>
        /// <param name="info">The info to fill out.</param>
        public virtual void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, name);
        }

        #endregion
    }
}
