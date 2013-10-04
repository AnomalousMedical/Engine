using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Engine.ObjectManagement;
using Engine.Renderer;
using Engine;

namespace BulletPlugin
{
    public interface BulletScene : SimElementManager
    {
        bool Active { get; set; }
    }
}
