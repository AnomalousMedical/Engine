using Engine;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace Anomalous.Minimus.Full
{
    public class Startup : IStartup
    {
        public string Title => "Anomalous Minimus with Ogre Next";

        public string Name => "OgreNextTest";

        public IEnumerable<Assembly> AdditionalPluginAssemblies => new Assembly[0];

        public Startup()
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            
        }

        public void Initialized(CoreApp pharosApp, PluginManager pluginManager)
        {
            var scope = pluginManager.GlobalScope;

            
        }
    }
}
