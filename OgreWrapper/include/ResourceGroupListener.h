#pragma once

namespace OgreWrapper{

ref class OgreResource;

/// <summary>
/// This interface defines an interface which is called back during resource
/// group loading to indicate the progress of the load.
/// 
/// Resource group loading is in 2 phases - creating resources from declarations
/// (which includes parsing scripts), and loading resources. Note that you don't
/// necessarily have to have both; it is quite possible to just parse all the
/// scripts for a group (see ResourceGroupManager::initialiseResourceGroup, but
/// not to load the resource group. The sequence of events is (* signifies a
/// repeating item):
///     * resourceGroupScriptingStarted
///     * scriptParseStarted (*)
///     * scriptParseEnded (*)
///     * resourceGroupScriptingEnded
///     * resourceGroupLoadStarted
///     * resourceLoadStarted (*)
///     * resourceLoadEnded (*)
///     * worldGeometryStageStarted (*)
///     * worldGeometryStageEnded (*)
///     * resourceGroupLoadEnded
///     * resourceGroupPrepareStarted
///     * resourcePrepareStarted (*)
///     * resourcePrepareEnded (*)
///     * resourceGroupPrepareEnded
/// </summary>
interface class ResourceGroupListener
{
public:
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
	void resourceGroupScriptingStarted(System::String^ groupName, int scriptCount);

	/// <summary>
	/// This event is fired when a script is about to be parsed. 
	/// </summary>
	/// <param name="scriptName">Name of the to be parsed.</param>
	/// <param name="skipThisScript">A boolean passed by reference which is by default set to false. If the event sets this to true, the script will be skipped and not parsed. Note that in this case the scriptParseEnded event will not be raised for this script.</param>
	void scriptParseStarted(System::String^ scriptName, bool% skipThisScript);

	/// <summary>
	/// This event is fired when the script has been fully parsed.
	/// </summary>
	/// <param name="scriptName">Name of the script that was parsed.</param>
	/// <param name="skipped">True if the script was skipped.</param>
	void scriptParseEnded(System::String^ scriptName, bool skipped);

	/// <summary>
	/// This event is fired when a resource group finished parsing scripts.
	/// </summary>
	/// <param name="groupName">The group that completed loading.</param>
	void resourceGroupScriptingEnded(System::String^ groupName);

	/// <summary>
	/// This event is fired when a resource group begins preparing.
	/// </summary>
	/// <param name="groupName">The name of the group being prepared.</param>
	/// <param name="resourceCount">The number of resources which will be prepared, including a number of stages required to prepare any linked world geometry.</param>
	void resourceGroupPrepareStarted(System::String^ groupName, int resourceCount);

	/// <summary>
	/// This event is fired when a declared resource is about to be prepared.
	/// </summary>
	/// <param name="resource">The resource being prepared.</param>
	void resourcePrepareStarted(OgreResource^ resource);

	/// <summary>
	/// This event is fired when the resource has been prepared.
	/// </summary>
	void resourcePrepareEnded();

	/// <summary>
	/// This event is fired when a stage of preparing linked world geometry is
    /// about to start.
	/// 
	/// The number of stages required will have been included in the
    /// resourceCount passed in resourceGroupLoadStarted. 
	/// </summary>
	/// <param name="description">Text description of what is to be prepared.</param>
	void worldGeometryPrepareStageStarted(System::String^ description);

	/// <summary>
	/// This event is fired when a stage of preparing linked world geometry has
    /// been completed.
	/// 
	/// The number of stages required will have been included in the
    /// resourceCount passed in resourceGroupLoadStarted. 
	/// </summary>
	void worldGeometryPrepareStageEnded();

	/// <summary>
	/// This event is fired when a resource group finished preparing.
	/// </summary>
	/// <param name="groupName">The name of the group that has finished preparing.</param>
	void resourceGroupPrepareEnded(System::String^ groupName);

	/// <summary>
	/// This event is fired when a resource group begins loading.
	/// </summary>
	/// <param name="groupName">The name of the group being loaded.</param>
	/// <param name="resourceCount">The number of resources which will be loaded, including a number of stages required to load any linked world geometry.</param>
	void resourceGroupLoadStarted(System::String^ groupName, int resourceCount);

	/// <summary>
	/// This event is fired when a declared resource is about to be loaded.
	/// </summary>
	/// <param name="resource">The resource being loaded.</param>
	void resourceLoadStarted(OgreResource^ resource);

	/// <summary>
	/// This event is fired when the resource has been loaded.
	/// </summary>
	void resourceLoadEnded();

	/// <summary>
	/// This event is fired when a stage of loading linked world geometry is
    /// about to start.
	/// 
	/// The number of stages required will have been included in the
    /// resourceCount passed in resourceGroupLoadStarted. 
	/// </summary>
	/// <param name="description">Text description of what was just loaded.</param>
	void worldGeometryStageStarted(System::String^ description);

	/// <summary>
	/// This event is fired when a stage of loading linked world geometry has
    /// been completed.
	/// 
	/// The number of stages required will have been included in the
    /// resourceCount passed in resourceGroupLoadStarted. 
	/// </summary>
	void worldGeometryStageEnded();

	/// <summary>
	/// This event is fired when a resource group finished loading.
	/// </summary>
	/// <param name="groupName"></param>
	void resourceGroupLoadEnded(System::String^ groupName);
};

}