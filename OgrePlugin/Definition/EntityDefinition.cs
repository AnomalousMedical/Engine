using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;
using Engine.Reflection;
using OgreWrapper;
using Engine.ObjectManagement;
using Logging;

namespace OgrePlugin
{
    /// <summary>
    /// This is a definition class for Entities.
    /// </summary>
    public class EntityDefinition : Saveable
    {
        #region Static

        private static MemberScanner memberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static EntityDefinition()
        {
            memberScanner = new MemberScanner();
            memberScanner.ProcessFields = false;
            memberScanner.Filter = EditableAttributeFilter.Instance;
        }

        #endregion Static

        private String name;
        private SkeletonInfo skeleton = null;
        private String meshName;
        private EditInterface editInterface = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public EntityDefinition(String name)
        {
            this.name = name;
        }

        /// <summary>
        /// Get an EditInterface for this entity.
        /// </summary>
        /// <returns>An EditInterface for this entity.</returns>
        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, name + "Entity", null);
            }
            return editInterface;
        }

        /// <summary>
        /// Create the product for this entity.
        /// </summary>
        /// <param name="element">The SceneNodeElement to add the definition to so it can be destroyed properly.</param>
        /// <param name="scene">The OgreSceneManager that will get the entity.</param>
        /// <param name="simObject">The SimObject that will get the entity.</param>
        internal void createProduct(SceneNodeElement element, OgreSceneManager scene, SimObject simObject)
        {
            if (OgreResourceGroupManager.getInstance().findGroupContainingResource(meshName) != null)
            {
                Identifier identifier = new Identifier(simObject.Name, name);
                Entity entity = scene.createEntity(identifier, this);
                if (entity.hasSkeleton() && skeleton != null)
                {
                    skeleton.initialzeSkeleton(entity.getSkeleton());
                }
                element.addEntity(identifier, entity);
            }
            else
            {
                Log.Default.sendMessage("Cannot create entity {0} because the mesh {1} cannot be found.", LogLevel.Warning, OgreInterface.PluginName, name, meshName);
            }
        }

        /// <summary>
        /// The name of the mesh to use for this entity.
        /// </summary>
        [Editable("The name of the mesh to use for this entity.", typeof(Mesh))]
        public String MeshName
        {
            get
            {
                return meshName;
            }
            set
            {
                meshName = value;
            }
        }

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

        #region Saveable Members

        private const String NAME = "Name";
        private const String MESH_NAME = "MeshName";
        private const String SKELETON = "Skeleton";

        private EntityDefinition(LoadInfo info)
        {
            name = info.GetString(NAME);
            meshName = info.GetString(MESH_NAME);
            if (info.hasValue(SKELETON))
            {
                skeleton = info.GetValue<SkeletonInfo>(SKELETON);
            }
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(NAME, name);
            info.AddValue(MESH_NAME, meshName);
            info.AddValue(SKELETON, skeleton);
        }

        #endregion
    }
}
