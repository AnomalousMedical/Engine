using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    public interface FeedbackCameraPositioner
    {
        Vector3 Translation { get; }

        Vector3 LookAt { get; }

        void preRender();
    }
}
