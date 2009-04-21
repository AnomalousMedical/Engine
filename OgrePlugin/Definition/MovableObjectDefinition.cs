using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;
using Engine.Reflection;
using OgreWrapper;
using Engine.ObjectManagement;

namespace OgrePlugin
{
    /// <summary>
    /// This is a definition class for Ogre MovableObjects such as Entities or
    /// Lights.
    /// </summary>
    public abstract class MovableObjectDefinition : Saveable
    {
        #region Static

        private static MemberScanner memberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static MovableObjectDefinition()
        {
            memberScanner = new MemberScanner();
            memberScanner.ProcessFields = false;
            memberScanner.Filter = EditableAttributeFilter.Instance;
        }

        #endregion Static

        private String name;
        private EditInterface editInterface;
        private String interfaceName;
        private Validate validateCallback;

        protected MovableObjectDefinition(String name, String interfaceName, Validate validateCallback)
        {
            this.name = name;
            this.interfaceName = interfaceName;
            this.validateCallback = validateCallback;
        }

        protected MovableObjectDefinition(String name, MovableObject movableObject, String interfaceName, Validate validateCallback)
        {
            this.interfaceName = interfaceName;
            this.validateCallback = validateCallback;
            RenderQueue = movableObject.getRenderQueueGroup();
            this.name = name;
        }

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, typeof(MovableObjectDefinition), memberScanner, name + interfaceName, validateCallback);
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
        /// <param name="element">The SceneNodeElement to add the definition to so it can be destroyed properly.</param>
        /// <param name="scene">The OgreSceneManager that will get the entity.</param>
        /// <param name="simObject">The SimObject that will get the entity.</param>
        internal void createProduct(SceneNodeElement element, OgreSceneManager scene, SimObject simObject)
        {
            MovableObject movable = createActualProduct(element, scene, simObject);
            if (movable != null)
            {
                movable.setRenderQueueGroup(RenderQueue);
            }
        }

        /// <summary>
        /// This funciton will build the MovableObject and return it to this
        /// class so it can be configured with all of the MovableObject
        /// properties.
        /// </summary>
        /// <param name="element">The SceneNodeElement to add the definition to so it can be destroyed properly.</param>
        /// <param name="scene">The OgreSceneManager that will get the entity.</param>
        /// <param name="simObject">The SimObject that will get the entity.</param>
        /// <returns>The newly created MovableObject or null if there was an error.</returns>
        protected abstract MovableObject createActualProduct(SceneNodeElement element, OgreSceneManager scene, SimObject simObject);

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

        [Editable]
        public byte RenderQueue { get; set; }
        
        #region Saveable Members

        private const String NAME = "Name";
        private const String RENDER_QUEUE = "RenderQueue";

        protected MovableObjectDefinition(LoadInfo info, String interfaceName, Validate validateCallback)
        {
            this.interfaceName = interfaceName;
            this.validateCallback = validateCallback;
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
