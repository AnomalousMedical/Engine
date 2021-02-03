using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            mainTimer.removeUpdateListener(soundUpdate);
        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<OpenALManager>();
            serviceCollection.TryAddSingleton<SoundUpdateListener>();
            serviceCollection.TryAddSingleton<SoundManager>();
        }

        public void link(PluginManager pluginManager, IServiceScope globalScope)
        {
            var globalScopeProvider = globalScope.ServiceProvider;

            openALManager = globalScopeProvider.GetRequiredService<OpenALManager>();
            soundUpdate = globalScopeProvider.GetRequiredService<SoundUpdateListener>();

            this.mainTimer = globalScopeProvider.GetRequiredService<UpdateTimer>();
            mainTimer.addUpdateListener(soundUpdate);

            resourceWindow = globalScopeProvider.GetRequiredService<OSWindow>();
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

        void resourceWindow_CreateInternalResources(OSWindow window, InternalResourceType resourceType)
        {
            if ((resourceType & InternalResourceType.Sound) == InternalResourceType.Sound)
            {
                openALManager.resumeAudio();
            }
        }

        void resourceWindow_DestroyInternalResources(OSWindow window, InternalResourceType resourceType)
        {
            if ((resourceType & InternalResourceType.Sound) == InternalResourceType.Sound)
            {
                openALManager.suspendAudio();
            }
        }
    }
}
