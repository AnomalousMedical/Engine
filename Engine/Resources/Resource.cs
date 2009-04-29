using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;

namespace Engine.Resources
{
    /// <summary>
    /// This describes the type of location the resource is saved into.
    /// </summary>
    public enum ResourceType : uint
    {
	    FileSystem = 0,
	    ZipArchive = 1,
    };

    /// <summary>
    /// A resource is an individual file or folder containing assets that need
    /// to be loaded into the engine.
    /// </summary>
    public class Resource : Saveable
    {
        #region Static

        private static String resourceRoot = ".";

        /// <summary>
	    /// This is the root directory where resources can be found.  This is static
	    /// and set for all resources.
	    /// </summary>
	    static String ResourceRoot 
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

        #endregion Fields

        #region Constructors

        /// <summary>
	    /// Constructor, should be used by the UI only.
	    /// </summary>
        public Resource()
        {

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
	    /// Determine if all the properties of these two resources match.
	    /// </summary>
	    /// <param name="checkAgainst">The resource to check against.</param>
	    /// <returns>True if all properties match.</returns>
        public bool allPropertiesMatch(Resource checkAgainst)
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
                return resourceRoot + "\\" + locName;
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
    }
}
