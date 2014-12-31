using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using OgrePlugin;
using OgreWrapper;
using Engine;
using Logging;
using System.Diagnostics;
using System.IO;
using Anomalous.Minimus;

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
        //private SelectableModel selectableModel = new SelectableModel();
        private bool showSkeleton = false;

        //OgreModelEditorController controller;

        //GUI
        //private SkeletonWindow skeletonWindow = new SkeletonWindow();
        //private CustomParameterControl customParameters = new CustomParameterControl();
        //private AnimationWindow animationWindow = new AnimationWindow();

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
            //this.controller = controller;
            //controller.MainTimer.addUpdateListener(animationWindow);
        }

        //public DockContent getDockContent(String persistString)
        //{
        //    if (skeletonWindow.GetType().ToString() == persistString)
        //    {
        //        return skeletonWindow;
        //    }
        //    if (customParameters.GetType().ToString() == persistString)
        //    {
        //        return customParameters;
        //    }
        //    if (animationWindow.GetType().ToString() == persistString)
        //    {
        //        return animationWindow;
        //    }
        //    return null;
        //}

        //public void createDefaultWindows()
        //{
        //    controller.showDockContent(skeletonWindow);
        //    controller.showDockContent(customParameters);
        //    controller.showDockContent(animationWindow);
        //}

        public void Dispose()
        {
            //fixedFunctionTextured.Dispose();
            //skeletonWindow.Dispose();
            //animationWindow.Dispose();
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
            scene.buildScene(SceneBuildOptions.None);
            sw.Stop();
            Log.Info("Scene loaded in {0} ms.", sw.ElapsedMilliseconds);
            entity = ((SceneNodeElement)currentSimObject.getElement("EntityNode")).getNodeObject("Entity") as Entity;
            readModelInfo();
            entity.setDisplaySkeleton(showSkeleton);
            //selectableModel.ModelObject = currentSimObject;
            //controller.Selection.setSelectedObject(selectableModel);
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

        public void removeBinormals()
        {
            //This code makes the editor crash and is currently not being called. Being left for reference.
            using (MeshPtr mesh = entity.getMesh())
            {
                SubMesh subMesh = mesh.Value.getSubMesh(0);
                VertexData vertexData = subMesh.vertexData;
                VertexDeclaration vertexDeclaration = vertexData.vertexDeclaration;
                //VertexBufferBinding vertexBinding = vertexData.vertexBufferBinding;
                //VertexElement normalElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_NORMAL);
                //VertexElement tangentElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_TANGENT);
                VertexElement binormalElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_BINORMAL);
                if (binormalElement != null)
                {
                    vertexDeclaration.removeElement(VertexElementSemantic.VES_BINORMAL);
                    vertexData.reorganizeBuffers(vertexDeclaration);
                }
            }
            Log.Info("Binormals Removed");
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

        internal unsafe void saveModelJSON(string filename)
        {
            StringBuilder sb = new StringBuilder();

            using (MeshPtr mesh = entity.getMesh())
            {
                SubMesh subMesh = mesh.Value.getSubMesh(0);
                VertexData vertexData = subMesh.vertexData;
                VertexDeclaration vertexDeclaration = vertexData.vertexDeclaration;
                VertexBufferBinding vertexBinding = vertexData.vertexBufferBinding;
                VertexElement normalElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_NORMAL);
                VertexElement tangentElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_TANGENT);
                VertexElement binormalElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_BINORMAL);
                VertexElement uvElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_TEXTURE_COORDINATES);
                VertexElement positionElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_POSITION);

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

                HardwareVertexBufferSharedPtr uvHardwareBuffer = vertexBinding.getBuffer(uvElement.getSource());
                int uvVertexSize = uvHardwareBuffer.Value.getVertexSize().ToInt32();
                byte* uvBuffer = (byte*)uvHardwareBuffer.Value.lockBuf(HardwareBuffer.LockOptions.HBL_NORMAL);

                HardwareVertexBufferSharedPtr positionHardwareBuffer = vertexBinding.getBuffer(positionElement.getSource());
                int positionVertexSize = positionHardwareBuffer.Value.getVertexSize().ToInt32();
                byte* positionBuffer = (byte*)positionHardwareBuffer.Value.lockBuf(HardwareBuffer.LockOptions.HBL_NORMAL);

                Vector3* normal;
                Vector3* tangent;
                Vector3* binormal;
                Vector3* uv;
                Vector3* position;

                IndexData indexData = subMesh.indexData;

                sb.AppendFormat(@"{{

""metadata"":
{{
""sourceFile"": ""Skull.max"",
""generatedBy"": ""3ds max ThreeJSExporter"",
""formatVersion"": 3,
""vertices"": {0},
""normals"": {1},
""colors"": 0,
""uvs"": {2},
""triangles"": {3},
""materials"": 1
}},

""materials"": [
{{
""DbgIndex"" : 0,
""DbgName"" : ""Material #327"",
""colorDiffuse"" : [0.5880, 0.5880, 0.5880],
""colorAmbient"" : [0.5880, 0.5880, 0.5880],
""colorSpecular"" : [0.9000, 0.9000, 0.9000],
""transparency"" : 1.0,
""specularCoef"" : 10.0,
""vertexColors"" : false
}}

],

", numVertices, numVertices, numVertices, indexData.IndexBuffer.Value.getNumIndexes().ToInt32() / 3);

                StringBuilder vertexStringBuilder = new StringBuilder("\n\n\"vertices\": [");
                StringBuilder normalStringBuilder = new StringBuilder("\n\n\"normals\": [");
                StringBuilder uvStringBuilder = new StringBuilder("\n\n\"uvs\": [[");

                String appendString = "{0},{1},{2}";

                for (int i = 0; i < numVertices; ++i)
                {
                    normalElement.baseVertexPointerToElement(normalBuffer, (float**)&normal);
                    tangentElement.baseVertexPointerToElement(tangentBuffer, (float**)&tangent);
                    binormalElement.baseVertexPointerToElement(binormalBuffer, (float**)&binormal);
                    uvElement.baseVertexPointerToElement(uvBuffer, (float**)&uv);
                    positionElement.baseVertexPointerToElement(positionBuffer, (float**)&position);

                    //*binormal = normal->cross(ref *tangent) * -1.0f;
                    vertexStringBuilder.AppendFormat(appendString, position->x, position->y, position->z);
                    normalStringBuilder.AppendFormat(appendString, normal->x, normal->y, normal->z);
                    uvStringBuilder.AppendFormat(appendString, uv->x, uv->y, uv->z);

                    normalBuffer += normalVertexSize;
                    tangentBuffer += tangetVertexSize;
                    binormalBuffer += binormalVertexSize;
                    uvBuffer += uvVertexSize;
                    positionBuffer += positionVertexSize;

                    appendString = ",{0},{1},{2}";
                }

                vertexStringBuilder.Append("],");
                normalStringBuilder.Append("],");
                uvStringBuilder.Append("]],");

                normalHardwareBuffer.Value.unlock();
                tangentHardwareBuffer.Value.unlock();
                binormalHardwareBuffer.Value.unlock();
                uvHardwareBuffer.Value.unlock();
                positionHardwareBuffer.Value.unlock();

                normalHardwareBuffer.Dispose();
                tangentHardwareBuffer.Dispose();
                binormalHardwareBuffer.Dispose();
                uvHardwareBuffer.Dispose();
                positionHardwareBuffer.Dispose();

                sb.Append(vertexStringBuilder.ToString());
                sb.Append(normalStringBuilder.ToString());
                sb.Append(uvStringBuilder.ToString());
                sb.Append("\n\n\"colors\": [],");

                StringBuilder triangleStringBuilder = new StringBuilder("\n\n\"faces\": [");

                int numFaces = indexData.IndexBuffer.Value.getNumIndexes().ToInt32();
                int indexSize = indexData.IndexBuffer.Value.getIndexSize().ToInt32();
                byte* indexBuffer = (byte*)indexData.IndexBuffer.Value.lockBuf(HardwareBuffer.LockOptions.HBL_READ_ONLY);

                appendString = "{0}";

                switch (indexData.IndexBuffer.Value.getType())
                {
                    case HardwareIndexBuffer.IndexType.IT_16BIT:
                        for (int i = 0; i < numFaces; ++i)
                        {
                            triangleStringBuilder.AppendFormat(appendString, *(short*)indexBuffer);
                            indexBuffer += indexSize;
                            appendString = ",{0}";
                        }
                        break;
                    case HardwareIndexBuffer.IndexType.IT_32BIT:
                        for(int i = 0; i < numFaces; ++i)
                        {
                            triangleStringBuilder.AppendFormat(appendString, *(int*)indexBuffer);
                            indexBuffer += indexSize;
                            appendString = ",{0}";
                        }
                        break;
                }

                triangleStringBuilder.Append("]");

                sb.Append(triangleStringBuilder.ToString());

                sb.Append("\n\n}");
            }

            using (StreamWriter fileStream = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write)))
            {
                fileStream.Write(sb.ToString());
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
            //customParameters.Entity = entity;
            //if (entity.hasSkeleton())
            //{
            //    skeletonWindow.setSkeleton(entity);
            //}
            //else
            //{
            //    skeletonWindow.clearSkeleton();
            //}
            //animationWindow.findAnimations(entity);
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
