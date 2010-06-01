using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class HardwareBuffer
    {
        [SingleEnum]
        public enum Usage : uint
        {
	        /// <summary>
	        /// Static buffer which the application rarely modifies once created.
            /// Modifying the contents of this buffer will involve a performance hit.
	        /// </summary>
	        HBU_STATIC = 1,

	        /// <summary>
	        /// Indicates the application would like to modify this buffer with the CPU
            /// fairly often. Buffers created with this flag will typically end up in
            /// AGP memory rather than video memory.
	        /// </summary>
	        HBU_DYNAMIC = 2,

	        /// <summary>
	        /// Indicates the application will never read the contents of the buffer back, 
            /// it will only ever write data. Locking a buffer with this flag will ALWAYS 
            /// return a pointer to new, blank memory rather than the memory associated 
            /// with the contents of the buffer; this avoids DMA stalls because you can 
            /// write to a new memory area while the previous one is being used. 
	        /// </summary>
	        HBU_WRITE_ONLY = 4,

	        /// <summary>
	        /// Indicates that the application will be refilling the contents
            /// of the buffer regularly (not just updating, but generating the
            /// contents from scratch), and therefore does not mind if the contents 
            /// of the buffer are lost somehow and need to be recreated. This
            /// allows and additional level of optimisation on the buffer.
            /// This option only really makes sense when combined with 
            /// HBU_DYNAMIC_WRITE_ONLY.
	        /// </summary>
	        HBU_DISCARDABLE = 8,

	        /// <summary>
	        /// Combination of HBU_STATIC and HBU_WRITE_ONLY
	        /// </summary>
	        HBU_STATIC_WRITE_ONLY = 5, 

	        /// <summary>
	        /// Combination of HBU_DYNAMIC and HBU_WRITE_ONLY. If you use 
            /// this, strongly consider using HBU_DYNAMIC_WRITE_ONLY_DISCARDABLE
            /// instead if you update the entire contents of the buffer very 
            /// regularly. 
	        /// </summary>
	        HBU_DYNAMIC_WRITE_ONLY = 6,
            
	        /// <summary>
	        /// Combination of HBU_DYNAMIC, HBU_WRITE_ONLY and HBU_DISCARDABLE
	        /// </summary>
	        HBU_DYNAMIC_WRITE_ONLY_DISCARDABLE = 14


        };

        /// <summary>
        /// Hardware buffer locking options.
        /// </summary>
        [SingleEnum]
        public enum LockOptions : uint
        {
	        /// <summary>
	        /// Normal mode, ie allows read/write and contents are preserved. 
	        /// </summary>
	        HBL_NORMAL,

	        /// <summary>
	        /// Discards the <em>entire</em> buffer while locking; this allows optimisation to be 
	        /// performed because synchronisation issues are relaxed. Only allowed on buffers 
            /// created with the HBU_DYNAMIC flag.
	        /// </summary>
	        HBL_DISCARD,

	        /// <summary>
	        /// Lock the buffer for reading only. Not allowed in buffers which are created with HBU_WRITE_ONLY. 
	        /// Mandatory on static buffers, i.e. those created without the HBU_DYNAMIC flag. 
	        /// </summary>
	        HBL_READ_ONLY,

	        /// <summary>
	        /// As HBL_NORMAL, except the application guarantees not to overwrite any 
            /// region of the buffer which has already been used in this frame, can allow
            /// some optimisation on some APIs.
	        /// </summary>
            HBL_NO_OVERWRITE
        };

    }
}
