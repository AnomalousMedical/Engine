using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Platform
{
    public interface SystemTimer
    {
        bool initialize();

        void prime();

        double getDelta();
    }
}
