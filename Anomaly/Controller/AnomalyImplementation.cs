using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anomaly
{
    public interface IAnomalyImplementation
    {
        void AddPlugins(PluginManager pluginManager);
    }
}
