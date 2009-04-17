using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using OgreWrapper;
using Engine.Renderer;
using Logging;
using Engine.Platform;

namespace OgrePlugin
{
    public class OgreInterface : RendererPlugin
    {
        private Root root;
        OgreLogConnection ogreLog;
        private OgreWindowInfo windowInfo;
        private OgreUpdate ogreUpdate;

        public OgreInterface()
        {
            root = new Root("", "", "");
            ogreLog = new OgreLogConnection();
            ogreUpdate = new OgreUpdate(root);
        }

        public void Dispose()
        {
            ogreLog.Dispose();
        }

        public void initialize(PluginManager pluginManager)
        {
            DefaultWindowInfo defaultWindowInfo;
            pluginManager.setRendererPlugin(this, out defaultWindowInfo);

            try
            {
                root.loadPlugin("RenderSystem_Direct3D9");
                RenderSystem rs = root.getRenderSystemByName("Direct3D9 Rendering Subsystem");
                String valid = rs.validateConfigOptions();
                if (valid.Length != 0)
                {
                    throw new InvalidPluginException(String.Format("Invalid Ogre configuration {0}", valid));
                }
                root.setRenderSystem(rs);
                root.initialize(false);

                root.loadPlugin("Plugin_CgProgramManager");

                if (defaultWindowInfo.AutoCreateWindow)
                {
                    RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen);
                    OgreOSWindow ogreWindow = new OgreOSWindow(renderWindow);
                    windowInfo = new OgreWindowInfo(ogreWindow);
                }
                else
                {
                    Dictionary<String, String> miscParams = new Dictionary<string, string>();
                    miscParams.Add("externalWindowHandle", defaultWindowInfo.EmbedWindow.Handle.ToInt32().ToString());
                    root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                    windowInfo = new OgreWindowInfo(defaultWindowInfo.EmbedWindow);
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginException(String.Format("Exception initializing renderer. Message: {0}", e.Message));
            }
        }

        public void setPlatformInfo(Timer mainTimer, EventManager eventManager)
        {
            mainTimer.addFullSpeedUpdateListener(ogreUpdate);
        }

        public string getName()
        {
            return "OgrePlugin";
        }

        #region RendererPlugin Members

        public WindowInfo WindowInfo
        {
            get
            {
                return windowInfo;
            }
        }

        #endregion
    }
}
