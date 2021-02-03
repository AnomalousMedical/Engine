using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anomalous.OSPlatform
{
    public class WindowOptions
    {
        public String Title { get; set; } = "I'm Lazy";

        public IntVector2 Position { get; set; }

        public IntSize2 Size { get; set; } = new IntSize2(1920, 1080);

        public IntSize2 FullScreenSize { get; set; } = new IntSize2(1920, 1080);

        public bool Fullscreen { get; set; }

        public bool Maximized { get; set; } = true;
    }
}
