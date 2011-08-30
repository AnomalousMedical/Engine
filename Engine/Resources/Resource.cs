using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;
using System.IO;
using Engine.Attributes;

namespace Engine.Resources
{
    /// <summary>
    /// A resource is an individual file or folder containing assets that need
    /// to be loaded into the engine.
    /// </summary>
    public class Resource : Saveable, EditableProperty
    {
        #region Fields

        private String locName;
        private bool recursive;
        private ResourceGroup group;

        #endregion Fields

        #region Constructors

        /// <summary>
	    /// Constructor, should be used by the UI only.
	    /// </summary>
        internal Resource()
        {
            locName = "";
            recursive = false;
        }

        /// <summary>
	    /// Constructor.
	    /// </summary>
	    /// <param name="locName">The location of the resource.</param>
	    /// <param name="type">The type of the resource.</param>
	    /// <param name="recursive">True to recurse subdirectories.</param>
        internal Resource(String locName, bool recursive)
        {
            this.locName = locName;
            this.recursive = recursive;
        }

        /// <summary>
	    /// Constructor, copies an existing resource.
	    /// </summary>
	    /// <param name="toCopy">The resource to copy.</param>
        internal Resource(Resource toCopy)
        {
        	this.locName = toCopy.locName;
            this.recursive = toCopy.recursive;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Determine if this Resource points to a valid location. Returns true
        /// if it does.
        /// </summary>
        /// <returns>True if this resource points to a valid location.</returns>
        public bool isValid()
        {
            return VirtualFileSystem.Instance.exists(FullPath);
        }

        /// <summary>
	    /// Determine if all the properties of these two resources match.
	    /// </summary>
	    /// <param name="checkAgainst">The resource to check against.</param>
	    /// <returns>True if all properties match.</returns>
        internal bool allPropertiesMatch(Resource checkAgainst)
        {
	        return locName == checkAgainst.locName && 
		        recursive == checkAgainst.recursive;
        }

        /// <summary>
        /// Get the locName of this resource.
        /// </summary>
        /// <returns></returns>
        internal String getLocName()
        {
	        return locName;
        }

        /// <summary>
        /// Set the ResourceGroup that owns this resource.
        /// </summary>
        /// <param name="group">The ResourceGroup that owns this resource.</param>
        internal void setResourceGroup(ResourceGroup group)
        {
            this.group = group;
        }

        #endregion Functions

        #region Properties

        /// <summary>
	    /// This is the location on the file system of the resource relative to the root.
	    /// </summary>
	    public String RelativePath 
	    {
		    get
            {
                return locName;
            }
		    set
            {
                locName = value;
            }
	    }

	    /// <summary>
	    /// This is the absolute location of the resource on the file system.
	    /// </summary>
        public String FullPath 
	    {
		    get
            {
                return locName;
            }
	    }

	    /// <summary>
	    /// True if the resource should scan subdirectories.
	    /// </summary>
        public bool Recursive 
	    {
            get
            {
                return recursive;
            }
            set
            {
                recursive = value;
            }
        }

        #endregion Properties

        #region Saveable Members

        private const String LOC_NAME = "LocName";
        private const String RECURSIVE = "Recursive";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="info">The load info.</param>
        private Resource(LoadInfo info)
        {
            locName = info.GetString(LOC_NAME);
            recursive = info.GetBoolean(RECURSIVE);
        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info">Save info.</param>
        public void getInfo(SaveInfo info)
        {
            info.AddValue(LOC_NAME, locName);
            info.AddValue(RECURSIVE, recursive);
        }

        #endregion

        #region EditableProperty Members

        private const int LOC_COLUMN = 0;
        private const int RECURSIVE_COLUMN = 1;

        internal static readonly EditablePropertyInfo Info = new EditablePropertyInfo();

        static Resource()
        {
            Info.addColumn(new EditablePropertyColumn("Location", false));
            Info.addColumn(new EditablePropertyColumn("Recursive", false));
        }

        /// <summary>
        /// Get the value for a given column.
        /// </summary>
        /// <param name="column">The column to get the value for.</param>
        /// <returns></returns>
        public string getValue(int column)
        {
            switch (column)
            {
                case LOC_COLUMN:
                    return locName;
                case RECURSIVE_COLUMN:
                    return recursive.ToString();
            }
            throw new EditException(String.Format("Attempted to get a column from a Resource {0} that is not valid.", column));
        }

        /// <summary>
        /// Set the value of this property from a string.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void setValueStr(int column, string value)
        {
            switch (column)
            {
                case LOC_COLUMN:
                    //This exchange is complex, when the resource is 
                    //removed the group will be reset so we must keep a 
                    //local reference to it.
                    ResourceGroup localGroup = group;
                    group.removeResource(locName);
                    locName = value;
                    localGroup.addResource(this);
                    break;
                case RECURSIVE_COLUMN:
                    recursive = bool.Parse(value);
                    break;
                default:
                    throw new EditException(String.Format("Attempted to set a column from a Resource {0} that is not valid.", column));
            }
        }

        /// <summary>
        /// Determine if the given string is in the correct format for this
        /// property to parse.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value">The value to try to parse.</param>
        /// <param name="errorMessage">An error message if the function returns false.</param>
        /// <returns>True if the string can be parsed.</returns>
        public bool canParseString(int column, string value, out string errorMessage)
        {
            switch (column)
            {
                case LOC_COLUMN:
                    if (value == String.Empty || value == null)
                    {
                        errorMessage = null;
                        return true;
                    }
                    if (value != locName && group.containsResource(value))
                    {
                        errorMessage = "This resource location is already defined.";
                        return false;
                    }
                    String fullPath = value;
                    if (VirtualFileSystem.Instance.exists(fullPath))
                    {
                        errorMessage = null;
                        return true;
                    }
                    else
                    {
                        errorMessage = String.Format("Could not find resource path \"{0}\".", fullPath);
                        return false;
                    }
                case RECURSIVE_COLUMN:
                    errorMessage = "Cannot evaluate recursive value as a bool";
                    bool result;
                    return bool.TryParse(value, out result);
            }
            throw new EditException(String.Format("Attempted to validate a column from a Resource {0} that is not valid.", column));
        }

        /// <summary>
        /// Get the type of this property's target object.
        /// </summary>
        /// <param name="column"></param>
        /// <returns>The Type of the object this property will set.</returns>
        public Type getPropertyType(int column)
        {
            switch (column)
            {
                case LOC_COLUMN:
                    return typeof(String);
                case RECURSIVE_COLUMN:
                    return typeof(bool);
            }
            throw new EditException(String.Format("Attempted to get a column type from a Resource {0} that is not valid.", column));
        }

        public Browser getBrowser(int column)
        {
            return null;
        }

        public bool hasBrowser(int column)
        {
            return false;
        }

        #endregion
    }
}
