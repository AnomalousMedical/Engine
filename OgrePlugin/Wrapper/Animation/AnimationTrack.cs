using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public enum VertexAnimationType : uint
    {
	    /// <summary>
	    /// No animation
	    /// </summary>
	    VAT_NONE = 0,
	    /// <summary>
	    /// Morph animation is made up of many interpolated snapshot keyframes
	    /// </summary>
	    VAT_MORPH = 1,
	    /// <summary>
	    /// Pose animation is made up of a single delta pose keyframe
	    /// </summary>
	    VAT_POSE = 2
    };

    class AnimationTrack
    {
    }
}
