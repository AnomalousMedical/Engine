using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;

namespace SoundPlugin
{
    public class SoundPluginInterface : PluginInterface
    {
#if STATIC_LINK
		public const String LibraryName = "__Internal";
#else
        public const String LibraryName = "SoundWrapper";
#endif

        private OpenALManager openALManager = null;
        private SoundUpdateListener soundUpdate;
        private UpdateTimer mainTimer;
        private SoundManager soundManager;
        private OSWindow resourceWindow;

        public static SoundPluginInterface Instance { get; private set; }

        public SoundPluginInterface()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Cannot create the SoundPlugin more than once");
            }
        }

        public void Dispose()
        {
            if (openALManager != null)
            {
                mainTimer.removeUpdateListener(soundUpdate);
                openALManager.Dispose();
                openALManager = null;
            }
        }

        public void initialize(PluginManager pluginManager)
        {
            openALManager = new OpenALManager();
            soundUpdate = new SoundUpdateListener(openALManager);
            soundManager = new SoundManager(openALManager);
        }

        public void link(PluginManager pluginManager)
        {
            
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.mainTimer = mainTimer;
            mainTimer.addUpdateListener(soundUpdate);
        }

        /// <summary>
        /// Set the window used to track when to create / destroy internal resources.
        /// Should be called with the main window, or else audio suspend / resume will
        /// not be detected.
        /// </summary>
        /// <param name="window">The OSWindow instance to listen to.</param>
        public void setResourceWindow(OSWindow window)
        {
            if(resourceWindow != null)
            {
                resourceWindow.CreateInternalResources -= resourceWindow_CreateInternalResources;
                resourceWindow.DestroyInternalResources -= resourceWindow_DestroyInternalResources;
            }
            resourceWindow = window;
            if (resourceWindow != null)
            {
                resourceWindow.CreateInternalResources += resourceWindow_CreateInternalResources;
                resourceWindow.DestroyInternalResources += resourceWindow_DestroyInternalResources;
            }
        }

        public string Name
        {
            get
            {
                return "SoundPlugin";
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

        public SoundManager SoundManager
        {
            get
            {
                return soundManager;
            }
        }

        void resourceWindow_CreateInternalResources(OSWindow window)
        {
            openALManager.resumeAudio();
        }

        void resourceWindow_DestroyInternalResources(OSWindow window)
        {
            openALManager.suspendAudio();
        }
    }
}
