using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;
using Engine.Reflection;
using OgreWrapper;
using Engine.ObjectManagement;
using Engine.Attributes;

namespace OgrePlugin
{
    /// <summary>
    /// This is a definition class for Ogre MovableObjects such as Entities or
    /// Lights.
    /// </summary>
    public abstract class MovableObjectDefinition : Saveable
    {
        #region Static

        private static FilteredMemberScanner memberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static MovableObjectDefinition()
        {
            memberScanner = new FilteredMemberScanner();
            memberScanner.ProcessFields = false;
            memberScanner.Filter = new EditableAttributeFilter();
        }

        #endregion Static

        private String name;
        private EditInterface editInterface;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the MovableObject.</param>
        protected MovableObjectDefinition(String name)
        {
            this.name = name;
            RenderQueue = 50;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the MovableObject.</param>
        /// <param name="movableObject">An existing movableObject to take parameters from.</param>
        protected MovableObjectDefinition(String name, MovableObject movableObject)
        {
            RenderQueue = movableObject.getRenderQueueGroup();
            this.name = name;
        }

        /// <summary>
        /// Get the EditInterface for the MovableObject.
        /// </summary>
        /// <returns>The EditInterface.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, typeof(MovableObjectDefinition), memberScanner, String.Format("{0} - {1}", name, InterfaceName), null);
                setupEditInterface(editInterface);
            }
            return editInterface;
        }

        /// <summary>
        /// This function will fill out the EditInterface with the rest of the
        /// properties for this MovableObject.
        /// </summary>
        /// <param name="editInterface">The EditInterface to fill out.</param>
        protected abstract void setupEditInterface(EditInterface editInterface);

        /// <summary>
        /// Create the product for this movableobject.
        /// </summary>
        /// <param name="scene">The scene to create the product into.</param>
        /// <param name="baseName">The base name of the MovableObject.</param>
        /// <returns>The newly created MovableObjectContainer or null if there was an error.</returns>
        internal MovableObjectContainer createProduct(OgreSceneManager scene, String baseName)
        {
            MovableObjectContainer movable = createActualProduct(scene, baseName);
            if (movable != null)
            {
                movable.MovableObject.setRenderQueueGroup(RenderQueue);
            }
            return movable;
        }

        /// <summary>
        /// This funciton will build the MovableObject and return it to this
        /// class so it can be configured with all of the MovableObject
        /// properties. 
        /// <para>
        /// DO NOT call this method outside of the
        /// MovableObjectDefinition. It must be internal because you cannot be
        /// internal and protected.
        /// </para>
        /// </summary>
        /// <param name="scene">The scene to create the product into.</param>
        /// <param name="name">The base name of the MovableObject.</param>
        /// <returns>The newly created MovableObjectContainer or null if there was an error.</returns>
        internal abstract MovableObjectContainer createActualProduct(OgreSceneManager scene, String baseName);

        /// <summary>
        /// The name of this entity.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }

        protected abstract String InterfaceName { get; }

        /// <summary>
        /// The RenderQueue this MovableObject belongs to.
        /// </summary>
        [Editable]
        public byte RenderQueue { get; set; }
        
        #region Saveable Members

        private const String NAME = "Name";
        private const String RENDER_QUEUE = "RenderQueue";

        protected MovableObjectDefinition(LoadInfo info)
        {
            name = info.GetString(NAME);
            RenderQueue = info.GetByte(RENDER_QUEUE);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, name);
            info.AddValue(RENDER_QUEUE, RenderQueue);
            getSpecificInfo(info);
        }

        /// <summary>
        /// Get the info to save for the subclass.
        /// </summary>
        /// <param name="info">The info to fill out.</param>
        protected abstract void getSpecificInfo(SaveInfo info);

        #endregion
    }
}
