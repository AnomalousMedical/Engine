using Anomalous.Interop;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public class GpuProgram : Resource
    {
        private static StringRetriever stringRetriever = new StringRetriever();

        internal GpuProgram(IntPtr ptr)
            :base(ptr)
        {
            
        }

        public String SourceFile
        {
            get
            {
                GpuProgram_getSourceFile(this.OgreResource, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
            set
            {
                GpuProgram_setSourceFile(this.OgreResource, value);
            }
        }

        public String Source
        {
            get
            {
                GpuProgram_getSource(this.OgreResource, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
            set
            {
                GpuProgram_setSource(this.OgreResource, value);
            }
        }

        public String SyntaxCode
        {
            get
            {
                GpuProgram_getSyntaxCode(this.OgreResource, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
            set
            {
                GpuProgram_setSyntaxCode(this.OgreResource, value);
            }
        }

        public GpuProgramType ProgramType
        {
            get
            {
                return GpuProgram_getType(this.OgreResource);
            }
            set
            {
                GpuProgram_setType(this.OgreResource, value);
            }
        }

        public bool IsSupported
        {
            get
            {
                return GpuProgram_isSupported(this.OgreResource);
            }
        }

        public bool SkeletalAnimationIncluded
        {
            get
            {
                return GpuProgram_isSkeletalAnimationIncluded(this.OgreResource);
            }
            set
            {
                GpuProgram_setSkeletalAnimationIncluded(this.OgreResource, value);
            }
        }

        public bool MorphAnimationIncluded
        {
            get
            {
                return GpuProgram_isMorphAnimationIncluded(this.OgreResource);
            }
            set
            {
                GpuProgram_setMorphAnimationIncluded(this.OgreResource, value);
            }
        }

        public bool PoseAnimationIncluded
        {
            get
            {
                return GpuProgram_isPoseAnimationIncluded(this.OgreResource);
            }
        }

        public ushort NumberOfPoses
        {
            get
            {
                return GpuProgram_getNumberOfPosesIncluded(this.OgreResource);
            }
            set
            {
                GpuProgram_setPoseAnimationIncluded(this.OgreResource, value);
            }
        }

        public bool VertexTextureFetchRequired
        {
            get
            {
                return GpuProgram_isVertexTextureFetchRequired(this.OgreResource);
            }
            set
            {
                GpuProgram_setVertexTextureFetchRequired(this.OgreResource, value);
            }
        }

        public bool AdjacencyInfoRequired
        {
            get
            {
                return GpuProgram_isAdjacencyInfoRequired(this.OgreResource);
            }
            set
            {
                GpuProgram_setAdjacencyInfoRequired(this.OgreResource, value);
            }
        }

        public Vector3 ComputeGroupDimensions
        {
            get
            {
                return GpuProgram_getComputeGroupDimensions(this.OgreResource);
            }
            set
            {
                GpuProgram_setComputeGroupDimensions(this.OgreResource, value);
            }
        }

        public bool HasDefaultParameters
        {
            get
            {
                return GpuProgram_hasDefaultParameters(this.OgreResource);
            }
        }

        public bool PassSurfaceAndLightStates
        {
            get
            {
                return GpuProgram_getPassSurfaceAndLightStates(this.OgreResource);
            }
        }

        public bool PassFogStates
        {
            get
            {
                return GpuProgram_getPassFogStates(this.OgreResource);
            }
        }

        public bool PassTransformStates
        {
            get
            {
                return GpuProgram_getPassTransformStates(this.OgreResource);
            }
        }

        public String Language
        {
            get
            {
                GpuProgram_getLanguage(this.OgreResource, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
        }

        public bool HasCompileError
        {
            get
            {
                return GpuProgram_hasCompileError(this.OgreResource);
            }
        }

        public void resetCompileError()
        {
            GpuProgram_resetCompileError(this.OgreResource);
        }

        /// <summary>
        /// Get a Sharedthis.OgreResource to the program parameters, you must dispose the returned pointer yourself.
        /// </summary>
        public GpuProgramParametersSharedPtr getDefaultParameters()
        {
            return GpuProgramManager.Instance.getGpuProgramParametersWrapper(GpuProgram_getDefaultParameters(this.OgreResource, GpuProgramManager.Instance.ProcessWrapperObjectCallback));
        }

        public String getParam(String name)
        {
            GpuProgram_getParam(this.OgreResource, name, stringRetriever.StringCallback, stringRetriever.Handle);
            return stringRetriever.retrieveString();
        }

        public void setParam(String name, String value)
        {
            GpuProgram_setParam(this.OgreResource, name, value);
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setSourceFile(IntPtr gpuProgram, String filename);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setSource(IntPtr gpuProgram, String source);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_getSyntaxCode(IntPtr gpuProgram, StringRetriever.Callback srCallback, IntPtr handle);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setSyntaxCode(IntPtr gpuProgram, String syntax);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_getSourceFile(IntPtr gpuProgram, StringRetriever.Callback srCallback, IntPtr handle);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_getSource(IntPtr gpuProgram, StringRetriever.Callback srCallback, IntPtr handle);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setType(IntPtr gpuProgram, GpuProgramType t);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern GpuProgramType GpuProgram_getType(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_isSupported(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setSkeletalAnimationIncluded(IntPtr gpuProgram, bool included);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_isSkeletalAnimationIncluded(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setMorphAnimationIncluded(IntPtr gpuProgram, bool included);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setPoseAnimationIncluded(IntPtr gpuProgram, ushort poseCount);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_isMorphAnimationIncluded(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_isPoseAnimationIncluded(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort GpuProgram_getNumberOfPosesIncluded(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setVertexTextureFetchRequired(IntPtr gpuProgram, bool r);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_isVertexTextureFetchRequired(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setAdjacencyInfoRequired(IntPtr gpuProgram, bool r);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_isAdjacencyInfoRequired(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setComputeGroupDimensions(IntPtr gpuProgram, Vector3 dimensions);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector3 GpuProgram_getComputeGroupDimensions(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GpuProgram_getDefaultParameters(IntPtr gpuProgram, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_hasDefaultParameters(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_getPassSurfaceAndLightStates(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_getPassFogStates(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_getPassTransformStates(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_getLanguage(IntPtr gpuProgram, StringRetriever.Callback srCallback, IntPtr handle);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgram_hasCompileError(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_resetCompileError(IntPtr gpuProgram);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_setParam(IntPtr gpuProgram, String name, String value);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgram_getParam(IntPtr gpuProgram, String name, StringRetriever.Callback srCallback, IntPtr handle);

        #endregion
    }
}
