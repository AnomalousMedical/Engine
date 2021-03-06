﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomaly
{
    public class EngineConfig
    {
        private ConfigSection section;

        public EngineConfig(ConfigFile configFile)
        {
            section = configFile.createOrRetrieveConfigSection("Engine");
        }

        public int MaxFPS
        {
            get
            {
                return section.getValue("MaxFPS", 60);
            }
            set
            {
                section.setValue("MaxFPS", value);
            }
        }

        public int HorizontalRes
        {
            get
            {
                return section.getValue("HorizontalRes", 800);
            }
            set
            {
                section.setValue("HorizontalRes", value);
            }
        }

        public int VerticalRes
        {
            get
            {
                return section.getValue("VerticalRes", 600);
            }
            set
            {
                section.setValue("VerticalRes", value);
            }
        }

        public bool Fullscreen
        {
            get
            {
                return section.getValue("Fullscreen", false);
            }
            set
            {
                section.setValue("Fullscreen", value);
            }
        }
    }
}
