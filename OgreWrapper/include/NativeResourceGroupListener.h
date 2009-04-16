#pragma once

#include "OgreResourceGroupManager.h"
#include <string>
#include "gcroot.h"

namespace Rendering{

ref class ManagedResourceGroupListener;
interface class ResourceGroupListener;

class NativeResourceGroupListener : Ogre::ResourceGroupListener
{
private:
	gcroot<ManagedResourceGroupListener^> managedListener;

public:
	NativeResourceGroupListener(void);

	~NativeResourceGroupListener(void);

	/// <summary>
	/// Add a listener.
	/// </summary>
	/// <param name="listener">The listener to add.</param>
	void addListener(gcroot<Rendering::ResourceGroupListener^> listener);

	/// <summary>
	/// Remove a listener.
	/// </summary>
	/// <param name="listener">The listener to remove.</param>
	void removeListener(gcroot<Rendering::ResourceGroupListener^> listener);

	/// <summary>
	/// This event is fired when a resource group begins parsing scripts.
	/// 
	/// Remember that if you are loading resources through
    /// ResourceBackgroundQueue, these callbacks will occur in the background
    /// thread, so you should not perform any thread-unsafe actions in this
    /// callback if that's the case (check the group name / script name).
	/// </summary>
	/// <param name="groupName">The name of the group.</param>
	/// <param name="scriptCount">The number of scripts which will be parsed.</param>
	virtual void resourceGroupScriptingStarted(const Ogre::String& groupName, size_t scriptCount);

	/// <summary>
	/// This event is fired when a script is about to be parsed. 
	/// </summary>
	/// <param name="scriptName">Name of the to be parsed.</param>
	/// <param name="skipThisScript">A boolean passed by reference which is by default set to false. If the event sets this to true, the script will be skipped and not parsed. Note that in this case the scriptParseEnded event will not be raised for this script.</param>
	virtual void scriptParseStarted(const Ogre::String& scriptName, bool& skipThisScript);

	/// <summary>
	/// This event is fired when the script has been fully parsed.
	/// </summary>
	/// <param name="scriptName">Name of the script that was parsed.</param>
	/// <param name="skipped">True if the script was skipped.</param>
	virtual void scriptParseEnded(const Ogre::String& scriptName, bool skipped);

	/// <summary>
	/// This event is fired when a resource group finished parsing scripts.
	/// </summary>
	/// <param name="groupName">The group that completed loading.</param>
	virtual void resourceGroupScriptingEnded(const Ogre::String& groupName);

	/// <summary>
	/// This event is fired when a resource group begins preparing.
	/// </summary>
	/// <param name="groupName">The name of the group being prepared.</param>
	/// <param name="resourceCount">The number of resources which will be prepared, including a number of stages required to prepare any linked world geometry.</param>
	virtual void resourceGroupPrepareStarted(const Ogre::String& groupName, size_t resourceCount);

	/// <summary>
	/// This event is fired when a declared resource is about to be prepared.
	/// </summary>
	/// <param name="resource">The resource being prepared.</param>
	virtual void resourcePrepareStarted(const Ogre::ResourcePtr& resource);

	/// <summary>
	/// This event is fired when the resource has been prepared.
	/// </summary>
	virtual void resourcePrepareEnded();

	/// <summary>
	/// This event is fired when a stage of preparing linked world geometry is
    /// about to start.
	/// 
	/// The number of stages required will have been included in the
    /// resourceCount passed in resourceGroupLoadStarted. 
	/// </summary>
	/// <param name="description">Text description of what is to be prepared.</param>
	virtual void worldGeometryPrepareStageStarted(const Ogre::String& description);

	/// <summary>
	/// This event is fired when a stage of preparing linked world geometry has
    /// been completed.
	/// 
	/// The number of stages required will have been included in the
    /// resourceCount passed in resourceGroupLoadStarted. 
	/// </summary>
	virtual void worldGeometryPrepareStageEnded();

	/// <summary>
	/// This event is fired when a resource group finished preparing.
	/// </summary>
	/// <param name="groupName">The name of the group that has finished preparing.</param>
	virtual void resourceGroupPrepareEnded(const Ogre::String& groupName);

	/// <summary>
	/// This event is fired when a resource group begins loading.
	/// </summary>
	/// <param name="groupName">The name of the group being loaded.</param>
	/// <param name="resourceCount">The number of resources which will be loaded, including a number of stages required to load any linked world geometry.</param>
	virtual void resourceGroupLoadStarted(const Ogre::String& groupName, size_t resourceCount);

	/// <summary>
	/// This event is fired when a declared resource is about to be loaded.
	/// </summary>
	/// <param name="resource">The resource being loaded.</param>
	virtual void resourceLoadStarted(const Ogre::ResourcePtr& resource);

	/// <summary>
	/// This event is fired when the resource has been loaded.
	/// </summary>
	virtual void resourceLoadEnded();

	/// <summary>
	/// This event is fired when a stage of loading linked world geometry is
    /// about to start.
	/// 
	/// The number of stages required will have been included in the
    /// resourceCount passed in resourceGroupLoadStarted. 
	/// </summary>
	/// <param name="description">Text description of what was just loaded.</param>
	virtual void worldGeometryStageStarted(const Ogre::String& description);

	/// <summary>
	/// This event is fired when a stage of loading linked world geometry has
    /// been completed.
	/// 
	/// The number of stages required will have been included in the
    /// resourceCount passed in resourceGroupLoadStarted. 
	/// </summary>
	virtual void worldGeometryStageEnded();

	/// <summary>
	/// This event is fired when a resource group finished loading.
	/// </summary>
	/// <param name="groupName"></param>
	virtual void resourceGroupLoadEnded(const Ogre::String& groupName);
};

}