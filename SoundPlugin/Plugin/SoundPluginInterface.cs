﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;

namespace SoundPlugin
{
    public class SoundPluginInterface : PluginInterface
    {
        private OpenALManager openALManager = null;
        private SoundUpdateListener soundUpdate;
        private UpdateTimer mainTimer;
        private SoundManager soundManager;

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
                mainTimer.removeFixedUpdateListener(soundUpdate);
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

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.mainTimer = mainTimer;
            mainTimer.addFixedUpdateListener(soundUpdate);
        }

        public string getName()
        {
            return "SoundPlugin";
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }

        public SoundManager SoundManager
        {
            get
            {
                return soundManager;
            }
        }
    }
}
