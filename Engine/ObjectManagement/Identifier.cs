using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// An Identifier is a way of identifying an object that is part of a subsystem. There are two major components to an Identifier instance.
    /// </summary>
    public class Identifier
    {
        #region Static

        public const string Separator = "/";
        private const string ELEMENT_NAME = "ElementName";
        private const string SIM_OBJECT_NAME = "SimObjectName";
        private static int SIM_OBJECT_OFFSET = SIM_OBJECT_NAME.Length + 1; //SimObjectName=
        private static int ELEMENT_OFFSET = ELEMENT_NAME.Length + 3; //, ElementName=

        #endregion Static

        #region Fields

        private string fullName;
        private string elementName;
        private string simObjectName;
        private string path;

        #endregion Fields

        #region Constructors

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
            this.simObjectName = instanceName;
            this.elementName = baseName;
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
            this.simObjectName = toCopy.simObjectName;
            this.elementName = toCopy.elementName;
            this.path = toCopy.path;
            this.fullName = toCopy.fullName;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Set the name of the object at once.  This is preferable to setting them both
        /// individually if the whole name is being chagned.
        /// </summary>
        /// <param name="simObjectName">The name of the SimObject.</param>
        /// <param name="elementName">The name of the SimObjectElement.</param>
        public void setName(String simObjectName, String elementName)
        {
            this.simObjectName = simObjectName;
            this.elementName = elementName;
            this.fullName = simObjectName + elementName;
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
            return typeof(Identifier) == obj.GetType() && this.fullName == ((Identifier)obj).fullName;
        }

        /// <summary>
        /// Returns a string formatted for use in the FromString function.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override String ToString()
        {
            return String.Format("{0}={1}, {2}={3}", SIM_OBJECT_NAME, simObjectName, ELEMENT_NAME, elementName);
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
                if (commaIndex != -1)
                {
                    simObjectName = str.Substring(SIM_OBJECT_OFFSET, commaIndex - SIM_OBJECT_OFFSET);
                    elementName = str.Substring(commaIndex + ELEMENT_OFFSET);
                    fullName = simObjectName + elementName;
                    return true;
                }
                else
                {
                    Log.Default.sendMessage("Invalid formatting on instance name \"{0}\" must be {1}=name, {2}=name.", LogLevel.Error, "Core", str, SIM_OBJECT_NAME, ELEMENT_NAME);
                }
            }
            else
            {
                Log.Default.sendMessage("Invalid instance name.  Input string is null or empty.", LogLevel.Error, "Core");
            }
            return false;
        }

        #endregion Functions

        #region Properties

        /// <summary>
	    /// The name of the SimElement.
	    /// </summary>
        public String ElementName 
        { 
            get
            {
                return elementName;
            }
            set
            {
                elementName = value;
	            fullName = simObjectName + elementName;
            }
        }

	    /// <summary>
	    /// The name of the SimObject.
	    /// </summary>
        public String SimObjectName 
	    { 
            get
            {
                return simObjectName;
            }
            set
            {
                simObjectName = value;
	            fullName = simObjectName + elementName;
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

        #endregion Properties
    }
}
