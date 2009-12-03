using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Engine
{
    public delegate void ConfigEvent(ConfigSection source);

    public class ConfigSection
    {
        public event ConfigEvent SectionSaving;
        public event ConfigEvent SectionLoaded;

        private String name;
        private Dictionary<String, String> configValues = new Dictionary<string,string>();
        private String[] sep = { "=" };
        private bool hidden = false; //Determine if this section is written to config files, note this section will not ever be hidden if it exists in a config file

        /// <summary>
        /// Default constructor. Always visible.
        /// </summary>
        /// <param name="name">The name of the config section.</param>
        public ConfigSection(String name)
        {
            this.name = name;
        }

        /// <summary>
        /// Constructor. If hidden is true this file will not be written to the
        /// config file. Please note, however, that if the hidden section is in
        /// the config file this constructor will never be called and that
        /// section will be written anyway.
        /// </summary>
        /// <param name="name">The name of the config section.</param>
        /// <param name="hidden">If true this section will not be written to the config file.</param>
        public ConfigSection(String name, bool hidden)
        {
            this.name = name;
            this.hidden = hidden;
        }

        public void setValue(String name, String value)
        {
            if (!configValues.ContainsKey(name))
            {
                configValues.Add(name, value);
            }
            else
            {
                configValues[name] = value;
            }
        }

        public void setValue(String name, bool value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, char value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, byte value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, short value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, ushort value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, int value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, uint value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, long value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, ulong value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, float value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, double value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, Vector3 value)
        {
	        setValue(name, value.ToString());
        }

        public void setValue(String name, Quaternion value)
        {
	        setValue(name, value.ToString());
        }

        public void removeValue(String name)
        {
            if (configValues.ContainsKey(name))
            {
                configValues.Remove(name);
            }
        }

        public bool hasValue(string name)
        {
            return configValues.ContainsKey(name);
        }

        public String getValue(String name, String defaultVal)
        {
            if (configValues.ContainsKey(name))
            {
                return configValues[name];
            }
            else
            {
                setValue(name, defaultVal);
            }
            return defaultVal;
        }

        public bool getValue(String name, bool defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        bool result;
		        bool success = bool.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public char getValue(String name, char defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        char result;
		        bool success = char.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public byte getValue(String name, byte defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        byte result;
		        bool success = Byte.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public short getValue(String name, short defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        short result;
		        bool success = short.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public ushort getValue(String name, ushort defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        ushort result;
		        bool success = UInt16.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public int getValue(String name, int defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        int result;
		        bool success = int.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public uint getValue(String name, uint defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        uint result;
		        bool success = UInt32.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public long getValue(String name, long defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        long result;
		        bool success = long.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public ulong getValue(String name, ulong defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        UInt32 result;
		        bool success = UInt32.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public float getValue(String name, float defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        float result;
		        bool success = float.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public double getValue(String name, double defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        double result;
		        bool success = double.TryParse(configValues[name], out result);
		        if(success)
		        {
			        return result;
		        }
            }
            setValue(name, defaultVal);
	        return defaultVal;
        }

        public Vector3 getValue(String name, Vector3 defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
                Vector3 result = new Vector3(); ;
		        bool success = result.setValue(configValues[name]);
		        if(success)
		        {
			        return result;
		        }
            }
	        setValue(name, defaultVal);
	        return defaultVal;
        }

        public Quaternion getValue(String name, Quaternion defaultVal)
        {
	        if (configValues.ContainsKey(name))
            {
		        Quaternion result = new Quaternion();
		        bool success = result.setValue(configValues[name]);
		        if(success)
		        {
			        return result;
		        }
            }
	        setValue(name, defaultVal);
	        return defaultVal;
        }

        public void clearValues()
        {
            configValues.Clear();
        }

        internal void writeSection(StreamWriter writer)
        {
            if (SectionSaving != null)
            {
                SectionSaving.Invoke(this);
            }
            if (!hidden)
            {
                writer.WriteLine("[{0}]", Name);
                foreach (String key in configValues.Keys)
                {
                    writer.WriteLine("{0}{1}{2}", key, sep[0], configValues[key]);
                }
            }
        }

        /// <summary>
        /// Read a section and return the name of the next section when it is encountered.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>The name of the next section or null if no more sections are found.</returns>
        internal String readSection(StreamReader reader)
        {
            String line;
            line = reader.ReadLine();
	        while (line != null && !line.StartsWith(ConfigFile.SECTION_HEADER))
            {
		        String[] values = line.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 2)
                {
                    setValue(values[0].Trim(), values[1].Trim());
                }
                else if (values.Length == 1) //length of 1 means a null value
                {
                    setValue(values[0].Trim(), null);
                }
                line = reader.ReadLine();
            }
            if (SectionLoaded != null)
            {
                SectionLoaded.Invoke(this);
            }
            return line;
        }

        /// <summary>
        /// The name of this section.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }
}
