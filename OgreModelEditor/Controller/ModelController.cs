using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using OgrePlugin;
using OgreWrapper;
using Engine;
using WeifenLuo.WinFormsUI.Docking;
using Logging;
using System.Diagnostics;

namespace OgreModelEditor.Controller
{
    /// <summary>
    /// This class handles the loading and unloading of the model.
    /// </summary>
    class ModelController : IDisposable
    {
        private GenericSimObjectDefinition simObjectDefinition;
        private EntityDefinition entityDefintion;
        private SceneNodeDefinition nodeDefinition;
        private SimObjectBase currentSimObject;
        private String entityMaterialName;
        private Entity entity;
        private MaterialPtr fixedFunctionTextured;
        private TextureUnitState fixedTexture;
        private LinkedList<String> modelTextures = new LinkedList<string>();
        private SelectableModel selectableModel = new SelectableModel();
        private bool showSkeleton = false;

        OgreModelEditorController controller;

        //GUI
        private SkeletonWindow skeletonWindow = new SkeletonWindow();
        private new CustomParameterControl customParameters = new CustomParameterControl();

        /// <summary>
        /// Constructor.
        /// </summary>
        public ModelController(OgreModelEditorController controller)
        {
            simObjectDefinition = new GenericSimObjectDefinition("EntitySimObject");
            simObjectDefinition.Enabled = true;
            entityDefintion = new EntityDefinition("Entity");
            nodeDefinition = new SceneNodeDefinition("EntityNode");
            nodeDefinition.addMovableObjectDefinition(entityDefintion);
            simObjectDefinition.addElement(nodeDefinition);
            fixedFunctionTextured = MaterialManager.getInstance().getByName("FixedFunctionTextured");
            fixedTexture = fixedFunctionTextured.Value.getTechnique(0).getPass(0).getTextureUnitState(0);
            this.controller = controller;
        }

        public DockContent getDockContent(String persistString)
        {
            if (skeletonWindow.GetType().ToString() == persistString)
            {
                return skeletonWindow;
            }
            if (customParameters.GetType().ToString() == persistString)
            {
                return customParameters;
            }
            return null;
        }

        public void createDefaultWindows()
        {
            controller.showDockContent(skeletonWindow);
            controller.showDockContent(customParameters);
        }

        public void Dispose()
        {
            fixedFunctionTextured.Dispose();
            skeletonWindow.Dispose();
        }

        /// <summary>
        /// Create a model.
        /// </summary>
        /// <param name="meshName">The name of the mesh to load.</param>
        /// <param name="scene">The scene to load the mesh into.</param>
        public void createModel(String meshName, SimScene scene)
        {
            entityDefintion.MeshName = meshName;
            currentSimObject = simObjectDefinition.register(scene.getDefaultSubScene());
            Stopwatch sw = new Stopwatch();
            sw.Start();
            scene.buildScene();
            sw.Stop();
            Log.Info("Scene loaded in {0} ms.", sw.ElapsedMilliseconds);
            entity = ((SceneNodeElement)currentSimObject.getElement("EntityNode")).getNodeObject("Entity") as Entity;
            readModelInfo();
            entity.setDisplaySkeleton(showSkeleton);
            selectableModel.ModelObject = currentSimObject;
            controller.Selection.setSelectedObject(selectableModel);
        }

        /// <summary>
        /// Destroy the model currently created.
        /// </summary>
        public void destroyModel()
        {
            if (currentSimObject != null)
            {
                currentSimObject.Dispose();
            }
        }

        /// <summary>
        /// Determine if a model is currently being shown.
        /// </summary>
        /// <returns>True if a model is being shown.</returns>
        public bool modelActive()
        {
            return currentSimObject != null;
        }

        /// <summary>
        /// Show the BinormalDebug shader.
        /// </summary>
        public void setBinormalDebug()
        {
            entity.setMaterialName("BinormalDebug");
        }

        /// <summary>
        /// Show the TangentDebug shader.
        /// </summary>
        public void setTangentDebug()
        {
            entity.setMaterialName("TangentDebug");
        }

        /// <summary>
        /// Show the NormalDebug shader.
        /// </summary>
        public void setNormalDebug()
        {
            entity.setMaterialName("NormalDebug");
        }

        /// <summary>
        /// Render the model as normal.
        /// </summary>
        public void setNormalMaterial()
        {
            entity.setMaterialName(entityMaterialName);
        }

        /// <summary>
        /// Show a particluar texture on the model.
        /// </summary>
        /// <param name="textureFileName">The name of the texture to show.</param>
        public void showIndividualTexture(String textureFileName)
        {
            fixedTexture.setTextureName(textureFileName);
            entity.setMaterialName(fixedFunctionTextured.Value.getName());
        }

        /// <summary>
        /// Rebuild or build the Tangent vectors using the ogre calculator.
        /// </summary>
        public void buildTangentVectors()
        {
            using (MeshPtr mesh = entity.getMesh())
            {
                mesh.Value.buildTangentVectors();
            }
        }

