﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace OgrePlugin
{
    /// <summary>
    /// This is the configuration for ogre.
    /// </summary>
    public class OgreConfig
    {
        private static ConfigSection ogreSection;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configFile">The ConfigFile to use.</param>
        internal OgreConfig(ConfigFile configFile)
        {
            if (ogreSection == null)
            {
                ogreSection = configFile.createOrRetrieveConfigSection("Ogre");
            }
            else
            {
                throw new Exception("OgreConfig can only be initailized one time");
            }
        }

        /// <summary>
        /// True if VSync is enabled. False if it is disabled.
        /// </summary>
        public static bool VSync
        {
            get
            {
                return ogreSection.getValue("VSync", true);
            }
            set
            {
                ogreSection.setValue("VSync", value);
            }
        }

        /// <summary>
        /// The Full Screen Anti Aliasing mode.
        /// </summary>
        public static int FSAA
        {
            get
            {
                return ogreSection.getValue("FSAA", 0);
            }
            set
            {
                ogreSection.setValue("FSAA", value);
            }
        }
    }
}
