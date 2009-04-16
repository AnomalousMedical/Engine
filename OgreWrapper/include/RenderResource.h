#pragma once

namespace Ogre
{
	class Resource;
}

namespace Engine
{

namespace Rendering
{

[Engine::Attributes::DoNotSaveAttribute]
public ref class RenderResource abstract
{
public:
[Engine::Attributes::SingleEnum]
enum class LoadingState : unsigned int
{
	LOADSTATE_UNLOADED,
	LOADSTATE_LOADING,
	LOADSTATE_LOADED,
	LOADSTATE_UNLOADING,
	LOADSTATE_PREPARED,
	LOADSTATE_PREPARING
};

private:
	Ogre::Resource* ogreResource;

internal:
	RenderResource(Ogre::Resource* ogreResource);

public:
	/// <summary>
	/// Get the name of the resource.
	/// </summary>
	/// <returns>The name of the resource</returns>
	System::String^ getName();

	/// <summary>
	/// Get the resource handle.
	/// </summary>
	/// <returns>The resource handle.</returns>
	unsigned long getHandle();

	/// <summary>
	/// Get the resource group.
	/// </summary>
	/// <returns>The resource group.</returns>
	System::String^ getGroup();

	/// <summary>
	/// Prepares the resource for load, if it is not already. 
	/// </summary>
	void prepare();

	/// <summary>
	/// Loads the resource, if it is not already. 
	/// </summary>
	/// <param name="backgroundThread">True to load in the background thread.</param>
	void load(bool backgroundThread);

	/// <summary>
	/// Reloads the resource, if it is already loaded. 
	/// </summary>
	void reload();

	/// <summary>
	/// Check to see if the resource can be reloaded.
	/// </summary>
	/// <returns>True if the resource can be reloaded.  False if it cannot.</returns>
	bool isReloadable();

	/// <summary>
	/// Check to see if the resource is manually loaded.
	/// </summary>
	/// <returns>True if manually loaded.  False if not.</returns>
	bool isManuallyLoaded();

	/// <summary>
	/// Unloads the resource; this is not permanent, the resource can be reloaded later if required. 
	/// </summary>
	void unload();

	/// <summary>
	/// Retrieves info about the size of the resource. 
	/// </summary>
	/// <returns></returns>
	unsigned int getSize();

	/// <summary>
	/// 'Touches' the resource to indicate it has been used. 
	/// </summary>
	void touch();

	/// <summary>
	/// Check to see if the resource is prepared.
	/// </summary>
	/// <returns>Returns true if the Resource has been prepared, false otherwise.</returns>
	bool isPrepared();

	/// <summary>
	/// Check to see if the resource is loaded.
	/// </summary>
	/// <returns>Returns true if the Resource has been loaded, false otherwise.</returns>
	bool isLoaded();

	/// <summary>
	/// Returns whether the resource is currently in the process of background loading. 
	/// </summary>
	/// <returns>True if still loading.  False if loaded.</returns>
	bool isLoading();

	/// <summary>
	/// Returns the current loading state. 
	/// </summary>
	/// <returns>The current loading state.</returns>
	RenderResource::LoadingState getLoadingState();

	/// <summary>
	/// Returns whether this Resource has been earmarked for background loading. 
	/// </summary>
	/// <returns>True if this is set to load in the background.</returns>
	bool isBackgroundLoaded();

	/// <summary>
	/// Tells the resource whether it is background loaded or not. 
	/// </summary>
	/// <param name="bl">True to use background loading.  False to use inline loading.</param>
	void setBackgroundLoaded(bool bl);

	/// <summary>
	/// Escalates the loading of a background loaded resource. 
	/// 
	/// If a resource is set to load in the background, but something needs it before it's been 
	/// loaded, there could be a problem. If the user of this resource really can't wait, they can 
	/// escalate the loading which basically pulls the loading into the current thread immediately. 
	/// If the resource is already being loaded but just hasn't quite finished then this method 
	/// will simply wait until the background load is complete.
	/// </summary>
	void escalateLoading();

	/// <summary>
	/// Get the origin of this resource, e.g. a script file name. 
	/// </summary>
	/// <returns>The origin of the resource.</returns>
	System::String^ getOrigin();

	/// <summary>
	/// Returns the number of times this resource has changed state, which generally means the 
	/// number of times it has been loaded.
	/// 
	/// Objects that build derived data based on the resource can check this value against a copy 
	/// they kept last time they built this derived data, in order to know whether it needs 
	/// rebuilding. This is a nice way of monitoring changes without having a tightly-bound callback. 
	/// </summary>
	/// <returns>The number of times the resource has changed state.</returns>
	unsigned int getStateCount();

	/// <summary>
	/// Generic parameter setting method.
	/// 
    /// Call this method with the name of a parameter and a string version of the value to set. 
	/// The implementor will convert the string to a native type internally. If in doubt, check the 
	/// parameter definition in the list returned from StringInterface::getParameters. 
	/// </summary>
	/// <param name="name">The name of the parameter to set.</param>
	/// <param name="value">String value. Must be in the right format for the type specified in the parameter definition. See the StringConverter class for more information.</param>
	void setParameter(System::String^ name, System::String^ value);

	/// <summary>
	/// Generic parameter retrieval method.
	/// 
    /// Call this method with the name of a parameter to retrieve a string-format value of the 
	/// parameter in question. If in doubt, check the parameter definition in the list returned 
	/// from getParameters for the type of this parameter. If you like you can use StringConverter 
	/// to convert this string back into a native type. 
	/// </summary>
	/// <param name="name">The name of the parameter to get.</param>
	/// <returns>The parameter specified by name.</returns>
	System::String^ getParameter(System::String^ name);
};

}

}