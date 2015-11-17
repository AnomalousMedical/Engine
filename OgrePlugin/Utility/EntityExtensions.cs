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

        /// <summary>
        /// Calculate the volume of an entity's subentity at the given index (default 0). This is not the most robust funciton
        /// but does fine with plain meshes and meshes with skeletal animation.
        /// </summary>
        /// <param name="entity">The entity to calculate the sub entity volume of.</param>
        /// <param name="subEntityIndex">The index of the sub entity.</param>
        /// <returns>The volume in engine units.</returns>
        public static unsafe float calculateVolume(this Entity entity, ushort subEntityIndex = 0)
        {
            float volume = 0.0f;

            using (MeshPtr mesh = entity.getMesh())
            {
                SubMesh subMesh = mesh.Value.getSubMesh(subEntityIndex);
                using (VertexData vertexData = subMesh.vertexData.clone(true))
                {
                    if (entity.hasSkeleton())
                    {
                        entity.animateVertexData(vertexData, subEntityIndex);
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

        /// <summary>
        /// Perform skeletal animation on a given entity copying the results to vertexData. Used for volume calculations.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="vertexData"></param>
        /// <param name="subEntityIndex"></param>
        private static void animateVertexData(this Entity entity, VertexData vertexData, ushort subEntityIndex)
        {
            Entity_animateVertexData(entity.OgreObject, vertexData.OgreObject, subEntityIndex);
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Entity_animateVertexData(IntPtr entity, IntPtr vertexData, ushort subEntityIndex);

        #endregion
    }
}
