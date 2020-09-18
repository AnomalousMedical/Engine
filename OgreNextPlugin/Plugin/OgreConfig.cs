using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace OgreNextPlugin
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
                return ogreSection.getValue("VSync", false);
            }
            set
            {
                ogreSection.setValue("VSync", value);
            }
        }

        /// <summary>
        /// The Full Screen Anti Aliasing mode.
        /// </summary>
        public static String FSAA
        {
            get
            {
                return ogreSection.getValue("FSAA", "0");
            }
            set
            {
                ogreSection.setValue("FSAA", value);
            }
        }

        public static bool UseNvPerfHUD
        {
            get
            {
                return ogreSection.getValue("UseNvPerfHUD", false);
            }
            set
            {
                ogreSection.setValue("UseNvPerfHUD", value);
            }
        }

        public static RenderSystemType RenderSystemType
        {
            get
            {
                RenderSystemType type = OgreNextPlugin.RenderSystemType.Default;
                Enum.TryParse<RenderSystemType>(ogreSection.getValue("RenderSystemType", () => RenderSystemType.Default.ToString()), out type);
                return type;
            }
            set
            {
                ogreSection.setValue("RenderSystemType", value.ToString());
            }
        }

        /// <summary>
        /// Allow the microcode cache to save. It is set to true by default. 
        /// If this is set to false the cache will not be saved on shutdown.
        /// </summary>
        public static bool SaveMicrocodeCache
        {
            get
            {
                return ogreSection.getValue("SaveMicrocodeCache", true);
            }
            set
            {
                ogreSection.setValue("SaveMicrocodeCache", value);
            }
        }
    }
}
