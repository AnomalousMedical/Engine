using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using OgrePlugin;
using OgreWrapper;
using Engine;

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

        /// <summary>
        /// Constructor.
        /// </summary>
        public ModelController()
        {
            simObjectDefinition = new GenericSimObjectDefinition("EntitySimObject");
            simObjectDefinition.Enabled = true;
            entityDefintion = new EntityDefinition("Entity");
            nodeDefinition = new SceneNodeDefinition("EntityNode");
            nodeDefinition.addMovableObjectDefinition(entityDefintion);
            simObjectDefinition.addElement(nodeDefinition);
            fixedFunctionTextured = MaterialManager.getInstance().getByName("FixedFunctionTextured");
            fixedTexture = fixedFunctionTextured.Value.getTechnique(0).getPass(0).getTextureUnitState(0);
        }

        public void Dispose()
        {
            fixedFunctionTextured.Dispose();
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
            scene.buildScene();
            entity = ((SceneNodeElement)currentSimObject.getElement("EntityNode")).getEntity(new Identifier("EntitySimObject", "Entity"));
            readModelInfo();
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

                uint numVertices = vertexData.vertexCount;
                HardwareVertexBufferSharedPtr normalHardwareBuffer = vertexBinding.getBuffer(normalElement.getSource());
                uint normalVertexSize = normalHardwareBuffer.Value.getVertexSize();
                byte* normalBuffer = (byte*)normalHardwareBuffer.Value.@lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);

                HardwareVertexBufferSharedPtr tangentHardwareBuffer = vertexBinding.getBuffer(tangentElement.getSource());
                uint tangetVertexSize = tangentHardwareBuffer.Value.getVertexSize();
                byte* tangentBuffer = (byte*)tangentHardwareBuffer.Value.@lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);

                HardwareVertexBufferSharedPtr binormalHardwareBuffer = vertexBinding.getBuffer(binormalElement.getSource());
                uint binormalVertexSize = binormalHardwareBuffer.Value.getVertexSize();
                byte* binormalBuffer = (byte*)binormalHardwareBuffer.Value.@lock(HardwareBuffer.LockOptions.HBL_NORMAL);

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
        }

        public IEnumerable<String> TextureNames
        {
            get
            {
                return modelTextures;
            }
        }
    }
}