        /// <summary>
        /// Recalculate the binormal vectors as the cross product between the tangents and the normals.
        /// </summary>
        public unsafe void buildBinormalVectors()
        {
            using (MeshPtr mesh = entity.getMesh())
            {
                SubMesh subMesh = mesh.Value.getSubMesh(0);
                VertexData vertexData = subMesh.vertexData;
                VertexDeclaration vertexDeclaration = vertexData.vertexDeclaration;
                VertexBufferBinding vertexBinding = vertexData.vertexBufferBinding;
                VertexElement normalElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_NORMAL);
                VertexElement tangentElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_TANGENT);
                VertexElement binormalElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_BINORMAL);

                int numVertices = vertexData.vertexCount.ToInt32();
                HardwareVertexBufferSharedPtr normalHardwareBuffer = vertexBinding.getBuffer(normalElement.getSource());
                int normalVertexSize = normalHardwareBuffer.Value.getVertexSize().ToInt32();
                byte* normalBuffer = (byte*)normalHardwareBuffer.Value.lockBuf(HardwareBuffer.LockOptions.HBL_READ_ONLY);

                HardwareVertexBufferSharedPtr tangentHardwareBuffer = vertexBinding.getBuffer(tangentElement.getSource());
                int tangetVertexSize = tangentHardwareBuffer.Value.getVertexSize().ToInt32();
                byte* tangentBuffer = (byte*)tangentHardwareBuffer.Value.lockBuf(HardwareBuffer.LockOptions.HBL_READ_ONLY);

                HardwareVertexBufferSharedPtr binormalHardwareBuffer = vertexBinding.getBuffer(binormalElement.getSource());
                int binormalVertexSize = binormalHardwareBuffer.Value.getVertexSize().ToInt32();
                byte* binormalBuffer = (byte*)binormalHardwareBuffer.Value.lockBuf(HardwareBuffer.LockOptions.HBL_NORMAL);

                Vector3* normal;
                Vector3* tangent;
                Vector3* binormal;

                for (int i = 0; i < numVertices; ++i)
                {
                    normalElement.baseVertexPointerToElement(normalBuffer, (float**)&normal);
                    tangentElement.baseVertexPointerToElement(tangentBuffer, (float**)&tangent);
                    binormalElement.baseVertexPointerToElement(binormalBuffer, (float**)&binormal);

                    *binormal = normal->cross(ref *tangent) * -1.0f;

                    normalBuffer += normalVertexSize;
                    tangentBuffer += tangetVertexSize;
                    binormalBuffer += binormalVertexSize;
                }

                normalHardwareBuffer.Value.unlock();
                tangentHardwareBuffer.Value.unlock();
                binormalHardwareBuffer.Value.unlock();

                normalHardwareBuffer.Dispose();
                tangentHardwareBuffer.Dispose();
                binormalHardwareBuffer.Dispose();
            }
        }

        /// <summary>
        /// Save the model in binary format.
        /// </summary>
        /// <param name="filename">The name of the file to save.</param>
        public void saveModel(String filename)
        {
            if (entity != null)
            {
                using (MeshSerializer meshSerializer = new MeshSerializer())
                {
                    using (MeshPtr mesh = entity.getMesh())
                    {
                        meshSerializer.exportMesh(mesh.Value, filename);
                    }
                }
            }
        }

        private void readModelInfo()
        {
            entityMaterialName = entity.getSubEntity(0).getMaterialName();
            Log.Debug("Material name is {0}.", entityMaterialName);
            using (MaterialPtr modelMaterial = MaterialManager.getInstance().getByName(entityMaterialName))
            {
                //Get the texture names
                modelTextures.Clear();
                ushort numTechniques = modelMaterial.Value.getNumTechniques();
                for (ushort tech = 0; tech < numTechniques; ++tech)
                {
                    Technique technique = modelMaterial.Value.getTechnique(tech);
                    ushort numPasses = technique.getNumPasses();
                    for (ushort p = 0; p < numPasses; ++p)
                    {
                        Pass pass = technique.getPass(p);
                        ushort numTextures = pass.getNumTextureUnitStates();
                        for (ushort tex = 0; tex < numTextures; ++tex)
                        {
                            TextureUnitState texture = pass.getTextureUnitState(tex);
                            modelTextures.AddLast(texture.getTextureName());
                        }
                    }
                }
            }
            customParameters.SubEntity = entity.getSubEntity(0);
            if (entity.hasSkeleton())
            {
                skeletonWindow.setSkeleton(entity.getSkeleton());
            }
            else
            {
                skeletonWindow.clearSkeleton();
            }
            Log.Default.debug("Model has {0} sub entities.", entity.getNumSubEntities());
        }

        public IEnumerable<String> TextureNames
        {
            get
            {
                return modelTextures;
            }
        }

        public bool ShowSkeleton
        {
            get
            {
                return showSkeleton;
            }
            set
            {
                showSkeleton = value;
                if (entity != null)
                {
                    entity.setDisplaySkeleton(showSkeleton);
                }
            }
        }
    }
}
