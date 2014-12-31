using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgrePlugin
{
    /// <summary>
    /// Time index object used to search keyframe at the given position. 
    /// </summary>
    public struct TimeIndex
    {
        float timePos;
	    uint keyIndex;

        /// <summary>
	    /// Construct time index object by the given time position. 
	    /// </summary>
	    /// <param name="timePos"></param>
        public TimeIndex(float timePos)
	    {
            this.timePos = timePos;
            keyIndex = 0;
	    }

	    /// <summary>
	    /// Construct time index object by the given time position and global
        /// keyframe index. 
	    /// 
	    /// Normally, you don't need to use this constructor directly, use
        /// Animation::_getTimeIndex instead. 
	    /// </summary>
	    /// <param name="timePos"></param>
	    /// <param name="keyIndex"></param>
        public TimeIndex(float timePos, uint keyIndex)
	    {
            this.timePos = timePos;
            this.keyIndex = keyIndex;
	    }

	    /// <summary>
	    /// Get the time position (in relation to the whole animation sequence)
	    /// </summary>
	    /// <returns>The time position.</returns>
        public float getTimePos()
	    {
		    return timePos;
	    }

	    /// <summary>
	    /// Get the global keyframe index (in relation to the whole animation
        /// sequence) that used to convert to local keyframe index, or
        /// INVALID_KEY_INDEX which means global keyframe index unavailable, and
        /// then slight slow method will used to search local keyframe index. 
	    /// </summary>
	    /// <returns>The key index.</returns>
        public uint getKeyIndex()
	    {
		    return keyIndex;
	    }
    }
}
