using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Logging;

namespace Engine
{
    public class ConfigFile
    {
        private Dictionary<String, ConfigSection> sections = new Dictionary<string,ConfigSection>();
        private String backingFile;

        internal static String SECTION_HEADER = "[";

        public ConfigFile(String backingFile)
        {
            this.backingFile = backingFile;
        }

        public ConfigSection createOrRetrieveConfigSection(String name)
        {
            if(!sections.ContainsKey(name))
            {
                sections.Add(name, new ConfigSection(name));
            }
            return sections[name];
        }

        public void destroyConfigSection(String name)
        {
            if (sections.ContainsKey(name))
            {
                sections.Remove(name);
            }
        }

        public void writeConfigFile()
        {
            StreamWriter writer = null;
            try
            {
		        writer = new StreamWriter(new BufferedStream(File.Open(BackingFile, FileMode.Create, FileAccess.ReadWrite)));
                foreach (ConfigSection section in sections.Values)
                {
                    section.writeSection(writer);
                }
            }
            catch(Exception ex)
            {
		        Log.Default.sendMessage("Error saving config file {0}.\n{1}.", LogLevel.Error, "Config", backingFile, ex.Message);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public void loadConfigFile()
        {
            StreamReader reader = null;
            try
            {
		        reader = new StreamReader(new BufferedStream(File.Open(BackingFile, FileMode.Open, FileAccess.Read)));
                String line = reader.ReadLine();
                while (line != null && !line.StartsWith(SECTION_HEADER))
                {
                    line = reader.ReadLine();
                }
                if (line != null)
                {
			        line = line.Replace("[", String.Empty).Replace("]", String.Empty);
                    ConfigSection section = createOrRetrieveConfigSection(line);
                    line = section.readSection(reader);
                    while (line != null)
                    {
				        line = line.Replace("[", String.Empty).Replace("]", String.Empty);
                        section = createOrRetrieveConfigSection(line);
                        line = section.readSection(reader);
                    }
                }
            }
            catch (Exception ex)
            {
		        Log.Default.sendMessage("Error loading config file {0}.\n{1}.", LogLevel.Error, "Config", backingFile, ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void clearSections()
        {
	        sections.Clear();
        }

        public IEnumerable<ConfigSection> getSectionIterator()
        {
	        return sections.Values;
        }

        public String BackingFile
        {
            get
            {
                return backingFile;
            }
            set
            {
                backingFile = value;
            }
        }
    }
}
