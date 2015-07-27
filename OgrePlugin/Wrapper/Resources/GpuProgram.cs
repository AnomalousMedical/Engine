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
    public class GpuProgram : IDisposable
    {
        private static StringRetriever stringRetriever = new StringRetriever();

        private IntPtr ptr;

        internal GpuProgram(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        public void Dispose()
        {
            ptr = IntPtr.Zero;
        }

        public String SourceFile
        {
            get
            {
                GpuProgram_getSourceFile(ptr, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
            set
            {
                GpuProgram_setSourceFile(ptr, value);
            }
        }

        public String Source
        {
            get
            {
                GpuProgram_getSource(ptr, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
            set
            {
                GpuProgram_setSource(ptr, value);
            }
        }

        public String SyntaxCode
        {
            get
            {
                GpuProgram_getSyntaxCode(ptr, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
            set
            {
                GpuProgram_setSyntaxCode(ptr, value);
            }
        }

        public GpuProgramType ProgramType
        {
            get
            {
                return GpuProgram_getType(ptr);
            }
            set
            {
                GpuProgram_setType(ptr, value);
            }
        }

        public bool IsSupported
        {
            get
            {
                return GpuProgram_isSupported(ptr);
            }
        }

        public bool SkeletalAnimationIncluded
        {
            get
            {
                return GpuProgram_isSkeletalAnimationIncluded(ptr);
            }
            set
            {
                GpuProgram_setSkeletalAnimationIncluded(ptr, value);
            }
        }

        public bool MorphAnimationIncluded
        {
            get
            {
                return GpuProgram_isMorphAnimationIncluded(ptr);
            }
            set
            {
                GpuProgram_setMorphAnimationIncluded(ptr, value);
            }
        }

        public bool PoseAnimationIncluded
        {
            get
            {
                return GpuProgram_isPoseAnimationIncluded(ptr);
            }
        }

        public ushort NumberOfPoses
        {
            get
            {
                return GpuProgram_getNumberOfPosesIncluded(ptr);
            }
            set
            {
                GpuProgram_setPoseAnimationIncluded(ptr, value);
            }
        }

        public bool VertexTextureFetchRequired
        {
            get
            {
                return GpuProgram_isVertexTextureFetchRequired(ptr);
            }
            set
            {
                GpuProgram_setVertexTextureFetchRequired(ptr, value);
            }
        }

        public bool AdjacencyInfoRequired
        {
            get
            {
                return GpuProgram_isAdjacencyInfoRequired(ptr);
            }
            set
            {
                GpuProgram_setAdjacencyInfoRequired(ptr, value);
            }
        }

        public Vector3 ComputeGroupDimensions
        {
            get
            {
                return GpuProgram_getComputeGroupDimensions(ptr);
            }
            set
            {
                GpuProgram_setComputeGroupDimensions(ptr, value);
            }
        }

        public bool HasDefaultParameters
        {
            get
            {
                return GpuProgram_hasDefaultParameters(ptr);
            }
        }

        public bool PassSurfaceAndLightStates
        {
            get
            {
                return GpuProgram_getPassSurfaceAndLightStates(ptr);
            }
        }

        public bool PassFogStates
        {
            get
            {
                return GpuProgram_getPassFogStates(ptr);
            }
        }

        public bool PassTransformStates
        {
            get
            {
                return GpuProgram_getPassTransformStates(ptr);
            }
        }

        public String Language
        {
            get
            {
                GpuProgram_getLanguage(ptr, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
        }

        public bool HasCompileError
        {
            get
            {
                return GpuProgram_hasCompileError(ptr);
            }
        }

        public void resetCompileError()
        {
            GpuProgram_resetCompileError(ptr);
        }

        /// <summary>
        /// Get a SharedPtr to the program parameters, you must dispose the returned pointer yourself.
        /// </summary>
        public GpuProgramParametersSharedPtr getDefaultParameters()
        {
            return GpuProgramManager.Instance.getGpuProgramParametersWrapper(GpuProgram_getDefaultParameters(ptr, GpuProgramManager.Instance.ProcessWrapperObjectCallback));
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

        #endregion
    }
}
