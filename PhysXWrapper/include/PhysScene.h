#pragma once
#include "AutoPtr.h"
#include "Enums.h"
#include "NativeRaycastReport.h"
#include "NativeContactReport.h"
#include "Nxp.h"
#include "NxScene.h"
#include "NxSceneExportWrapper.h"

class NxVec3;
class NxScene;

namespace Engine
{

namespace Physics
{

ref class PhysActor;
ref class PhysActorDesc;
interface class RaycastReport;
ref class PhysJoint;
ref class PhysJointDesc;
value class PhysActorGroupPair;
ref class PhysMaterial;
ref class PhysMaterialDesc;
ref class PhysSoftBody;
ref class PhysSoftBodyDesc;

typedef System::Collections::Generic::Dictionary<Engine::Identifier^, PhysActor^> ActorDictionary;
typedef System::Collections::Generic::Dictionary<Engine::Identifier^, PhysJoint^> JointDictionary;
typedef System::Collections::Generic::Dictionary<Engine::Identifier^, PhysSoftBody^> SoftBodyDictionary;

public delegate void ActorAdded(PhysActor^ actor);
public delegate void ActorRemoved(PhysActor^ actor);
public delegate void JointAdded(PhysJoint^ joint);
public delegate void JointRemoved(PhysJoint^ joint);
public delegate void SoftBodyAdded(PhysSoftBody^ softBody);
public delegate void SoftBodyRemoved(PhysSoftBody^ softBody);

/// <summary>
/// Wrapper class for NxScene.
/// A scene is a collection of bodies, constraints, and effectors which can interact. 
/// The scene simulates the behavior of these objects over time. Several scenes may exist
/// at the same time, but each body, constraint, or effector object is specific to a 
/// scene -- they may not be shared.
/// For example, attempting to create a joint in one scene and then using it to attach 
/// bodies from a different scene results in undefined behavior.
/// </summary>
public ref class PhysScene
{
private:
	System::String^ name;
	ActorDictionary actors;
	JointDictionary joints;
	SoftBodyDictionary softBodies;

	AutoPtr<NxVec3> pooledVector;  //Vector to be passed to physx functions.
	AutoPtr<NativeRaycastReport> nativeRaycastReport;
	AutoPtr<NativeContactReport> nativeContactReport;
	AutoPtr<NxSceneExportWrapper> sceneExportWrapper;

	unsigned int actorGroupPairPosition;
	AutoPtr<NxActorGroupPair> actorGroupPairBuffer;

	ActorAdded^ onActorAdded;
	ActorRemoved^ onActorRemoved;
	JointAdded^ onJointAdded;
	JointRemoved^ onJointRemoved;
	SoftBodyAdded^ onSoftBodyAdded;
	SoftBodyRemoved^ onSoftBodyRemoved;

internal:
	NxScene* scene;

	/// <summary>
	/// Constructor internal because scenes must be created by the sdk.
	/// </summary>
	PhysScene(NxScene* scene, System::String^ name);

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	~PhysScene();

	/// <summary>
	/// Sets the gravity in units per timeunits squared.
	/// </summary>
	/// <param name="grav">The gravity to set.</param>
	void setGravity(EngineMath::Vector3 grav);

	/// <summary>
	/// Gets the current gravity.
	/// </summary>
	/// <param name="grav">The vector3 to hold the result.</param>
	void getGravity(EngineMath::Vector3% grav);

	/// <summary>
	/// Creates an actor in this scene.
	/// </summary>
	/// <param name="desc">The description of the actor.</param>
	PhysActor^ createActor(PhysActorDesc^ desc);

	/// <summary>
	/// Destroys an actor in this scene.  After this call the actor will be disposed
	/// and can no longer be used.
	/// </summary>
	/// <param name="actor">The actor to destroy.</param>
	void releaseActor(PhysActor^ actor);

	/// <summary>
	/// Gets the actor named name.
	/// </summary>
	/// <param name="name">The name of the actor to get.</param>
	/// <returns>The actor specified by name or null if it does not exist.</returns>
	PhysActor^ getActor(Engine::Identifier^ name);

	/// <summary>
	/// Advances the simulation by the given time and will dispatch position updates
	/// for all actors that move.
	/// </summary>
	/// <param name="time">The amount of time to advance the simulation.</param>
	void stepSimulation(double time);

	/// <summary>
	/// With this method one can set contact reporting flags between actors belonging 
	/// to a pair of groups.  The groups can be the same for communication between actors
	/// in the same group.
	/// </summary>
	/// <param name="group1">The first group.</param>
	/// <param name="group2">The second group.</param>
	/// <param name="flags">The flag(s) for contact between these groups.</param>
	void setActorGroupPairFlags( int group1, int group2, ContactPairFlag flags );

	/// <summary>
	/// Reset the actor group pair iterator to the first actor group pair defined.
	/// </summary>
	void startActorGroupPairIter();

	/// <summary>
	/// Check to see if there is another actor group pair to recover.
	/// </summary>
	/// <returns>True if there are more pairs.  False if iteration is complete.</returns>
	bool hasNextActorGroupPair();

	/// <summary>
	/// Get the next defined actor group pair.  Must first call startActorGroupPairIter and 
	/// hasNextActorGroupPair.
	/// </summary>
	/// <returns>The next actor group pair.</returns>
	PhysActorGroupPair getNextActorGroupPair();

	/// <summary>
	/// Calls the report's onHit() method for all the shapes of type ShapeType 
	/// intersected by the ray. 
	/// hintFlags is a combination of NxRaycastBit flags. T
	/// The point of impact is provided as a parameter to hitCallback().
	/// </summary>
	/// <param name="ray">The ray.</param>
	/// <param name="report">The report callback to use.</param>
	/// <param name="shapesType">The types of shapes to include in the raycast.</param>
	/// <returns>Returns the number of shapes hit.</returns>
	int raycastAllShapes( EngineMath::Ray3 ray, 
						  RaycastReport^ report, 
					      ShapesType shapesType );

	/// <summary>
	/// Calls the report's onHit() method for all the shapes of type ShapeType 
	/// intersected by the ray. 
	/// hintFlags is a combination of NxRaycastBit flags. T
	/// The point of impact is provided as a parameter to hitCallback().
	/// </summary>
	/// <param name="ray">The ray.</param>
	/// <param name="report">The report callback to use.</param>
	/// <param name="shapesType">The types of shapes to include in the raycast.</param>
	/// <param name="groups">Mask used to filter shape objects.  Works on shape groups.</param>
	int raycastAllShapes( EngineMath::Ray3 ray, 
						  RaycastReport^ report, 
					      ShapesType shapesType, 
						  unsigned int groups );

	/// <summary>
	/// Calls the report's onHit() method for all the shapes of type ShapeType 
	/// intersected by the ray. 
	/// hintFlags is a combination of NxRaycastBit flags. T
	/// The point of impact is provided as a parameter to hitCallback().
	/// </summary>
	/// <param name="ray">The ray.</param>
	/// <param name="report">The report callback to use.</param>
	/// <param name="shapesType">The types of shapes to include in the raycast.</param>
	/// <param name="groups">Mask used to filter shape objects.  Works on shape groups.</param>
	/// <param name="maxDistance">Max distance to check along the ray for intersecting objects. Range: (0,inf)</param>
	int raycastAllShapes( EngineMath::Ray3 ray, 
						  RaycastReport^ report, 
					      ShapesType shapesType, 
						  unsigned int groups, 
						  float maxDistance );

	/// <summary>
	/// Calls the report's onHit() method for all the shapes of type ShapeType 
	/// intersected by the ray. 
	/// hintFlags is a combination of NxRaycastBit flags. T
	/// The point of impact is provided as a parameter to hitCallback().
	/// </summary>
	/// <param name="ray">The ray.</param>
	/// <param name="report">The report callback to use.</param>
	/// <param name="shapesType">The types of shapes to include in the raycast.</param>
	/// <param name="groups">Mask used to filter shape objects.  Works on shape groups.</param>
	/// <param name="maxDistance">Max distance to check along the ray for intersecting objects. Range: (0,inf)</param>
	/// <param name="hintFlags">Allows the user to specify which field of NxRaycastHit they are interested in.</param>
	int raycastAllShapes( EngineMath::Ray3 ray, 
						  RaycastReport^ report, 
					      ShapesType shapesType, 
						  unsigned int groups, 
						  float maxDistance, 
						  RaycastBit hintFlags );

	/// <summary>
	/// Creates a new joint based on the descriptor.  The name property of the descriptor
	/// must be unique for all joints in the scene, but the descriptor itself can be used
	/// to create multiple joints if the name is changed.
	/// </summary>
	/// <param name="jointDesc">The joint descriptor.</param>
	/// <returns>A new PhysJoint or null if an error occured.</returns>
	PhysJoint^ createJoint(PhysJointDesc^ jointDesc);

	/// <summary>
	/// Releases the native parts of a joint.  After this function is called the joint is
	/// no longer valid.
	/// </summary>
	/// <param name="joint">The joint to destroy.  It must have been made by this scene.</param>
	void releaseJoint(PhysJoint^ joint);

	/// <summary>
	/// Gets the joint specified by name.
	/// </summary>
	/// <param name="name">The name of the joint to get.</param>
	/// <returns>The joint specified by name or null if it does not exist.</returns>
	PhysJoint^ getJoint(Engine::Identifier^ name);

	/// <summary>
	/// Get the scene flags.
	/// </summary>
	/// <returns>The scene flags.</returns>
	SceneFlags getFlags();

	/// <summary>
	/// The name of the scene.
	/// </summary>
	property System::String^ Name
	{ 
		System::String^ get();
	}

	/// <summary>
	/// Get the scene in a wrapped form that can be exported to another dll.
	/// </summary>
	/// <returns></returns>
	NxSceneExportWrapper* getNxScene()
	{
		return sceneExportWrapper.Get();
	}

	/// <summary>
	/// Get the number of materials defined for this scene.
	/// </summary>
	/// <returns>The number of materials defined in this scene.</returns>
	unsigned int getNbMaterials();

	/// <summary>
	/// Returns current highest valid material index.
	/// 
	/// Note that not all indices below this are valid if some of them belong to
    /// meshes that have been freed.
	/// </summary>
	/// <returns>The highest material index.</returns>
	short getHighestMaterialIndex();

	/// <summary>
	/// Retrieves the material with the given material index. 
	/// 
	/// There is always at least one material in the Scene, the default material
    /// (index 0). If the specified material index is out of range (larger than
    /// getHighestMaterialIndex) or belongs to a material that has been
    /// released, then the default material is returned, but no error is
    /// reported.
	/// 
	/// You can always get a pointer to the default material by specifying index
    /// 0. You can change the properties of the default material by changing the
    /// properties directly on the material. It is not possible to release the
    /// default material, calling releaseMaterial(defaultMaterial) has no
    /// effect.
	/// </summary>
	/// <param name="index">The index of the material.</param>
	/// <returns>The associated material.</returns>
	PhysMaterial^ getMaterialFromIndex(short index);

	/// <summary>
	/// Creates a new NxMaterial. 
	/// 
	/// The material library consists of an array of material objects. Each
    /// material has a well defined index that can be used to refer to it. If an
    /// object (shape or triangle) references an undefined material, the default
    /// material with index 0 is used instead.
	/// </summary>
	/// <param name="desc">The material desc to use to create a material. See NxMaterialDesc.</param>
	/// <returns>The new material.</returns>
	PhysMaterial^ createMaterial(PhysMaterialDesc^ desc);

	/// <summary>
	/// Deletes the specified material. 
	/// 
	/// The material must be in this scene. Do not keep a reference to the
    /// deleted instance. If you release a material while shapes or meshes are
    /// referencing its material index, the material assignment of these objects
    /// becomes undefined. Avoid release calls while the scene is simulating (in
    /// between simulate() and fetchResults() calls).
	/// </summary>
	/// <param name="material">The material to release.</param>
	void releaseMaterial(PhysMaterial^ material);

	PhysSoftBody^ createSoftBody(PhysSoftBodyDesc^ softBodyDesc);

	void releaseSoftBody(PhysSoftBody^ softBody);

	PhysSoftBody^ getSoftBody(Engine::Identifier^ name);

	/// <summary>
	/// Called when an Actor is added to the scene.
	/// </summary>
	event ActorAdded^ OnActorAdded
	{
        void add(ActorAdded^ value)
		{
			onActorAdded = (ActorAdded^)System::Delegate::Combine(onActorAdded, value);
        }
        void remove(ActorAdded^ value)
		{
			onActorAdded = (ActorAdded^)System::Delegate::Remove(onActorAdded, value);
        }
    }

	/// <summary>
	/// Called when an Actor is removed from the scene.
	/// </summary>
	event ActorRemoved^ OnActorRemoved
	{
        void add(ActorRemoved^ value)
		{
			onActorRemoved = (ActorRemoved^)System::Delegate::Combine(onActorRemoved, value);
        }
        void remove(ActorRemoved^ value)
		{
			onActorRemoved = (ActorRemoved^)System::Delegate::Remove(onActorRemoved, value);
        }
    }

	/// <summary>
	/// Called when a joint is added to the scene.
	/// </summary>
	event JointAdded^ OnJointAdded
	{
        void add(JointAdded^ value)
		{
			onJointAdded = (JointAdded^)System::Delegate::Combine(onJointAdded, value);
        }
        void remove(JointAdded^ value)
		{
			onJointAdded = (JointAdded^)System::Delegate::Remove(onJointAdded, value);
        }
    }

	/// <summary>
	/// Called when a joint is removed from the scene.
	/// </summary>
	event JointRemoved^ OnJointRemoved
	{
        void add(JointRemoved^ value)
		{
			onJointRemoved = (JointRemoved^)System::Delegate::Combine(onJointRemoved, value);
        }
        void remove(JointRemoved^ value)
		{
			onJointRemoved = (JointRemoved^)System::Delegate::Remove(onJointRemoved, value);
        }
    }

	/// <summary>
	/// Called when a SoftBody is added to the scene.
	/// </summary>
	event SoftBodyAdded^ OnSoftBodyAdded
	{
        void add(SoftBodyAdded^ value)
		{
			onSoftBodyAdded = (SoftBodyAdded^)System::Delegate::Combine(onSoftBodyAdded, value);
        }
        void remove(SoftBodyAdded^ value)
		{
			onSoftBodyAdded = (SoftBodyAdded^)System::Delegate::Remove(onSoftBodyAdded, value);
        }
    }

	/// <summary>
	/// Called when a SoftBody is removed from the scene.
	/// </summary>
	event SoftBodyRemoved^ OnSoftBodyRemoved
	{
        void add(SoftBodyRemoved^ value)
		{
			onSoftBodyRemoved = (SoftBodyRemoved^)System::Delegate::Combine(onSoftBodyRemoved, value);
        }
        void remove(SoftBodyRemoved^ value)
		{
			onSoftBodyRemoved = (SoftBodyRemoved^)System::Delegate::Remove(onSoftBodyRemoved, value);
        }
    }
};

}

}