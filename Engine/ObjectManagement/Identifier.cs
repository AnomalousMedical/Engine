using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine
{
    class Identifier
    {
        private string fullName;
        private string baseName;
        private string instanceName;
        private string path;

        public const string Separator = "/";

       /// <summary>
	    /// Parameterless constructor.
	    /// </summary>
        public Identifier()
        {
        }

	    /// <summary>
	    /// Constructor, takes the instance name and the base name.
	    /// </summary>
	    /// <param name="instanceName">The instance name.</param>
	    /// <param name="baseName">The base name.</param>
        public Identifier(String instanceName, String baseName)
            :this(instanceName, baseName, null)
        {
            
        }

	    /// <summary>
	    /// Constructor, takes the instance name and the base name.
	    /// </summary>
	    /// <param name="instanceName">The instance name.</param>
	    /// <param name="baseName">The base name.</param>
	    /// <param name="path">The path to this identifier.</param>
        public Identifier(String instanceName, String baseName, String path)
        {
            this.instanceName = instanceName;
            this.baseName = baseName;
            this.fullName = instanceName + baseName;
            this.path = path;
        }

	    /// <summary>
	    /// Constructor, extracts the name out of a properly formatted string.
	    /// See FromString()
	    /// </summary>
	    /// <param name="nameString">The formatted string with the name.</param>
        public Identifier(String nameString)
        {
            FromString(nameString);
        }

	    /// <summary>
	    /// Constructor, duplicates a given Identifier so that the new object contains
	    /// the same values and will answer true to an equality test.
	    /// </summary>
	    /// <param name="toCopy">The instance variable to copy.</param>
        public Identifier(Identifier toCopy)
        {
            this.instanceName = toCopy.instanceName;
            this.baseName = toCopy.baseName;
            this.path = toCopy.path;
            this.fullName = toCopy.fullName;
        }

	    /// <summary>
	    /// The base name of the identifier.
	    /// </summary>
        public String BaseName 
        { 
            get
            {
                return baseName;
            }
            set
            {
                baseName = value;
	            fullName = instanceName + baseName;
            }
        }

	    /// <summary>
	    /// The instance name of the identifier.
	    /// </summary>
        public String InstanceName 
	    { 
            get
            {
                return instanceName;
            }
            set
            {
                instanceName = value;
	            fullName = instanceName + baseName;
            }
        }

	    /// <summary>
	    /// The path of this identifier.  Used to sort objects named with Identifiers.  
	    /// Separate folders with a '/'.
	    /// </summary>
        public String Path 
	    { 
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

	    /// <summary>
	    /// The full name which is InstanceName+BaseName, some subsystems still use flat
	    /// strings internally to identify objects so this should be passed as that string.
	    /// This name does not include the path.
	    /// </summary>
	    public String FullName 
	    {
		    get
            {
                return fullName;
            }
	    }

	    /// <summary>
	    /// Set the name of the object at once.  This is preferable to setting them both
	    /// individually if the whole name is being chagned.
	    /// </summary>
	    /// <param name="instanceName">The instance name.</param>
	    /// <param name="baseName">The base name.</param>
        public void setName(String instanceName, String baseName)
        {
            this.instanceName = instanceName;
            this.baseName = baseName;
            this.fullName = instanceName + baseName;
        }

	    /// <summary>
	    /// Calculates a hash code for the identifier.
	    /// </summary>
	    /// <returns>The hash for this object.</returns>
	    public override int GetHashCode()
        {
            return fullName.GetHashCode();
        }

	    /// <summary>
	    /// Determine if this Identifier equals the passed object.
	    /// </summary>
	    /// <param name="obj">The object to test.</param>
	    public override bool Equals(Object obj)
        {
            return typeof(Identifier) == obj.GetType() && this == ((Identifier)obj);
        }

	    /// <summary>
	    /// Returns a string formatted for use in the FromString function.
	    /// </summary>
	    /// <returns>The formatted string.</returns>
        public override String ToString()
        {
            return String.Format("InstanceName={0}, BaseName={1}", instanceName, baseName);
        }

	    /// <summary>
	    /// Parses a string in the format "InstanceName=0, BaseName=1"  where 0 is the
	    /// instance name and 1 is the base name.
	    /// </summary>
	    /// <param name="string">The string to parse.</param>
	    /// <returns>True if the string could be parsed, otherwise false.</returns>
        public bool FromString(String str)
        {
            if (str != null && str.Length > 0)
	        {
                int commaIndex = str.IndexOf(',');
		        if(commaIndex != -1)
		        {
                    instanceName = str.Substring(13, commaIndex - 13);
                    baseName = str.Substring(commaIndex + 11);
			        fullName = instanceName + baseName;
			        return true;
		        }
		        else
		        {
			        Log.Default.sendMessage("Invalid formatting on instance name \"{0}\" must be InstanceName=name, BaseName=name.", LogLevel.Error, "Core", str);
		        }
	        }
	        else
	        {
		       Log.Default.sendMessage("Invalid instance name.  Input string is null or empty.", LogLevel.Error, "Core");
	        }
	        return false;
        }
    }
}
