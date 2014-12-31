using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;
using Engine;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class Mesh : Resource
    {
        internal static Mesh createWrapper(IntPtr mesh)
        {
            return new Mesh(mesh);
        }

        private WrapperCollection<SubMesh> subMeshes = new WrapperCollection<SubMesh>(SubMesh.createWrapper);
        private WrapperCollection<Animation> animations = new WrapperCollection<Animation>(Animation.createWrapper);
        private WrapperCollection<Pose> poses = new WrapperCollection<Pose>(Pose.createWrapper);

        private VertexData sharedVertexData = null;

        private Mesh(IntPtr mesh)
            :base(mesh)
        {
            IntPtr sharedVData = Mesh_getSharedVertexData(mesh);
            if(sharedVData != IntPtr.Zero)
            {
                sharedVertexData = new VertexData(sharedVData);
            }
        }

        public override void Dispose()
        {
            subMeshes.Dispose();
            animations.Dispose();
            poses.Dispose();
            base.Dispose();
        }

        public SubMesh createSubMesh()
        {
            return subMeshes.getObject(Mesh_createSubMesh(resource));
        }

        public SubMesh createSubMesh(String name)
        {
            return subMeshes.getObject(Mesh_createSubMeshName(resource, name));
        }

        public void nameSubMesh(String name, ushort index)
        {
            Mesh_nameSubMesh(resource, name, index);
        }

        public ushort _getSubMeshIndex(String name)
        {
            return Mesh__getSubMeshIndex(resource, name);
        }

        public ushort getNumSubMeshes()
        {
            return Mesh_getNumSubMeshes(resource);
        }

        public SubMesh getSubMesh(ushort index)
        {
            return subMeshes.getObject(Mesh_getSubMesh(resource, index));
        }

        public SubMesh getSubMesh(String name)
        {
            return subMeshes.getObject(Mesh_getSubMeshName(resource, name));
        }

        public MeshPtr clone(String newName)
        {
            MeshManager meshManager = MeshManager.getInstance();
            return meshManager.getObject(Mesh_clone(resource, newName, meshManager.ProcessWrapperObjectCallback));
        }

        public MeshPtr clone(String newName, String newGroup)
        {
            MeshManager meshManager = MeshManager.getInstance();
            return meshManager.getObject(Mesh_cloneChangeGroup(resource, newName, newGroup, meshManager.ProcessWrapperObjectCallback));
        }

        public float getBoundingSphereRadius()
        {
            return Mesh_getBoundingSphereRadius(resource);
        }

        public void _setBoundingSphereRadius(float radius)
        {
            Mesh__setBoundingSphereRadius(resource, radius);
        }

        public void setSkeletonName(String skeletonName)
        {
            Mesh_setSkeletonName(resource, skeletonName);
        }

        public bool hasSkeleton()
        {
            return Mesh_hasSkeleton(resource);
        }

        public bool hasVertexAnimation()
        {
            return Mesh_hasVertexAnimation(resource);
        }

        public SkeletonPtr getSkeleton()
        {
            SkeletonManager skeletonManager = SkeletonManager.getInstance();
            return skeletonManager.getObject(Mesh_getSkeleton(resource, skeletonManager.ProcessWrapperObjectCallback));
        }

        public String getSkeletonName()
        {
            return Marshal.PtrToStringAnsi(Mesh_getSkeletonName(resource));
        }

        public void _initAnimationState(AnimationStateSet animSet)
        {
            Mesh__initAnimationState(resource, animSet.OgreObject);
        }

        public void _refreshAnimationState(AnimationStateSet animSet)
        {
            Mesh__refreshAnimationState(resource, animSet.OgreObject);
        }

        public void clearBoneAssignments()
        {
            Mesh_clearBoneAssignments(resource);
        }

        public ushort getNumLodLevels()
        {
            return Mesh_getNumLodLevels(resource);
        }

        public void updateManualLodLevel(ushort index, String meshName)
        {
            Mesh_updateManualLodLevel(resource, index, meshName);
        }

        public ushort getLodIndex(float depth)
        {
            return Mesh_getLodIndex(resource, depth);
        }

        public void removeLodLevels()
        {
            Mesh_removeLodLevels(resource);
        }

        public void setVertexBufferPolicy(HardwareBuffer.Usage usage)
        {
            Mesh_setVertexBufferPolicy(resource, usage);
        }

        public void setVertexBufferPolicy(HardwareBuffer.Usage usage, bool shadowBuffer)
        {
            Mesh_setVertexBufferPolicyShadow(resource, usage, shadowBuffer);
        }

        public void setIndexBufferPolicy(HardwareBuffer.Usage usage)
        {
            Mesh_setIndexBufferPolicy(resource, usage);
        }

        public void setIndexBufferPolicy(HardwareBuffer.Usage usage, bool shadowBuffer)
        {
            Mesh_setIndexBufferPolicyShadow(resource, usage, shadowBuffer);
        }

        public HardwareBuffer.Usage getVertexBufferUsage()
        {
            return Mesh_getVertexBufferUsage(resource);
        }

        public HardwareBuffer.Usage getIndexBufferUsage()
        {
            return Mesh_getIndexBufferUsage(resource);
        }

        public bool isVertexBufferShadowed()
        {
            return Mesh_isVertexBufferShadowed(resource);
        }

        public bool isIndexBufferShadowed()
        {
            return Mesh_isIndexBufferShadowed(resource);
        }

        public void buildTangentVectors()
        {
            Mesh_buildTangentVectors(resource);
        }

        public void buildTangentVectors(VertexElementSemantic targetSemantic)
        {
            Mesh_buildTangentVectors1(resource, targetSemantic);
        }

        public void buildTangentVectors(VertexElementSemantic targetSemantic, ushort sourceTexCoordSet)
        {
            Mesh_buildTangentVectors2(resource, targetSemantic, sourceTexCoordSet);
        }

        public void buildTangentVectors(VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index)
        {
            Mesh_buildTangentVectors3(resource, targetSemantic, sourceTexCoordSet, index);
        }

        public void buildTangentVectors(VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored)
        {
            Mesh_buildTangentVectors4(resource, targetSemantic, sourceTexCoordSet, index, splitMirrored);
        }

        public void buildTangentVectors(VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated)
        {
            Mesh_buildTangentVectors5(resource, targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated);
        }

        public void buildTangentVectors(VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated, bool storeParityInW)
        {
            Mesh_buildTangentVectors6(resource, targetSemantic, sourceTexCoordSet, index, splitMirrored, splitRotated, storeParityInW);
        }

        public bool suggestTangentVectorBuildParams(VertexElementSemantic targetSemantic, out ushort outSourceCoordSet, out ushort outIndex)
        {
            return Mesh_suggestTangentVectorBuildParams(resource, targetSemantic, out outSourceCoordSet, out outIndex);
        }

        public void buildEdgeList()
        {
            Mesh_buildEdgeList(resource);
        }

        public void freeEdgeList()
        {
            Mesh_freeEdgeList(resource);
        }

        public void prepareForShadowVolume()
        {
            Mesh_prepareForShadowVolume(resource);
        }

        public bool isPreparedForShadowVolumes()
        {
            return Mesh_isPreparedForShadowVolumes(resource);
        }

        public bool isEdgeListBuilt()
        {
            return Mesh_isEdgeListBuilt(resource);
        }

        public uint getSubMeshIndex(String name)
        {
            return Mesh_getSubMeshIndex(resource, name);
        }

        public void setAutoBuildEdgeLists(bool autobuild)
        {
            Mesh_setAutoBuildEdgeLists(resource, autobuild);
        }

        public bool getAutoBuildEdgeLists()
        {
            return Mesh_getAutoBuildEdgeLists(resource);
        }

        public VertexAnimationType getSharedVertexDataAnimationType()
        {
            return Mesh_getSharedVertexDataAnimationType(resource);
        }

        public Animation createAnimation(String name, float length)
        {
            return animations.getObject(Mesh_createAnimation(resource, name, length));
        }

        public Animation getAnimation(String name)
        {
            return animations.getObject(Mesh_getAnimation(resource, name));
        }

        public bool hasAnimation(String name)
        {
            return Mesh_hasAnimation(resource, name);
        }

        public void removeAnimation(String name)
        {
            animations.destroyObject(Mesh_getAnimation(resource, name));
            Mesh_removeAnimation(resource, name);
        }

        public ushort getNumAnimations()
        {
            return Mesh_getNumAnimations(resource);
        }

        public Animation getAnimation(ushort index)
        {
            return animations.getObject(Mesh_getAnimationIndex(resource, index));
        }

        public void removeAllAnimations()
        {
            animations.clearObjects();
            Mesh_removeAllAnimations(resource);
        }

        public void updateMaterialForAllSubMeshes()
        {
            Mesh_updateMaterialForAllSubMeshes(resource);
        }

        public Pose createPose(ushort target)
        {
            return poses.getObject(Mesh_createPose(resource, target));
        }

        public Pose createPose(ushort target, String name)
        {
            return poses.getObject(Mesh_createPoseName(resource, target, name));
        }

        public int getPoseCount()
        {
            return Mesh_getPoseCount(resource);
        }

        public Pose getPose(ushort index)
        {
            return poses.getObject(Mesh_getPose(resource, index));
        }

        public Pose getPose(String name)
        {
            return poses.getObject(Mesh_getPoseName(resource, name));
        }

        public void removePose(ushort index)
        {
            poses.destroyObject(Mesh_getPose(resource, index));
            Mesh_removePose(resource, index);
        }

        public void removePose(String name)
        {
            poses.destroyObject(Mesh_getPoseName(resource, name));
            Mesh_removePoseName(resource, name);
        }

        public void removeAllPoses()
        {
            poses.clearObjects();
            Mesh_removeAllPoses(resource);
        }

        public void _updateCompiledBoneAssignments()
        {
            Mesh__updateCompiledBoneAssignments(resource);
        }

        public VertexData SharedVertexData
	    {
		    get
            {
                return sharedVertexData;
            }
	    }

#region PInvoke

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_getSharedVertexData(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_createSubMesh(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_createSubMeshName(IntPtr mesh, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_nameSubMesh(IntPtr mesh, String name, ushort index);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern ushort Mesh__getSubMeshIndex(IntPtr mesh, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern ushort Mesh_getNumSubMeshes(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_getSubMesh(IntPtr mesh, ushort index);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_getSubMeshName(IntPtr mesh, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_clone(IntPtr mesh, String newName, ProcessWrapperObjectDelegate processWrapper);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_cloneChangeGroup(IntPtr mesh, String newName, String newGroup, ProcessWrapperObjectDelegate processWrapper);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern float Mesh_getBoundingSphereRadius(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh__setBoundingSphereRadius(IntPtr mesh, float radius);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_setSkeletonName(IntPtr mesh, String skeletonName);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool Mesh_hasSkeleton(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool Mesh_hasVertexAnimation(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_getSkeleton(IntPtr mesh, ProcessWrapperObjectDelegate processWrapper);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_getSkeletonName(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh__initAnimationState(IntPtr mesh, IntPtr animSet);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh__refreshAnimationState(IntPtr mesh, IntPtr animSet);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_clearBoneAssignments(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern ushort Mesh_getNumLodLevels(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_updateManualLodLevel(IntPtr mesh, ushort index, String meshName);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern ushort Mesh_getLodIndex(IntPtr mesh, float depth);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_removeLodLevels(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_setVertexBufferPolicy(IntPtr mesh, HardwareBuffer.Usage usage);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_setVertexBufferPolicyShadow(IntPtr mesh, HardwareBuffer.Usage usage, bool shadowBuffer);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_setIndexBufferPolicy(IntPtr mesh, HardwareBuffer.Usage usage);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_setIndexBufferPolicyShadow(IntPtr mesh, HardwareBuffer.Usage usage, bool shadowBuffer);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern HardwareBuffer.Usage Mesh_getVertexBufferUsage(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern HardwareBuffer.Usage Mesh_getIndexBufferUsage(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool Mesh_isVertexBufferShadowed(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool Mesh_isIndexBufferShadowed(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_buildTangentVectors(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_buildTangentVectors1(IntPtr mesh, VertexElementSemantic targetSemantic);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_buildTangentVectors2(IntPtr mesh, VertexElementSemantic targetSemantic, ushort sourceTexCoordSet);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_buildTangentVectors3(IntPtr mesh, VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_buildTangentVectors4(IntPtr mesh, VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_buildTangentVectors5(IntPtr mesh, VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_buildTangentVectors6(IntPtr mesh, VertexElementSemantic targetSemantic, ushort sourceTexCoordSet, ushort index, bool splitMirrored, bool splitRotated, bool storeParityInW);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool Mesh_suggestTangentVectorBuildParams(IntPtr mesh, VertexElementSemantic targetSemantic, out ushort outSourceCoordSet, out ushort outIndex);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_buildEdgeList(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_freeEdgeList(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_prepareForShadowVolume(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool Mesh_isPreparedForShadowVolumes(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool Mesh_isEdgeListBuilt(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern uint Mesh_getSubMeshIndex(IntPtr mesh, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_setAutoBuildEdgeLists(IntPtr mesh, bool autobuild);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool Mesh_getAutoBuildEdgeLists(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern VertexAnimationType Mesh_getSharedVertexDataAnimationType(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_createAnimation(IntPtr mesh, String name, float length);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_getAnimation(IntPtr mesh, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool Mesh_hasAnimation(IntPtr mesh, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_removeAnimation(IntPtr mesh, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern ushort Mesh_getNumAnimations(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_getAnimationIndex(IntPtr mesh, ushort index);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_removeAllAnimations(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_updateMaterialForAllSubMeshes(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_createPose(IntPtr mesh, ushort target);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_createPoseName(IntPtr mesh, ushort target, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern int Mesh_getPoseCount(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_getPose(IntPtr mesh, ushort index);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr Mesh_getPoseName(IntPtr mesh, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_removePose(IntPtr mesh, ushort index);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_removePoseName(IntPtr mesh, String name);

    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
    private static extern void Mesh_removeAllPoses(IntPtr mesh);

    [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
    private static extern void Mesh__updateCompiledBoneAssignments(IntPtr mesh);
#endregion
    }
}
