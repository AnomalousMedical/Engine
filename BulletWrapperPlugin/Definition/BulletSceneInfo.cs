using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace BulletPlugin
{
    /// <summary>
    /// Info for the bullet scene.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct BulletSceneInfo
    {
	    public Vector3 worldAabbMin;
        public Vector3 worldAabbMax;
        public int maxProxies;
        public Vector3 gravity;
    };
}
