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
    public class Resource : Saveable
    {
        private String locName;
        private bool recursive;
        private String archiveType;
        private ResourceGroup group;

        /// <summary>
	    /// Constructor, should be used by the UI only.
	    /// </summary>
        internal Resource()
        {
            locName = "";
            recursive = false;
            archiveType = "EngineArchive";
        }

        /// <summary>
	    /// Constructor.
	    /// </summary>
	    /// <param name="locName">The location of the resource.</param>
	    /// <param name="type">The type of the resource.</param>
	    /// <param name="recursive">True to recurse subdirectories.</param>
        internal Resource(String locName, String archiveType, bool recursive)
        {
            this.locName = locName;
            this.archiveType = archiveType;
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
            this.archiveType = toCopy.archiveType;
        }

        /// <summary>
        /// Determine if this Resource points to a valid location. Returns true
        /// if it does.
        /// </summary>
        /// <returns>True if this resource points to a valid location.</returns>
        public bool isValid()
        {
            return VirtualFileSystem.Instance.exists(LocName);
        }

        /// <summary>
	    /// Determine if all the properties of these two resources match.
	    /// </summary>
	    /// <param name="checkAgainst">The resource to check against.</param>
	    /// <returns>True if all properties match.</returns>
        internal bool allPropertiesMatch(Resource checkAgainst)
        {
	        return locName == checkAgainst.locName && 
		        recursive == checkAgainst.recursive &&
                archiveType == checkAgainst.archiveType;
        }

        /// <summary>
        /// Set the ResourceGroup that owns this resource.
        /// </summary>
        /// <param name="group">The ResourceGroup that owns this resource.</param>
        internal void setResourceGroup(ResourceGroup group)
        {
            this.group = group;
        }

        /// <summary>
	    /// The location of the resource.
	    /// </summary>
	    public String LocName 
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

        public String ArchiveType
        {
            get
            {
                return archiveType;
            }
            set
            {
                archiveType = value;
            }
        }

        public ResourceGroup Group
        {
            get
            {
                return group;
            }
        }

        private const String LOC_NAME = "LocName";
        private const String RECURSIVE = "Recursive";
        private const String ARCHIVE_TYPE = "ArchiveType";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="info">The load info.</param>
        private Resource(LoadInfo info)
        {
            locName = info.GetString(LOC_NAME);
            recursive = info.GetBoolean(RECURSIVE);
            archiveType = info.GetString(ARCHIVE_TYPE, "EngineArchive");
        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info">Save info.</param>
        public void getInfo(SaveInfo info)
        {
            info.AddValue(LOC_NAME, locName);
            info.AddValue(RECURSIVE, recursive);
            info.AddValue(ARCHIVE_TYPE, archiveType);
        }
    }
}
