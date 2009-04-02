#pragma once

#include "Enums.h"
#include "AutoPtr.h"
#include <string>

class NxJoint;
class NxJointDesc;

namespace PhysXWrapper
{

ref class PhysActor;
ref class PhysScene;

/// <summary>
/// Abstract base class for the different types of joints. 
/// All joints are used to connect two dynamic actors, or an actor and the environment.
/// 
/// A NULL actor represents the environment. Whenever the below comments mention two 
/// actors, one of them may always be the environment (NULL).
/// </summary>
[Engine::Attributes::NativeSubsystemType]
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysJoint abstract
{
private:

protected:
	PhysActor^ actor0;
	PhysActor^ actor1;
	PhysScene^ scene;

internal:
	NxJoint* joint;
	PhysJoint(NxJoint* joint, PhysActor^ actor0, PhysActor^ actor1, PhysScene^ scene);
	virtual NxJointDesc& getDesc() = 0;

	virtual void reloadFromDesc(NxJointDesc& desc) = 0;

	/// <summary>
	/// Destructor
	/// </summary>
	virtual ~PhysJoint();
public:
	/// <summary>
	/// Gets the first actor on the joint.
	/// </summary>
	/// <returns>The first actor.</returns>
	PhysActor^ getActor0();

	/// <summary>
	/// Gets the second actor on the joint.
	/// </summary>
	/// <returns>The second actor.</returns>
	PhysActor^ getActor1();

	/// <summary>
	/// Sets the point where the two actors are attached, specified in global coordinates. 
	///
	/// Set this after setting the actors of the joint.
	/// </summary>
	/// <param name="vec">Global connection point.</param>
	void setGlobalAnchor(EngineMath::Vector3 vec);

	/// <summary>
	/// Sets the direction of the joint's primary axis, specified in global coordinates. 
	///
	/// The direction vector should be normalized to unit length.
	/// </summary>
	/// <param name="vec">The axis direction.</param>
	void setGlobalAxis(EngineMath::Vector3 vec);

	/// <summary>
	/// Retrieves the joint anchor.
	/// </summary>
	/// <returns>The joints anchor point in the global frame.</returns>
	EngineMath::Vector3 getGlobalAnchor();

	/// <summary>
	/// Retrieves the joint axis.
	/// </summary>
	/// <returns>The joints axis in the global frame.</returns>
	EngineMath::Vector3 getGlobalAxis();

	/// <summary>
	/// Returns the state of the joint.
	/// </summary>
	/// <remarks>
	/// Joints are created in the NX_JS_UNBOUND state. Making certain changes to 
	/// the simulation or the joint can also make joints become unbound. Unbound 
	/// joints are automatically bound the next time Scene::run() is called, and 
	/// this changes their state to NX_JS_SIMULATING. NX_JS_BROKEN means that a 
	/// breakable joint has broken due to a large force or one of its actors has 
	/// been deleted. In either case the joint was removed from the simulation, 
	/// so it should be released by the user to free up its memory.
	/// </remarks>
	/// <returns>The state of the joint. See JointState</returns>
	JointState getState();

	/// <summary>
	/// Sets the maximum force magnitude that the joint is able to withstand without breaking.
	/// </summary>
	/// <remarks>
	/// If the joint force rises above this threshold, the joint breaks, and becomes disabled. Additionally, the jointBreakNotify() method of the scene's user notify callback will be called. (You can set this with NxScene::setUserNotify()).
	/// 
	/// There are two values, one for linear forces, and one for angular forces. 
	/// Both values are used directly as a value for the maximum impulse tolerated 
	/// by the joint constraints.
	/// 
	/// Both force values are NX_MAX_REAL by default. This setting makes the joint 
	/// unbreakable. The values should always be nonnegative.
	/// 
	/// The distinction between maxForce and maxTorque is dependent on how the joint is 
	/// implemented internally, which may not be obvious. For example what appears to be
	/// an angular degree of freedom may be constrained indirectly by a linear constraint.
	/// 
	/// So in most practical applications the user should set both maxTorque and maxForce 
	/// to low values.
	/// 
	/// Sleeping: This call wakes the actor(s) if they are sleeping
	/// </remarks>
	/// <param name="maxForce">Maximum force the joint can withstand without breaking. Range: (0,inf]</param>
	/// <param name="maxTorque">Maximum torque the joint can withstand without breaking. Range: (0,inf]</param>
	void setBreakable(float maxForce, float maxTorque);

	/// <summary>
	/// Retrieves the max forces of a breakable joint.
	/// </summary>
	/// <param name="maxForce">Maximum force the joint can withstand without breaking. Range: (0,inf]</param>
	/// <param name="maxTorque">Maximum torque the joint can withstand without breaking. Range: (0,inf]</param>
	void getBreakable(float% maxForce, float% maxTorque);

	/// <summary>
	/// Sets the solver extrapolation factor.
	/// </summary>
	/// <param name="factor">Value to set the extrapolation factor to.</param>
	void setSolverExtrapolationFactor(float factor);

	/// <summary>
	/// Retrieves the solver extrapolation factor.
	/// </summary>
	/// <returns>The solver extrapolation factor.</returns>
	float getSolverExtrapolationFactor();

	/// <summary>
	/// Switch between acceleration and force based spring.
	/// </summary>
	/// <param name="b">true: use acceleration spring, false: use force spring</param>
	void setUseAccelerationSpring(bool b);

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	bool getUseAccelerationSpring();

	/// <summary>
	/// Checks whether acceleration spring is used.
	/// </summary>
	/// <returns>True if acceleration spring is used else false.</returns>
	JointType getType();

	/// <summary>
	/// Retrieves owner scene.
	/// </summary>
	/// <returns>The scene which owns this joint.</returns>
	PhysScene^ getScene();

	//---------------
	//   Limits
	//---------------

	/// <summary>
	/// Sets the limit point. 
	///
	/// The point is specified in the global coordinate frame
	/// </summary>
	/// <remarks>
	/// All types of joints may be limited with the same system: You may elect a point 
	/// attached to one of the two actors to act as the limit point. You may also specify 
	/// several planes attached to the other actor.
	/// 
	/// The points and planes move together with the actor they are attached to.
	/// 
	/// The simulation then makes certain that the pair of actors only move relative to 
	/// each other so that the limit point stays on the positive side of all limit planes.
	/// 
	/// the default limit point is (0,0,0) in the local frame of actor2. Calling this 
	/// deletes all existing limit planes.
	/// 
	/// Sleeping: This call wakes the actor(s) if they are sleeping.
	/// </remarks>
	/// <param name="point">The limit reference point defined in the global frame. Range: position vector.</param>
	/// <param name="pointIsOnActor1">if true the point is attached to the second actor. Otherwise it is attached to the first.</param>
	void setLimitPoint(EngineMath::Vector3% point, bool pointIsOnActor1);

	/// <summary>
	/// Retrieves the global space limit point.
	/// </summary>
	/// <param name="worldLimitPoint">Used to store the global frame limit point</param>
	/// <returns>True if the point is fixed to actor 2 otherwise the point is fixed to actor 1.</returns>
	bool getLimitPoint(EngineMath::Vector3% worldLimitPoint);

	/// <summary>
	/// Adds a limit plane. 
	///
	/// Both of the parameters are given in global coordinates. see setLimitPoint() for 
	/// the meaning of limit planes.
	/// </summary>
	/// <remarks>
	/// The plane is affixed to the actor that does not have the limit point.
	/// 
	/// The normal of the plane points toward the positive side of the plane, and thus 
	/// toward the limit point. If the normal points away from the limit point at the time
	/// of this call, the method returns false and the limit plane is ignored.
	/// 
	/// Note: This function always returns true and adds the limit plane unlike earlier 
	/// versions. This behavior was changed to allow the joint to be serialized easily.
	/// Sleeping: This call wakes the actor(s) if they are sleeping.
	/// </remarks>
	/// <param name="normal">Normal for the limit plane in global coordinates. Range: direction vector</param>
	/// <param name="pointInPlane">Point in the limit plane in global coordinates. Range: position vector</param>
	/// <param name="restitution">Restitution of the limit plane. Range: [0.0, 1.0] Default: 0.0</param>
	/// <returns></returns>
	bool addLimitPlane(EngineMath::Vector3% normal, EngineMath::Vector3% pointInPlane, float restitution);

	/// <summary>
	/// deletes all limit planes added to the joint. 
	///
	/// Invalidates limit plane iterator.
	///
	/// Sleeping: Does NOT wake the associated actor up automatically.
	/// </summary>
	void purgeLimitPlanes();

	/// <summary>
	/// Restarts the limit plane iteration. 
	/// Call before starting to iterate. This method may be used together with the below 
	/// two methods to enumerate the limit planes. This iterator becomes invalid when planes 
	/// are added or removed, or the plane iterator mechanism is invoked on another joint.
	/// </summary>
	void resetLimitPlaneIterator();

	/// <summary>
	/// Returns true until the iterator reaches the end of the set of limit planes. 
	/// 
	/// Adding or removing elements does not reset the iterator
	/// </summary>
	/// <returns>True if the iterator has not reached the end of the sequence of limit planes.</returns>
	bool hasMoreLimitPlanes();

	/// <summary>
	/// 
	/// </summary>
	/// <param name="planeNormal">Used to store the plane normal.</param>
	/// <param name="planeD">Used to store the plane 'D'.</param>
	/// <param name="restitution">Optional, used to store restitution of the limit plane.</param>
	/// <returns></returns>
	bool getNextLimitPlane(EngineMath::Vector3% planeNormal, float% planeD, float% restitution);

	/// <summary>
	/// Set the local anchor of object 0.
	/// </summary>
	/// <param name="anchor">The local anchor to set in the object's frame.</param>
	void setLocalAnchor0(EngineMath::Vector3 anchor);

	/// <summary>
	/// Set the local anchor of object 1.
	/// </summary>
	/// <param name="anchor">The local anchor to set in the object's frame.</param>
	void setLocalAnchor1(EngineMath::Vector3 anchor);
};

}