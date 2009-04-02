#pragma once

class NxPhysicsSDK;
class NxRemoteDebugger;

#include "AutoPtr.h"
#include "PhysXLogger.h"
#include "Enums.h"

namespace Physics
{

ref class PhysScene;
ref class PhysSceneDesc;
ref class PhysMemoryReadBuffer;
ref class PhysConvexMesh;
ref class PhysTriangleMesh;
ref class PhysSoftBodyMesh;

using namespace System;

typedef System::Collections::Generic::Dictionary<String^, PhysScene^> SceneDictionary;

/// <summary>
/// Wrapper for NxPhysicsSDK.
/// </summary>
public ref class PhysSDK
{
private:
	NxPhysicsSDK* physicsSDK;
	NxRemoteDebugger* remoteDebugger;
	SceneDictionary^ scenes;
	
	static PhysSDK^ instance = nullptr;

	PhysSDK(void);

internal:
	AutoPtr<PhysXLogger> logger;

public:

	/// <summary>
	/// Instance property.
	/// </summary>
	static property PhysSDK^ Instance 
	{ 
		PhysSDK^ get()
		{
			if(instance == nullptr)
			{
				instance = gcnew PhysSDK();
			}
			return instance;
		}
	}

	/// <summary>
	/// Destructor.
	/// </summary>
	~PhysSDK();

	/// <summary>
	/// Connect to the remote debugger running on host.
	/// </summary>
	/// <param name="host">The name of the host to connect to.</param>
	void connectRemoteDebugger( String^ host );

	/// <summary>
	/// Disconnect from the remote debugger.
	/// </summary>
	void disconnectRemoteDebugger();

	/// <summary>
	/// Create a new PhysScene based on desc.
	/// </summary>
	/// <param name="desc">The scene description to use.</param>
	PhysScene^ createScene(PhysSceneDesc^ desc);

	/// <summary>
	/// Destroy the PhysScene.  After this it cannot be used anymore.
	/// </summary>
	/// <param name="scene">The scene to destroy.</param>
	void releaseScene(PhysScene^ scene);

	/// <summary>
	/// Get the number of scenes that have been created.
	/// </summary>
	/// <returns>The number of scenes created.</returns>
	int getNbScenes();

	/// <summary>
	/// Returns the scene identified by name.
	/// </summary>
	/// <param name="name">The name of the scene to get.</param>
	/// <returns>The scene identified by name.</returns>
	PhysScene^ getScene(String^ name);

	/// <summary>
	/// Creates a convex mesh object. This can then be instanced into
    /// NxConvexShape objects.
	/// </summary>
	/// <param name="mesh">The stream to load the convex mesh from.</param>
	/// <returns>The new convex mesh.</returns>
	PhysConvexMesh^ createConvexMesh(PhysMemoryReadBuffer^ mesh);

	/// <summary>
	/// Destroys the instance passed. Be sure to not keep a reference to this
	/// object after calling release. Do not release the convex mesh before all
    /// its instances are released first! Avoid release calls while the scene is
    /// simulating (in between simulate() and fetchResults() calls).
	/// </summary>
	/// <param name="mesh">The convex mesh to release.</param>
	void releaseConvexMesh(PhysConvexMesh^ mesh);

	/// <summary>
	/// Creates a triangle mesh object. This can then be instanced into
    /// NxTriangleMeshShape objects.
	/// </summary>
	/// <param name="mesh">The triangle mesh stream.</param>
	/// <returns>The new triangle mesh.</returns>
	PhysTriangleMesh^ createTriangleMesh(PhysMemoryReadBuffer^ mesh);

	/// <summary>
	/// Destroys the instance passed. Be sure to not keep a reference to this
	/// object after calling release. Do not release the convex mesh before all
    /// its instances are released first! Avoid release calls while the scene is
    /// simulating (in between simulate() and fetchResults() calls).
	/// </summary>
	/// <param name="mesh">The convex mesh to release.</param>
	void releaseTriangleMesh(PhysTriangleMesh^ mesh);

	/// <summary>
	/// Creates a soft body mesh from a cooked soft body mesh stored in a
    /// stream. Stream has to be created with NxCookSoftBodyMesh().
	/// </summary>
	/// <param name="name">The name of the soft body mesh.</param>
	/// <param name="mesh">The stream with the soft body mesh.</param>
	/// <returns>The new soft body mesh.</returns>
	PhysSoftBodyMesh^ createSoftBodyMesh(System::String^ name, PhysMemoryReadBuffer^ mesh);

	/// <summary>
	/// Deletes the specified soft body mesh. The soft body mesh must be in this
	/// scene. Do not keep a reference to the deleted instance. Avoid release
    /// calls while the scene is simulating (in between simulate() and
    /// fetchResults() calls).
	/// </summary>
	/// <param name="mesh">The mesh to release.</param>
	void releaseSoftBodyMesh(PhysSoftBodyMesh^ mesh);

	/// <summary>
	/// Reports the available revision of the PhysX Hardware.
	/// </summary>
	/// <returns>0 if there is no hardware present in the machine, 1 for the PhysX Athena revision 1.0 card.</returns>
	HWVersion getHWVersion();

	/// <summary>
	/// Function that lets you set global simulation parameters. Returns false
    /// if the value passed is out of range for usage specified by the enum.
    /// Sleeping: Does NOT wake any actors which may be affected.
	/// </summary>
	/// <param name="paramEnum">Parameter to set.</param>
	/// <param name="paramValue">The value to set.</param>
	/// <returns></returns>
	bool setParameter(PhysParameter paramEnum, float paramValue);

	/// <summary>
	/// Function that lets you query global simulation parameters.
	/// </summary>
	/// <param name="paramEnum">The parameter to check.</param>
	/// <returns>The parameter value.</returns>
	float getParameter(PhysParameter paramEnum);
};

}