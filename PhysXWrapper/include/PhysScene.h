#pragma once
#include "AutoPtr.h"
#include "Enums.h"
#include "NativeRaycastReport.h"
#include "NativeContactReport.h"
#include "Nxp.h"
#include "NxScene.h"
#include "PhysActorCollection.h"
#include "PhysSoftBodyCollection.h"
#include "PhysJointCollection.h"

class NxVec3;
class NxScene;

namespace PhysXWrapper
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
	PhysActorCollection actors;
	PhysJointCollection joints;
	PhysSoftBodyCollection softBodies;

	AutoPtr<NxVec3> pooledVector;  //Vector to be passed to physx functions.
	AutoPtr<NativeRaycastReport> nativeRaycastReport;
	AutoPtr<NativeContactReport> nativeContactReport;

	unsigned int actorGroupPairPosition;
	AutoPtr<NxActorGroupPair> actorGroupPairBuffer;

internal:
	NxScene* scene;

	/// <summary>
	/// Constructor internal because scenes must be created by the sdk.
	/// </summary>
	PhysScene(NxScene* scene);

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
	/// Get the scene flags.
	/// </summary>
	/// <returns>The scene flags.</returns>
	SceneFlags getFlags();

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
};

}