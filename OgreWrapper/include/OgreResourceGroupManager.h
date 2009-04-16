#pragma once

namespace Ogre
{
	class ResourceGroupManager;
}

namespace OgreWrapper
{

value class ResourceDeclaration;
ref class FileInfo;

typedef System::Collections::Generic::IEnumerable<System::String^> GroupEnum;
typedef System::Collections::Generic::IEnumerable<ResourceDeclaration> DeclarationEnum;
typedef System::Collections::Generic::IEnumerable<System::String^> ResourceNameEnum;
typedef System::Collections::Generic::List<FileInfo^> FileInfoEnum;

public delegate void ResourcesInitialized();

/// <summary>
/// This class wraps Ogre's ResourceGroupManager class.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class OgreResourceGroupManager
{
private:
	static OgreResourceGroupManager^ instance = gcnew OgreResourceGroupManager();

	Ogre::ResourceGroupManager* resourceManager;
	ResourcesInitialized^ onResourcesInitialized;
	OgreResourceGroupManager(void);

public:
	static OgreResourceGroupManager^ getInstance();

	/// <summary>
	/// Create a resource group.
	/// </summary>
	/// <param name="name">The name to give the resource group. </param>
	void createResourceGroup(System::String^ name);

	/// <summary>
	/// Initialize all resources that need it.  Should be called whenever the resources are changed.
	/// </summary>
	void initializeAllResourceGroups();

	void destroyResourceGroup(System::String^ name);

	/// <summary>
	/// Returns the group the specified resource belongs to or null if it does not exist.
	/// </summary>
	/// <param name="name">The fully qualified name of the resource.</param>
	/// <param name="locType">The type of location the resource is located on.  Typically "FileSystem"</param>
	/// <param name="group">The group the resource belongs to.</param>
	/// <param name="recursive">True to search subdirectories.</param>
	void addResourceLocation(System::String^ name, System::String^ locType, System::String^ group, bool recursive);

	/// <summary>
	/// Removes a resource location from the search path. 
	/// </summary>
	/// <param name="name">The location to remove.</param>
	/// <param name="resGroup">The resource group of the location to remove.</param>
	void removeResourceLocation(System::String^ name, System::String^ resGroup);

	/// <summary>
	/// Get a list of the currently defined resource groups.
	/// </summary>
	/// <returns>An enum of all resource groups.</returns>
	GroupEnum^ getResourceGroups();

	/// <summary>
	/// Get the list of resource declarations for the specified group name. 
	/// </summary>
	/// <param name="groupName">The name of the group.</param>
	/// <returns>An enum of currently defined resources.</returns>
	DeclarationEnum^ getResourceDeclarationList(System::String^ groupName);

	/// <summary>
	/// List all file or directory names in a resource group.
	/// This method only returns filenames, you can also retrieve other information using listFileInfo. 
	/// </summary>
	/// <param name="groupName">The name of the group.</param>
	/// <returns></returns>
	ResourceNameEnum^ listResourceNames(System::String^ groupName);

	/// <summary>
	/// List all file or directory names in a resource group.
	/// This method only returns filenames, you can also retrieve other information using listFileInfo. 
	/// </summary>
	/// <param name="groupName">The name of the group.</param>
	/// <param name="dirs">If true, directory names will be returned instead of file names.</param>
	/// <returns>An enum of resource names.</returns>
	ResourceNameEnum^ listResourceNames(System::String^ groupName, bool dirs);

	FileInfoEnum^ listResourceFileInfo(System::String^ groupName, bool dirs);

	FileInfoEnum^ findResourceFileInfo(System::String^ group, System::String^ pattern, bool dirs);

	/// <summary>
	/// Returns the group the specified resource belongs to or null if it does not exist.
	/// </summary>
	/// <param name="resourceName">The fully qualified name of the resource.</param>
	/// <returns>The name of the group containg the resource or null if the resource was not found.</returns>
	System::String^ findGroupContainingResource(System::String^ resourceName);

	/// <summary>
	/// Called when the resources are initialized.
	/// </summary>
	event ResourcesInitialized^ OnResourcesInitialized
	{
        void add(ResourcesInitialized^ value)
		{
			onResourcesInitialized = (ResourcesInitialized^)System::Delegate::Combine(onResourcesInitialized, value);
        }
        void remove(ResourcesInitialized^ value)
		{
			onResourcesInitialized = (ResourcesInitialized^)System::Delegate::Remove(onResourcesInitialized, value);
        }
    }
};

}