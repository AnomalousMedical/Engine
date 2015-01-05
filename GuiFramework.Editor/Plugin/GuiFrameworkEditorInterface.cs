using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Editor
{
    class GuiFrameworkEditorInterface : PluginInterface
    {
        public void initialize(PluginManager pluginManager)
        {
            throw new NotImplementedException();
        }

        public void link(PluginManager pluginManager)
        {
            throw new NotImplementedException();
        }

        public void setPlatformInfo(Engine.Platform.UpdateTimer mainTimer, Engine.Platform.EventManager eventManager)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public DebugInterface getDebugInterface()
        {
            throw new NotImplementedException();
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            throw new NotImplementedException();
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
