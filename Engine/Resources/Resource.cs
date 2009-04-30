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
    /// This describes the type of location the resource is saved into.
    /// </summary>
    [SingleEnum]
    public enum ResourceType : uint
    {
	    FileSystem = 0,
	    ZipArchive = 1,
    };

    /// <summary>
    /// A resource is an individual file or folder containing assets that need
    /// to be loaded into the engine.
    /// </summary>
    public class Resource : Saveable, EditableProperty
    {
        #region Static

        private static String resourceRoot = ".";

        /// <summary>
	    /// This is the root directory where resources can be found.  This is static
	    /// and set for all resources. This can be set to null to use absolute
        /// paths for everything. This is only reccomended for testing.
	    /// </summary>
	    public static String ResourceRoot 
	    {
		    get
            {
                return resourceRoot;
            }
		    set
            {
                resourceRoot = value;
            }
        }

        #endregion Static

        #region Fields

        private String locName;
        private ResourceType type;
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
            type = ResourceType.FileSystem;
            recursive = false;
        }

        /// <summary>
	    /// Constructor.
	    /// </summary>
	    /// <param name="locName">The location of the resource.</param>
	    /// <param name="type">The type of the resource.</param>
	    /// <param name="recursive">True to recurse subdirectories.</param>
        internal Resource(String locName, ResourceType type, bool recursive)
        {
            this.locName = locName;
            this.type = type;
            this.recursive = recursive;
        }

        /// <summary>
	    /// Constructor, copies an existing resource.
	    /// </summary>
	    /// <param name="toCopy">The resource to copy.</param>
        internal Resource(Resource toCopy)
        {
        	this.locName = toCopy.locName;
            this.type = toCopy.type;
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
            return File.Exists(FullPath) || Directory.Exists(FullPath);
        }

        /// <summary>
	    /// Determine if all the properties of these two resources match.
	    /// </summary>
	    /// <param name="checkAgainst">The resource to check against.</param>
	    /// <returns>True if all properties match.</returns>
        internal bool allPropertiesMatch(Resource checkAgainst)
        {
	        return locName == checkAgainst.locName && 
		        type == checkAgainst.type && 
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
                if (resourceRoot != null)
                {
                    return resourceRoot + "\\" + locName;
                }
                else
                {
                    return locName;
                }
            }
	    }

	    /// <summary>
	    /// This is the type of the resource.
	    /// </summary>
        public ResourceType Type 
	    {
		    get
            {
                return type;
            }
		    set
            {
                type = value;
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
        private const String TYPE = "Type";
        private const String RECURSIVE = "Recursive";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="info">The load info.</param>
        private Resource(LoadInfo info)
        {
            locName = info.GetString(LOC_NAME);
            type = info.GetValue<ResourceType>(TYPE);
            recursive = info.GetBoolean(RECURSIVE);
        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info">Save info.</param>
        public void getInfo(SaveInfo info)
        {
            info.AddValue(LOC_NAME, locName);
            info.AddValue(TYPE, type);
            info.AddValue(RECURSIVE, recursive);
        }

        #endregion

        #region EditableProperty Members

        private const int LOC_COLUMN = 0;
        private const int TYPE_COLUMN = 1;
        private const int RECURSIVE_COLUMN = 2;

        internal static readonly EditablePropertyInfo Info = new EditablePropertyInfo();

        static Resource()
        {
            Info.addColumn(new EditablePropertyColumn("Location", false));
            Info.addColumn(new EditablePropertyColumn("Type", false));
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
                case TYPE_COLUMN:
                    return type.ToString();
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
                case TYPE_COLUMN:
                    type = (ResourceType)Enum.Parse(typeof(ResourceType), value);
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
                    String fullPath;
                    if (resourceRoot == null)
                    {
                        fullPath = value;
                    }
                    else
                    {
                        fullPath = ResourceRoot + "\\" + value;
                    }
                    if (Directory.Exists(fullPath) || File.Exists(fullPath))
                    {
                        errorMessage = null;
                        return true;
                    }
                    else
                    {
                        errorMessage = String.Format("Could not find resource path \"{0}\".", fullPath);
                        return false;
                    }
                case TYPE_COLUMN:
                    try
                    {
                        Enum.Parse(typeof(ResourceType), value);
                    }
                    catch (ArgumentException)
                    {
                        errorMessage = "Invalid resource type";
                        return false;
                    }
                    errorMessage = null;
                    return true;
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
                case TYPE_COLUMN:
                    return typeof(ResourceType);
                case RECURSIVE_COLUMN:
                    return typeof(bool);
            }
            throw new EditException(String.Format("Attempted to get a column type from a Resource {0} that is not valid.", column));
        }

        #endregion
    }
}
