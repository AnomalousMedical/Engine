#pragma once

#pragma warning(push)
#pragma warning(disable : 4635)
#include "PhysXLoader.h"
#pragma warning(pop)

#include "Enums.h"

namespace PhysXWrapper
{

ref class PhysConvexMeshDesc;
ref class PhysMemoryWriteBuffer;
ref class PhysTriangleMeshDesc;
ref class PhysSoftBodyMeshDesc;

public value class PhysCookingParams
{
	public:
	/// <summary> 
	/// Target platform
	/// Should be set to the platform which you intend to load the cooked mesh data on. This allows
	/// the SDK to optimize the mesh data in an appropriate way for the platform and make sure that
	/// endianness issues are accounted for correctly.
	/// 
	/// Default value: Same as the platform on which the SDK is running.
	/// </summary>
	PhysPlatform	targetPlatform;

	/// <summary> 
	/// Skin width for convexes
	/// Specifies the amount to inflate the convex mesh by when the new convex hull generator is used.
	/// 
	/// Inflating the mesh allows the user to hide interpenetration errors by increasing the size of the
	/// collision mesh with respect to the size of the rendered geometry.

	/// Default value: 0.025f
	/// </summary>
	float		skinWidth;

	/// <summary> 
	/// Hint to choose speed or less memory for collision structures
	/// Default value: false
	/// </summary>
	bool		hintCollisionSpeed;
};

/// <summary>
/// Wrapper for the NxCooking interface.
/// </summary>
public ref class PhysCooking
{
private:
	static NxCookingInterface* cook = NxGetCookingLib(NX_PHYSICS_SDK_VERSION);

public:
	/// <summary>
	/// Initializes cooking This must be called at least once, before any
	/// cooking method is called (otherwise cooking fails) and should be matched
	/// with a call to NxCloseCooking() before you remove the allocator or
	/// output stream objects.
	/// <para>
	/// The previous state of the cooking initialization is stored in a stack
    /// each time you call NxInitCooking() and when you call NxCloseCooking()
    /// the previous state is activated again. The "state" that is saved is the
    /// allocator and output stream settings. The stack size is currently 32
    /// states, which means that you can not call NxInitCooking() 33 consecutive
    /// times without at least one call to NxCloseCooking() in between.
	/// </para>
	/// <para>
	/// Note: Cooking parameters (as set by NxSetCookingParams) are reset by
    /// this function. You should call NxSetCookingParams after this function,
    /// not before.
	/// </para>
	/// </summary>
	/// <returns>True if successful.</returns>
	static bool initCooking();

	/// <summary>
	/// Closes cooking.
	/// This must be called at the end of your app, to release cooking-related data.
	/// </summary>
	static void closeCooking();

	/// <summary>
	/// Cooks a convex mesh. The results are written to the stream. 
	/// To create a triangle mesh object(unlike previous versions) it is necessary to 
	/// first 'cook' the mesh data into a form which allows the SDK to perform efficient 
	/// collision detection.
	/// NxCookTriangleMesh() and NxCookConvexMesh() allow a mesh description to be cooked 
	/// into a binary stream suitable for loading and performing collision detection at 
	/// runtime.
	/// </summary>
	/// <param name="desc">The convex hull description.</param>
	/// <param name="stream">The output stream.</param>
	/// <returns>True if cooking is successful.</returns>
	static bool cookConvexMesh(PhysConvexMeshDesc^ desc, PhysMemoryWriteBuffer^ stream);

	/// <summary>
	/// Cooks a triangle mesh. The results are written to the stream. 
	/// 
	/// To create a triangle mesh object(unlike previous versions) it is necessary to first 
	/// 'cook' the mesh data into a form which allows the SDK to perform efficient collision 
	/// detection.
	/// 
	/// NxCookTriangleMesh() and NxCookConvexMesh() allow a mesh description to be cooked 
	/// into a binary stream suitable for loading and performing collision detection at 
	/// runtime.
	/// 
	/// NxCookConvex requires the input mesh to form a closed convex volume. This allows 
	/// more efficient and robust collision detection.
	/// </summary>
	/// <param name="desc">The mesh description.</param>
	/// <param name="stream">The output stream.</param>
	/// <returns>True if cooking was successful.</returns>
	static bool cookTriangleMesh(PhysTriangleMeshDesc^ desc, PhysMemoryWriteBuffer^ stream);

	/// <summary>
	/// Cooks a tetrahedral mesh to a SoftBodyMesh. 
	/// </summary>
	/// <param name="desc">The soft body mesh descriptor on which the generation of the cooked mesh depends.</param>
	/// <param name="stream">The stream the cooked mesh is written to.</param>
	/// <returns>True if cooking was successful.</returns>
	static bool cookSoftBodyMesh(PhysSoftBodyMeshDesc^ desc, PhysMemoryWriteBuffer^ stream);

	/// <summary>
	/// Sets cooking parameters.
	/// </summary>
	/// <param name="params"></param>
	static void setCookingParams(PhysCookingParams params);

	/// <summary>
	/// Gets cooking parameters.
	/// </summary>
	/// <returns></returns>
	static PhysCookingParams getCookingParams();
};

}