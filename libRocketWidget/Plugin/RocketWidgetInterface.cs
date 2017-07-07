using Autofac;
using Engine;
using Engine.Platform;
using libRocketPlugin;
using MyGUIPlugin;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.libRocketWidget
{
    public class RocketWidgetInterface : PluginInterface
    {
        private const String PluginName = "libRocketWidget";

        private EventListenerInstancer eventListenerInstancer;
        private static RocketRenderSystemListener rocketRenderSystemListener;

        internal RocketWidgetInterface()
        {

        }

        public void Dispose()
        {
            if (eventListenerInstancer != null)
            {
                eventListenerInstancer.Dispose();
            }
            if (rocketRenderSystemListener != null)
            {
                Root.getSingleton().getRenderSystem().removeListener(rocketRenderSystemListener);
                rocketRenderSystemListener.Dispose();
            }
        }

        public void initialize(PluginManager pluginManager, ContainerBuilder builder)
        {
            
        }

        public void link(PluginManager pluginManager)
        {
            MyGUIInterface.Instance.CommonResourceGroup.addResource(GetType().AssemblyQualifiedName, "EmbeddedScalableResource", true);

            rocketRenderSystemListener = new RocketRenderSystemListener();
            Root.getSingleton().getRenderSystem().addListener(rocketRenderSystemListener);

            eventListenerInstancer = new RocketEventListenerInstancer();
            Factory.RegisterEventListenerInstancer(eventListenerInstancer);

            RocketInterface.Instance.FileInterface.addExtension(new RocketAssemblyResourceLoader(typeof(RocketInterface).GetTypeInfo().Assembly));
            RocketInterface.Instance.FileInterface.addExtension(new RocketAssemblyResourceLoader(typeof(MyGUIInterface).GetTypeInfo().Assembly));

            FontDatabase.LoadFontFace("MyGUIPlugin.Resources.Fonts.Roboto-Regular.ttf", "Roboto", Font.Style.STYLE_NORMAL, Font.Weight.WEIGHT_NORMAL);
            FontDatabase.LoadFontFace("MyGUIPlugin.Resources.Fonts.Roboto-Bold.ttf", "Roboto", Font.Style.STYLE_NORMAL, Font.Weight.WEIGHT_BOLD);
            FontDatabase.LoadFontFace("MyGUIPlugin.Resources.Fonts.Roboto-BoldItalic.ttf", "Roboto", Font.Style.STYLE_ITALIC, Font.Weight.WEIGHT_BOLD);
            FontDatabase.LoadFontFace("MyGUIPlugin.Resources.Fonts.Roboto-Italic.ttf", "Roboto", Font.Style.STYLE_ITALIC, Font.Weight.WEIGHT_NORMAL);
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            
        }

        public string Name
        {
            get
            {
                return PluginName;
            }
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {
            
        }

        public static void clearAllCaches()
        {
            Factory.ClearStyleSheetCache();
            TemplateCache.ClearTemplateCache();
        }
    }
}
