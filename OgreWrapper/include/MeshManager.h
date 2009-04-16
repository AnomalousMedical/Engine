#pragma once

#include "HardwareBuffer.h"
#include "MeshPtr.h"

namespace Ogre
{
	class MeshManager;
	class Mesh;
}

namespace Engine{

namespace Rendering{

ref class Mesh;

/// <summary>
/// Handles the management of mesh resources.
/// <para>
/// This class deals with the runtime management of mesh data; like other
/// resource managers it handles the creation of resources (in this case mesh
/// data), working within a fixed memory budget. 
/// </para>
/// <para>
/// The wrapper also includes a centralized method to identify ogre meshes
/// so that the same Mesh class is returned on each request for an ogre mesh
/// see getMeshObject.
/// </para>
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class MeshManager
{
private:
	MeshPtrCollection meshPtrs;

	Ogre::MeshManager* meshManager;
	static MeshManager^ instance = gcnew MeshManager();

	MeshManager();

internal:
	MeshPtr^ getObject(const Ogre::MeshPtr& ogrePtr);

public:
	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~MeshManager();

	/// <summary>
	/// Get the instance of this MeshManager.
	/// </summary>
	/// <returns>The MeshManager instance.</returns>
	static MeshManager^ getInstance();

	/// <summary>
	/// Prepares a mesh for loading from a file. This does the IO in advance of
    /// the call to load(). If the model has already been created (prepared or
    /// loaded), the existing instance will be returned. This version will set
	/// the vertexBufferUsage and indexBufferUsage to
    /// Usage::HBU_STATIC_WRITE_ONLY and the vertexBufferShadowed and
    /// indexBufferShadowed variables to true.
	/// </summary>
	/// <param name="filename">The name of the .mesh file.</param>
	/// <param name="group">The name of the resource group to assign the mesh to.</param>
	/// <returns>The mesh.</returns>
	MeshPtr^ prepare(System::String^ filename, System::String^ group);

	/// <summary>
	/// Prepares a mesh for loading from a file. This does the IO in advance of
    /// the call to load(). If the model has already been created (prepared or
    /// loaded), the existing instance will be returned. This version will set
    /// the vertexBufferShadowed and indexBufferShadowed variables to true.
	/// </summary>
	/// <param name="filename">The name of the .mesh file.</param>
	/// <param name="group">The name of the resource group to assign the mesh to.</param>
	/// <param name="vertexBufferUsage">The usage flags with which the vertex buffer(s) will be created.</param>
	/// <param name="indexBufferUsage">The usage flags with which the index buffer(s) created for this mesh will be created with. </param>
	/// <returns>The mesh.</returns>
	MeshPtr^ prepare(System::String^ filename, System::String^ group, HardwareBuffer::Usage vertexBufferUsage, HardwareBuffer::Usage indexBufferUsage);

	/// <summary>
	/// Prepares a mesh for loading from a file. This does the IO in advance of
    /// the call to load(). If the model has already been created (prepared or
    /// loaded), the existing instance will be returned. 
	/// </summary>
	/// <param name="filename">The name of the .mesh file.</param>
	/// <param name="group">The name of the resource group to assign the mesh to.</param>
	/// <param name="vertexBufferUsage">The usage flags with which the vertex buffer(s) will be created.</param>
	/// <param name="indexBufferUsage">The usage flags with which the index buffer(s) created for this mesh will be created with. </param>
	/// <param name="vertexBufferShadowed">If true, the vertex buffers will be shadowed by system memory copies for faster read access.</param>
	/// <param name="indexBufferShadowed">If true, the index buffers will be shadowed by system memory copies for faster read access.</param>
	/// <returns>The mesh.</returns>
	MeshPtr^ prepare(System::String^ filename, System::String^ group, HardwareBuffer::Usage vertexBufferUsage, HardwareBuffer::Usage indexBufferUsage, bool vertexBufferShadowed, bool indexBufferShadowed);

	/// <summary>
	/// Loads a mesh from a file, making it immediately available for use. If
    /// the model has already been created (prepared or loaded), the existing
    /// instance will be returned. This version will set the vertexBufferUsage
	/// and indexBufferUsage to Usage::HBU_STATIC_WRITE_ONLY and the
    /// vertexBufferShadowed and indexBufferShadowed variables to true.
	/// </summary>
	/// <param name="filename">The name of the .mesh file.</param>
	/// <param name="group">The name of the resource group to assign the mesh to.</param>
	/// <returns>The mesh.</returns>
	MeshPtr^ load(System::String^ filename, System::String^ group);

	/// <summary>
	/// Loads a mesh from a file, making it immediately available for use. If
    /// the model has already been created (prepared or loaded), the existing
    /// instance will be returned. This version will set the
    /// vertexBufferShadowed and indexBufferShadowed variables to true.
	/// </summary>
	/// <param name="filename">The name of the .mesh file.</param>
	/// <param name="group">The name of the resource group to assign the mesh to.</param>
	/// <param name="vertexBufferUsage">The usage flags with which the vertex buffer(s) will be created.</param>
	/// <param name="indexBufferUsage">The usage flags with which the index buffer(s) created for this mesh will be created with. </param>
	/// <returns>The mesh.</returns>
	MeshPtr^ load(System::String^ filename, System::String^ group, HardwareBuffer::Usage vertexBufferUsage, HardwareBuffer::Usage indexBufferUsage);

	/// <summary>
	/// Loads a mesh from a file, making it immediately available for use. If
    /// the model has already been created (prepared or loaded), the existing
    /// instance will be returned. 
	/// </summary>
	/// <param name="filename">The name of the .mesh file.</param>
	/// <param name="group">The name of the resource group to assign the mesh to.</param>
	/// <param name="vertexBufferUsage">The usage flags with which the vertex buffer(s) will be created.</param>
	/// <param name="indexBufferUsage">The usage flags with which the index buffer(s) created for this mesh will be created with. </param>
	/// <param name="vertexBufferShadowed">If true, the vertex buffers will be shadowed by system memory copies for faster read access.</param>
	/// <param name="indexBufferShadowed">If true, the index buffers will be shadowed by system memory copies for faster read access.</param>
	/// <returns>The mesh.</returns>
	MeshPtr^ load(System::String^ filename, System::String^ group, HardwareBuffer::Usage vertexBufferUsage, HardwareBuffer::Usage indexBufferUsage, bool vertexBufferShadowed, bool indexBufferShadowed);

	//Mesh^ createManual(System::String^ name, System::String^ groupName);

	/// <summary>
	/// Get a mesh by its name.
	/// </summary>
	/// <param name="name">The name of the mesh.</param>
	/// <returns>The mesh specified by name or null if the mesh does not exist.</returns>
	MeshPtr^ getByName(System::String^ name);

	/// <summary>
	/// Retrieves a pointer to a resource by handle, or null if the resource does not exist. 
	/// </summary>
	/// <param name="handle">The handle to the resource to recover.</param>
	/// <returns>The resource identified by handle or null if it does not exist.</returns>
	MeshPtr^ getByHandle(unsigned long handle);

	/// <summary>
	/// Tells the mesh manager that all future meshes should prepare themselves
    /// for shadow volumes on loading. 
	/// </summary>
	/// <param name="enable">True to enable.</param>
	void setPrepareAllMeshesForShadowVolumes(bool enable);

	/// <summary>
	/// Retrieves whether all Meshes should prepare themselves for shadow volumes. 
	/// </summary>
	/// <returns>True if enabled.</returns>
	bool getPrepareAllMeshesForShadowVolumes();

	/// <summary>
	/// Gets the factor by which the bounding box of an entity is padded.
    /// Default is 0.01.
	/// </summary>
	/// <returns>The padding factor.</returns>
	float getBoundsPaddingFactor();

	/// <summary>
	/// Sets the factor by which the bounding box of an entity is padded. 
	/// </summary>
	/// <param name="paddingFactor">The new padding factor to set.</param>
	void setBoundsPaddingFactor(float paddingFactor);

	/// <summary>
	/// Set a limit on the amount of memory this resource handler may use.
	/// <para>
	/// If, when asked to load a new resource, the manager believes it will
    /// exceed this memory budget, it will temporarily unload a resource to make
    /// room for the new one. This unloading is not permanent and the Resource
    /// is not destroyed; it simply needs to be reloaded when next used. 
	/// </para>
	/// </summary>
	/// <param name="bytes">The maximum amount of memory to use.</param>
	void setMemoryBudget(size_t bytes);

	/// <summary>
	/// Get the limit on the amount of memory this resource handler may use. 
	/// </summary>
	/// <returns>The amount of memory this manager may use.</returns>
	size_t getMemoryBudget();

	/// <summary>
	/// Gets the current memory usage, in bytes. 
	/// </summary>
	/// <returns>The amount of memory used in bytes.</returns>
	size_t getMemoryUsage();

	/// <summary>
	/// Unloads a single resource by name.
	/// <para>
	/// Unloaded resources are not removed, they simply free up their memory as
    /// much as they can and wait to be reloaded. 
	/// </para>
	/// </summary>
	/// <param name="name">The name of the resource to unload.</param>
	void unload(System::String^ name);

	/// <summary>
	/// Unloads a single resource by handle. 
	/// <para>
	/// Unloaded resources are not removed, they simply free up their memory as
    /// much as they can and wait to be reloaded. 
	/// </para>
	/// </summary>
	/// <param name="handle"></param>
	void unload(unsigned long handle);

	/// <summary>
	/// Unloads all resources that are reloadable.
	/// <para>
	/// Unloaded resources are not removed, they simply free up their memory as
    /// much as they can and wait to be reloaded. 
	/// </para>
	/// </summary>
	void unloadAll();

	/// <summary>
	/// Unloads all resources.
	/// <para>
	/// Unloaded resources are not removed, they simply free up their memory as
    /// much as they can and wait to be reloaded. 
	/// </para>
	/// </summary>
	/// <param name="reloadableOnly">If true (the default), only unload the resource that is reloadable. Because some resources isn't reloadable, they will be unloaded but can't load them later. Thus, you might not want to them unloaded. Or, you might unload all of them, and then populate them manually later.</param>
	void unloadAll(bool reloadableOnly);

	/// <summary>
	/// Caused all currently loaded resources to be reloaded that are
    /// reloadable. All resources currently being held in this manager which
    /// are also marked as currently loaded will be unloaded, then loaded again. 
	/// </summary>
	void reloadAll();

	/// <summary>
	/// Caused all currently loaded resources to be reloaded. All resources
    /// currently being held in this manager which are also marked as currently
    /// loaded will be unloaded, then loaded again. 
	/// </summary>
	/// <param name="reloadableOnly">	If true (the default), only reload the resource that is reloadable. Because some resources isn't reloadable, they will be unloaded but can't loaded again. Thus, you might not want to them unloaded. Or, you might unload all of them, and then populate them manually later.</param>
	void reloadAll(bool reloadableOnly);

	/// <summary>
	/// Unload all resources which are not referenced by any other object that
    /// are realoadable.
	/// <para>
	/// This method behaves like unloadAll, except that it only unloads
    /// resources which are not in use, ie not referenced by other objects. This
    /// allows you to free up some memory selectively whilst still keeping the
    /// group around (and the resources present, just not using much memory). 
	/// </para>
	/// <para>
    /// Some referenced resource may exists 'weak' pointer to their
    /// sub-components (e.g. Entity held pointer to SubMesh), in this case,
    /// unload or reload that resource will cause dangerous pointer access. Use
    /// this function instead of unloadAll allows you avoid fail in those
    /// situations. 
	/// </para>
	/// </summary>
	void unloadUnreferencedResources();

	/// <summary>
	/// Unload all resources which are not referenced by any other object.
	/// <para>
	/// This method behaves like unloadAll, except that it only unloads
    /// resources which are not in use, ie not referenced by other objects. This
    /// allows you to free up some memory selectively whilst still keeping the
    /// group around (and the resources present, just not using much memory). 
	/// </para>
	/// <para>
    /// Some referenced resource may exists 'weak' pointer to their
    /// sub-components (e.g. Entity held pointer to SubMesh), in this case,
    /// unload or reload that resource will cause dangerous pointer access. Use
    /// this function instead of unloadAll allows you avoid fail in those
    /// situations. 
	/// </para>
	/// </summary>
	/// <param name="reloadableOnly">If true (the default), only unloads resources which can be subsequently automatically reloaded.</param>
	void unloadUnreferencedResources(bool reloadableOnly);

	/// <summary>
	/// Causes all currently loaded but not referenced by any other object
    /// resources to be reloaded that can be reloaded.
	/// <para>
	/// This method behaves like reloadAll, except that it only reloads
    /// resources which are not in use, i.e. not referenced by other objects. 
	/// </para>
	/// <para>
    /// Some referenced resource may exists 'weak' pointer to their
    /// sub-components (e.g. Entity held pointer to SubMesh), in this case,
    /// unload or reload that resource will cause dangerous pointer access. Use
    /// this function instead of reloadAll allows you avoid fail in those
    /// situations. 
	/// </para>
	/// </summary>
	void reloadUnreferencedResources();

	/// <summary>
	/// Causes all currently loaded but not referenced by any other object
    /// resources to be reloaded.
	/// <para>
	/// This method behaves like reloadAll, except that it only reloads
    /// resources which are not in use, i.e. not referenced by other objects. 
	/// </para>
	/// <para>
    /// Some referenced resource may exists 'weak' pointer to their
    /// sub-components (e.g. Entity held pointer to SubMesh), in this case,
    /// unload or reload that resource will cause dangerous pointer access. Use
    /// this function instead of reloadAll allows you avoid fail in those
    /// situations. 
	/// </para>
	/// </summary>
	/// <param name="reloadableOnly">If true (the default), only reloads resources which can be subsequently automatically reloaded. </param>
	void reloadUnreferencedResources(bool reloadableOnly);

	/// <summary>
	/// Remove a single resource.
	/// <para>
	/// Removes a single resource, meaning it will be removed from the list of
    /// valid resources in this manager, also causing it to be unloaded. 
	/// </para>
	/// <para>
    /// The word 'Destroy' is not used here, since if any other pointers are
    /// referring to this resource, it will persist until they have finished
    /// with it; however to all intents and purposes it no longer exists and
    /// will likely get destroyed imminently.
	/// </para>
	/// <para>
    /// If you do have shared pointers to resources hanging around after the
    /// ResourceManager is destroyed, you may get problems on destruction of
    /// these resources if they were relying on the manager (especially if it is
    /// a plugin). If you find you get problems on shutdown in the destruction
    /// of resources, try making sure you release all your shared pointers
    /// before you shutdown OGRE. 
	/// </para>
	/// </summary>
	/// <param name="r"></param>
	void remove(MeshPtr^ r);

	/// <summary>
	/// Remove a single resource by name.
	/// <para>
    /// The word 'Destroy' is not used here, since if any other pointers are
    /// referring to this resource, it will persist until they have finished
    /// with it; however to all intents and purposes it no longer exists and
    /// will likely get destroyed imminently.
	/// </para>
	/// <para>
    /// If you do have shared pointers to resources hanging around after the
    /// ResourceManager is destroyed, you may get problems on destruction of
    /// these resources if they were relying on the manager (especially if it is
    /// a plugin). If you find you get problems on shutdown in the destruction
    /// of resources, try making sure you release all your shared pointers
    /// before you shutdown OGRE. 
	/// </para>
	/// </summary>
	/// <param name="name"></param>
	void remove(System::String^ name);

	/// <summary>
	/// Remove a single resource by handle. 
	/// <para>
    /// The word 'Destroy' is not used here, since if any other pointers are
    /// referring to this resource, it will persist until they have finished
    /// with it; however to all intents and purposes it no longer exists and
    /// will likely get destroyed imminently.
	/// </para>
	/// <para>
    /// If you do have shared pointers to resources hanging around after the
    /// ResourceManager is destroyed, you may get problems on destruction of
    /// these resources if they were relying on the manager (especially if it is
    /// a plugin). If you find you get problems on shutdown in the destruction
    /// of resources, try making sure you release all your shared pointers
    /// before you shutdown OGRE. 
	/// </para>
	/// </summary>
	/// <param name="handle"></param>
	void remove(unsigned long handle);

	/// <summary>
	/// Removes all resources. 
	/// <para>
    /// The word 'Destroy' is not used here, since if any other pointers are
    /// referring to this resource, it will persist until they have finished
    /// with it; however to all intents and purposes it no longer exists and
    /// will likely get destroyed imminently.
	/// </para>
	/// <para>
    /// If you do have shared pointers to resources hanging around after the
    /// ResourceManager is destroyed, you may get problems on destruction of
    /// these resources if they were relying on the manager (especially if it is
    /// a plugin). If you find you get problems on shutdown in the destruction
    /// of resources, try making sure you release all your shared pointers
    /// before you shutdown OGRE. 
	/// </para>
	/// </summary>
	void removeAll();

	/// <summary>
	/// Returns whether the named resource exists in this manager. 
	/// </summary>
	/// <param name="name">The name to check for.</param>
	/// <returns>True if the resource exists.</returns>
	bool resourceExists(System::String^ name);

	/// <summary>
	/// Returns whether a resource with the given handle exists in this manager. 
	/// </summary>
	/// <param name="handle">The handle of the resource.</param>
	/// <returns>True if a resource with this handle exists.</returns>
	bool resourceExists(unsigned long handle);

	/// <summary>
	/// Sets whether this manager and its resources habitually produce log output. c
	/// </summary>
	/// <param name="v">True to enable.</param>
	void setVerbose(bool v);

	/// <summary>
	/// Gets whether this manager and its resources habitually produce log output. 
	/// </summary>
	/// <returns>True if enabled.</returns>
	bool getVerbose();
};

}

}