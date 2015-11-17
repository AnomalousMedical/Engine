using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public static class EntityExtensions
    {
        public static unsafe float calculateVolume(this Entity entity)
        {
            float volume = 0.0f;

            using (MeshPtr mesh = entity.getMesh())
            {
                SubMesh subMesh = mesh.Value.getSubMesh(0);
                using (VertexData vertexData = subMesh.vertexData.clone(true))
                {
                    if (entity.hasSkeleton())
                    {
                        entity.animateVertexData(vertexData);
                    }

                    VertexDeclaration vertexDeclaration = vertexData.vertexDeclaration;
                    VertexBufferBinding vertexBinding = vertexData.vertexBufferBinding;
                    VertexElement positionElement = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_POSITION);

                    HardwareVertexBufferSharedPtr positionHardwareBuffer = vertexBinding.getBuffer(positionElement.getSource());
                    int vertexSize = positionHardwareBuffer.Value.getVertexSize().ToInt32();
                    byte* vertexBuffer = (byte*)positionHardwareBuffer.Value.lockBuf(HardwareBuffer.LockOptions.HBL_NORMAL);

                    IndexData indexData = subMesh.indexData;
                    int numIndices = indexData.IndexCount.ToInt32();

                    byte* currentVertex;
                    Vector3* v0, v1, v2; //Each vertex

                    using (var indexBuffer = indexData.IndexBuffer)
                    {
                        switch (indexBuffer.Value.getType())
                        {
                            case HardwareIndexBuffer.IndexType.IT_16BIT:
                                short* shortIndexBuffer = (short*)indexBuffer.Value.lockBuf(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                                for (int i = 0; i < numIndices; i += 3)
                                {
                                    currentVertex = vertexBuffer + vertexSize * shortIndexBuffer[i];
                                    positionElement.baseVertexPointerToElement(currentVertex, (float**)&v0);

                                    currentVertex = vertexBuffer + vertexSize * shortIndexBuffer[i + 1];
                                    positionElement.baseVertexPointerToElement(currentVertex, (float**)&v1);

                                    currentVertex = vertexBuffer + vertexSize * shortIndexBuffer[i + 2];
                                    positionElement.baseVertexPointerToElement(currentVertex, (float**)&v2);

                                    volume += Geometry.SignedVolumeOfTetrahedron(*v0, *v1, *v2);
                                }
                                break;
                            case HardwareIndexBuffer.IndexType.IT_32BIT:
                                int* intIndexBuffer = (int*)indexData.IndexBuffer.Value.lockBuf(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                                for (int i = 0; i < numIndices; i += 3)
                                {
                                    currentVertex = vertexBuffer + vertexSize * intIndexBuffer[i];
                                    positionElement.baseVertexPointerToElement(currentVertex, (float**)&v0);

                                    currentVertex = vertexBuffer + vertexSize * intIndexBuffer[i];
                                    positionElement.baseVertexPointerToElement(currentVertex, (float**)&v1);

                                    currentVertex = vertexBuffer + vertexSize * intIndexBuffer[i];
                                    positionElement.baseVertexPointerToElement(currentVertex, (float**)&v2);

                                    volume += Geometry.SignedVolumeOfTetrahedron(*v0, *v1, *v2);
                                }
                                break;
                        }

                        indexBuffer.Value.unlock();
                    }

                    positionHardwareBuffer.Value.unlock();

                    positionHardwareBuffer.Dispose();
                }

                return volume;
            }
        }

        public static void animateVertexData(this Entity entity, VertexData vertexData)
        {
            Entity_animateVertexData(entity.OgreObject, vertexData.OgreObject);
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Entity_animateVertexData(IntPtr entity, IntPtr vertexData);

        #endregion
    }
}
