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
    public class EntityDefinition : MovableObjectDefinition
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
            EditableAttributeFilter filter = new EditableAttributeFilter();
            filter.TerminatingType = typeof(MovableObjectDefinition);
            memberScanner.Filter = filter;
        }

        #endregion Static

        private SkeletonInfo skeleton = null;
        private String meshName;

        /// <summary>
        /// Parameterless constructor for copy class.
        /// </summary>
        public EntityDefinition()
            :base("", "Entity", null)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public EntityDefinition(String name)
            :base(name, "Entity", null)
        {
            
        }

        public EntityDefinition(String name, Entity entity)
            :base(name, entity, "Entity", null)
        {
            using (MeshPtr mesh = entity.getMesh())
            {
                meshName = mesh.Value.getName();
            }
            if (entity.hasSkeleton())
            {
                skeleton = new SkeletonInfo();
                skeleton.saveSkeleton(entity.getSkeleton());
            }
        }

        /// <summary>
        /// This function will fill out the EditInterface with the rest of the
        /// properties for this MovableObject.
        /// </summary>
        /// <param name="editInterface">The EditInterface to fill out.</param>
        protected override void setupEditInterface(EditInterface editInterface)
        {
            ReflectedEditInterface.expandEditInterface(this, memberScanner, editInterface);
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
        protected override MovableObject createActualProduct(SceneNodeElement element, OgreSceneManager scene, SimObjectBase simObject)
        {
            if (OgreResourceGroupManager.getInstance().findGroupContainingResource(meshName) != null)
            {
                Identifier identifier = new Identifier(simObject.Name, Name);
                Entity entity = scene.createEntity(identifier, this);
                if (entity.hasSkeleton() && skeleton != null)
                {
                    skeleton.initialzeSkeleton(entity.getSkeleton());
                }
                element.attachObject(identifier, entity);
                return entity;
            }
            else
            {
                Log.Default.sendMessage("Cannot create entity {0} because the mesh {1} cannot be found.", LogLevel.Warning, OgreInterface.PluginName, Name, meshName);
                element.addInvalidDefinition(this);
                return null;
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

        #region Saveable Members

        private const String MESH_NAME = "MeshName";
        private const String SKELETON = "Skeleton";

        /// <summary>
        /// Deserialize constructor.
        /// </summary>
        /// <param name="info"></param>
        private EntityDefinition(LoadInfo info)
            :base(info, "Entity", null)
        {
            meshName = info.GetString(MESH_NAME);
            if (info.hasValue(SKELETON))
            {
                skeleton = info.GetValue<SkeletonInfo>(SKELETON);
            }
        }

        /// <summary>
        /// Get the info to save for the subclass.
        /// </summary>
        /// <param name="info">The info to fill out.</param>
        protected override void getSpecificInfo(SaveInfo info)
        {
            info.AddValue(MESH_NAME, meshName);
            info.AddValue(SKELETON, skeleton);
        }

        #endregion
    }
}
