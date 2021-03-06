﻿using System;
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

        public void setValue(string name, DateTime value)
        {
            setValue(name, NumberParser.ToString(value));
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

        public String getValue(String name, Func<String> defaultValFunc)
        {
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
                return strVal;
            }

            String defaultVal = defaultValFunc();
            setValue(name, defaultVal);
            return defaultVal;
        }

        public String getValue(String name, String defaultVal)
        {
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
                return strVal;
            }
            else
            {
                setValue(name, defaultVal);
            }
            return defaultVal;
        }

        public bool getValue(String name, Func<bool> defaultValFunc)
        {
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
                bool result;
                bool success = bool.TryParse(strVal, out result);
                if (success)
                {
                    return result;
                }
            }
            bool defaultVal = defaultValFunc();
            setValue(name, defaultVal);
            return defaultVal;
        }

        public bool getValue(String name, bool defaultVal)
        {
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        bool result;
		        bool success = bool.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        char result;
		        bool success = NumberParser.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        byte result;
		        bool success = NumberParser.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        short result;
		        bool success = NumberParser.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        ushort result;
		        bool success = UInt16.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        int result;
		        bool success = NumberParser.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        uint result;
		        bool success = UInt32.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        long result;
		        bool success = NumberParser.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        UInt32 result;
		        bool success = UInt32.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        float result;
		        bool success = NumberParser.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
		        double result;
		        bool success = NumberParser.TryParse(strVal, out result);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
                Vector3 result = new Vector3(); ;
		        bool success = result.setValue(strVal);
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
            String strVal;
            if (configValues.TryGetValue(name, out strVal))
            {
                Quaternion result = new Quaternion();
                bool success = result.setValue(strVal);
                if (success)
                {
                    return result;
                }
            }
	        setValue(name, defaultVal);
	        return defaultVal;
        }

        public DateTime getValue(String name, DateTime defaultVal)
        {
            DateTime retVal;
            String strVal;
            if(!configValues.TryGetValue(name, out strVal) || !NumberParser.TryParse(strVal, out retVal))
            {
                retVal = defaultVal;
                setValue(name, defaultVal);
            }
            return retVal;
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
		        //String[] values = line.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
                int keySplitIndex = line.IndexOf("=");
                if (keySplitIndex != -1)
                {
                    String key = line.Substring(0, keySplitIndex).Trim();
                    //Look for array value and put this value in the first index found if it is an array value
                    if (key.EndsWith("[]"))
                    {
                        key = key.Substring(0, key.Length - 2);
                        int i;
                        for (i = 0; hasValue(key + i); ++i) { }
                        key += i;
                    }
                    ++keySplitIndex;
                    if (keySplitIndex < line.Length)
                    {
                        setValue(key, line.Substring(keySplitIndex).Trim());
                    }
                    else //Null value
                    {
                        setValue(key, null);
                    }
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
