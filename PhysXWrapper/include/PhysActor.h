#pragma once

#include "Enums.h"
#include <vcclr.h>
#include <AutoPtr.h>
#include <string>
#include "PhysShapeCollection.h"

class NxActor;
class NxMat34;

namespace Engine
{

namespace Physics
{

ref class PhysActor;
ref class ContactIterator;
interface class ContactListener;
ref class PhysActorDesc;
ref class PhysBodyDesc;
ref class PhysShapeDesc;

typedef System::Collections::Generic::LinkedList<ContactListener^> ContactListenerList;
typedef gcroot<PhysActor^> PhysActorGCRoot;

interface class ActiveTransformCallback;
ref class PhysShape;

typedef System::Collections::Generic::List<PhysShape^> ShapeEnumerator;
/// <summary>
/// Wrapper for NxActor.
/// PhysActor is the main simulation object in the physics SDK. 
/// The actor is owned by and contained in a PhysScene.
/// An actor may optionally encapsulate a dynamic rigid body by setting 
/// the body member of the actor's descriptor when it is created. Otherwise 
/// the actor will be static (fixed in the world).
/// </summary>
[Engine::Attributes::NativeSubsystemType]
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysActor
{
private:
	ActiveTransformCallback^ callback;
	ContactListenerList^ contactListeners;

	AutoPtr<PhysActorGCRoot> actorRoot;

	PhysShapeCollection shapes;

internal:
	//NxActor is avaliable internally for easy use in SDK functions.
	NxActor* actor;

	/// <summary>
	/// Constructor.  Internal since the SDK must create the actors.
	/// </summary>
	/// <param name="actor">The NxActor to wrap.</param>
	PhysActor(NxActor* actor);

	/// <summary>
	/// Called when this actor has been moved.  Will dispatch the move message to the 
	/// ActiveTransformCallback.
	/// </summary>
	/// <param name="newLoc">The new location.</param>
	void fireLocationChanged(NxMat34& newLoc);

	/// <summary>
	/// Called by the NativeContactReport when this actor contacts another actor.  Will
	/// dispatch messages to the ContactListeners owned by this actor.
	/// </summary>
	/// <param name="contactWith">The item being contacted with.</param>
	/// <param name="myself">The owner item contacting the other item.</param>
	/// <param name="contacts">The contact iterator with the contact point information.</param>
	/// <param name="contactType">The type of contact.</param>
	void alertContact( PhysActor^ contactWith, PhysActor^ myself, ContactIterator^ contacts, ContactPairFlag contactType );

public:
	/// <summary>
	/// Destructor
	/// </summary>
	~PhysActor();

	/// <summary>
	/// Saves the body information of a dynamic actor to the passed body descriptor. 
	/// 
	/// This method only save the dynamic(body) state for the actor. The user should use 
	/// saveToDesc() to save the state common between static and dynamic actors. Plus 
	/// manually saving the shapes belonging to the actor.
	/// 
	/// The actor must be dynamic.
	/// 
	/// </summary>
	/// <param name="actorDesc">The actor description to save to.</param>	
	void saveToDesc(PhysActorDesc^ actorDesc);

	/// <summary>
	/// Saves the state of the actor to the passed descriptor. 
	/// 
	/// This method does not save out any shapes belonging to the actor to the descriptor's 
	/// shape vector, nor does it write to its body member. You have to iterate through the 
	/// shapes of the actor and save them manually. In addition for dynamic actors you have 
	/// to call the saveBodyToDesc() method.
	/// </summary>
	/// <param name="bodyDesc">Descriptor to save the state of the body to.</param>
	/// <returns>True for a dynamic body. Otherwise False.</returns>
	bool saveBodyToDesc(PhysBodyDesc^ bodyDesc);

	/// <summary>
	/// Sets the transform callback.  Called interally by the colleague or other
	/// active transform client.  Will only be fired if active transforms are enabled
	/// on the PhysScene for this actor.
	/// </summary>
	/// <param name="callback">The callback class.</param>
	void setActiveTransformCallback(ActiveTransformCallback^ callback);

	/// <summary>
	/// Adds a listener that gets fired when this object contacts another object.
	/// </summary>
	/// <param name="listener">The listener to add.</param>
	void addContactListener( ContactListener^ listener );

	/// <summary>
	/// Remove the contact listener.
	/// </summary>
	/// <param name="listener">The listener to remove.</param>
	void removeContactListener( ContactListener^ listener );

	/// <summary>
	/// Set the global translation of the rigid body.
	/// </summary>
	/// <param name="translation">The translation to set.</param>
	void setGlobalPosition( EngineMath::Vector3 translation );

	/// <summary>
	/// Set the global rotation of the rigid body.
	/// </summary>
	/// <param name="rotation">The rotation to set.</param>
	void setGlobalOrientationQuat( EngineMath::Quaternion rotation );

	/// <summary>
	/// Return the current translation in world coordinates.
	/// </summary>
	/// <returns>The global position of the actor.</returns>
	EngineMath::Vector3 getGlobalPosition();

	/// <summary>
	/// Return the current rotation in world coordinates.
	/// </summary>
	/// <returns>The global orientation of the actor.</returns>
	EngineMath::Quaternion getGlobalOrientationQuat();

	/// <summary>
	/// Makes the actor part of actorGroup.  This way it can participate
	/// in collision callbacks.
	/// </summary>
	/// <param name="actorGroup">The index of the actor group.</param>
	void setActorGroup( int actorGroup );

	/// <summary>
	/// Returns the group this actor belongs to
	/// </summary>
	/// <returns>The actor group.</returns>
	int getActorGroup();

	/// <summary>
	/// Set the collision group the actor's shapes belong to.  This can be used to
	/// filter out interactions between different actors.
	/// </summary>
	/// <param name="collisionGroup">The collision group to set all shapes into.</param>
	void setCollisionGroup( int collisionGroup );

	/// <summary>
	/// Return the collision group
	/// </summary>
	/// <returns>The collision group of the first shape added to the actor.</returns>
	int getCollisionGroup();

	/// <summary>
	/// Raise a particular body flag
	/// </summary>
	/// <param name="flag">The flag to raise.</param>
	void raiseBodyFlag( BodyFlag flag );

	/// <summary>
	/// Clear a particular body flag
	/// </summary>
	/// <param name="flag">The flag to clear.</param>
	void clearBodyFlag( BodyFlag flag );

	/// <summary>
	/// Read the given body flag
	/// </summary>
	/// <param name="flag">The flag to read.</param>
	/// <returns>True if the flag is set.</returns>
	bool readBodyFlag( BodyFlag flag );

	/// <summary>
	/// Raises a particular actor flag.
	/// </summary>
	/// <param name="actorFlag">The flag to raise.</param>
	void raiseActorFlag (ActorFlag actorFlag);
 
	/// <summary>
	/// Clears a particular actor flag. 
	/// </summary>
	/// <param name="actorFlag">The flag to clear.</param>
	void clearActorFlag (ActorFlag actorFlag);
  
	/// <summary>
	/// Reads a particular actor flag. 
	/// </summary>
	/// <param name="actorFlag">The flag to read.</param>
	/// <returns>True if the flag is set.</returns>
	bool readActorFlag (ActorFlag actorFlag);

	/// <summary>
	/// Check to see if the actor is dynamic. 
	/// </summary>
	/// <returns>True if the actor is dynamic.</returns>
	bool isDynamic();


	//----------------------
	//Forces
	//----------------------

	/// <summary>
	/// Applies a force (or impulse) defined in the global coordinate frame, acting 
	/// at a particular point in global coordinates, to the actor. 
	/// </summary>
	/// <param name="force">Force/impulse to add, defined in the global frame. Range: force vector.</param>
	/// <param name="pos">Position in the global frame to add the force at. Range: position vector.</param>
	/// <param name="mode">The mode to use when applying the force/impulse.</param>
	/// <param name="wakeup">Specify if the call should wake up the actor.</param>
	void addForceAtPos( EngineMath::Vector3% force, EngineMath::Vector3% pos, ForceMode mode, bool wakeup );

	/// <summary>
	/// Applies a force (or impulse) defined in the global coordinate frame, 
	/// acting at a particular point in local coordinates, to the actor. 
	/// </summary>
	/// <param name="force">Force/impulse to add, defined in the global frame. Range: force vector.</param>
	/// <param name="pos">Position in the local frame to add the force at. Range: position vector.</param>
	/// <param name="mode">The mode to use when applying the force/impulse.</param>
	/// <param name="wakeup">Specify if the call should wake up the actor.</param>
	void addForceAtLocalPos (const EngineMath::Vector3% force, const EngineMath::Vector3% pos, ForceMode mode, bool wakeup);
  
	/// <summary>
	/// Applies a force (or impulse) defined in the actor local coordinate 
	/// frame, acting at a particular point in global coordinates, to the actor. 
	/// </summary>
	/// <param name="force">Force/impulse to add, defined in the local frame. Range: force vector.</param>
	/// <param name="pos">Position in the global frame to add the force at. Range: position vector.</param>
	/// <param name="mode">The mode to use when applying the force/impulse.</param>
	/// <param name="wakeup">Specify if the call should wake up the actor.</param>
	void addLocalForceAtPos (const EngineMath::Vector3% force, const EngineMath::Vector3% pos, ForceMode mode, bool wakeup);

	/// <summary>
	/// Applies a force (or impulse) defined in the actor local coordinate frame, 
	/// acting at a particular point in local coordinates, to the actor. 
	/// </summary>
	/// <param name="force">Force/impulse to add, defined in the local frame. Range: force vector.</param>
	/// <param name="pos">Position in the local frame to add the force at. Range: position vector.</param>
	/// <param name="mode">The mode to use when applying the force/impulse.</param>
	/// <param name="wakeup">Specify if the call should wake up the actor.</param>
	void addLocalForceAtLocalPos (const EngineMath::Vector3% force, const EngineMath::Vector3% pos, ForceMode mode, bool wakeup);

	/// <summary>
	/// Applies a force (or impulse) defined in the global coordinate frame to the actor. 
    /// </summary>
	/// <param name="force">Force/Impulse to apply defined in the global frame. Range: force vector.</param>
	/// <param name="mode">The mode to use when applying the force/impulse.</param>
	/// <param name="wakeup">Specify if the call should wake up the actor.</param>
	void addForce (const EngineMath::Vector3% force, ForceMode mode, bool wakeup);
  
	/// <summary>
	/// Applies a force (or impulse) defined in the actor local coordinate frame to the actor. 
	/// </summary>
	/// <param name="force">Force/Impulse to apply defined in the local frame. Range: force vector.</param>
	/// <param name="mode">The mode to use when applying the force/impulse.</param>
	/// <param name="wakeup">Specify if the call should wake up the actor.</param>
	void addLocalForce (const EngineMath::Vector3% force, ForceMode mode, bool wakeup);
  
	/// <summary>
	/// Applies an impulsive torque defined in the global coordinate frame to the actor. 
	/// </summary>
	/// <param name="torque">Torque to apply defined in the global frame. Range: torque vector.</param>
	/// <param name="mode">The mode to use when applying the force/impulse.</param>
	/// <param name="wakeup">Specify if the call should wake up the actor.</param>
	void addTorque (const EngineMath::Vector3% torque, ForceMode mode, bool wakeup);
  
	/// <summary>
	/// Applies an impulsive torque defined in the actor local coordinate frame to the actor. 
	/// </summary>
	/// <param name="torque">Torque to apply defined in the local frame. Range: torque vector.</param>
	/// <param name="mode">The mode to use when applying the force/impulse.</param>
	/// <param name="wakeup">Specify if the call should wake up the actor.</param>
	void addLocalTorque (const EngineMath::Vector3% torque, ForceMode mode, bool wakeup); 


	//-----------------
	//Kinematic
	//-----------------
	
	/// <summary>
	/// The moveGlobal* calls serve to move kinematically controlled dynamic actors 
	/// through the game world. 
	/// </summary>
	//void moveGlobalPose (const Matrix% mat);
	
	/// <summary>
	/// The moveGlobal* calls serve to move kinematically controlled dynamic actors 
	/// through the game world.
	/// </summary>
	/// <param name="vec">The desired pose for the kinematic actor, in the global frame. Range: rigid body transform.</param>
	void moveGlobalPosition (const EngineMath::Vector3% vec);
  
	/// <summary>
	/// The moveGlobal* calls serve to move kinematically controlled dynamic actors 
	/// through the game world.
	/// </summary>
	//void moveGlobalOrientation (const Matrix% mat);
  
	/// <summary>
	/// The moveGlobal* calls serve to move kinematically controlled dynamic actors 
	/// through the game world.
	/// </summary>
	/// <param name="quat">The desired orientation quaternion for the kinematic actor, in the global frame.</param>
	void moveGlobalOrientationQuat (EngineMath::Quaternion% quat);

	//---------------------
	//Center of mass
	//---------------------

	/// <summary>
	/// The setCMassOffsetLocal*() methods set the pose of the center of mass relative 
	/// to the actor. 
	/// </summary>
	//void setCMassOffsetLocalPose(Matrix mat);

	/// <summary>
	/// The setCMassOffsetLocal*() methods set the pose of the center of mass relative 
	/// to the actor. 
	/// </summary>
	/// <param name="vec">Mass frame offset relative to the actor frame. Range: position vector.</param>
	void setCMassOffsetLocalPosition(EngineMath::Vector3 vec);
  
	/// <summary>
	/// The setCMassOffsetLocal*() methods set the pose of the center of mass relative 
	/// to the actor. 
	/// </summary>
	//void setCMassOffsetLocalOrientation(Matrix mat);

	/// <summary>
	/// The setCMassOffsetGlobal*() methods set the pose of the center of mass relative 
	/// to world space. 
    /// </summary>
	//void setCMassOffsetGlobalPose(Matrix mat);

	/// <summary>
	/// The setCMassOffsetGlobal*() methods set the pose of the center of mass 
	/// relative to world space. 
	/// </summary>
	/// <param name="vec">Mass frame offset relative to the global frame. Range: position vector.</param>
	void setCMassOffsetGlobalPosition(EngineMath::Vector3 vec);

	/// <summary>
	/// The setCMassOffsetGlobal*() methods set the pose of the center of mass relative 
	/// to world space. 
	/// </summary>
	//void setCMassOffsetGlobalOrientation(Matrix mat);

	/// <summary>
	/// The setCMassGlobal*() methods move the actor by setting the pose of the center of mass. 
	/// </summary>
	//void setCMassGlobalPose(Matrix mat);

	/// <summary>
	/// The setCMassGlobal*() methods move the actor by setting the pose of the center of mass. 
	/// </summary>
	/// <param name="vec">Actors new position, from the transformation of the mass frame to the global frame. Range: position vector.</param>
	void setCMassGlobalPosition(EngineMath::Vector3 vec);

	/// <summary>
	/// The setCMassGlobal*() methods move the actor by setting the pose of the center of mass. 
	/// </summary>
	//void setCMassGlobalOrientation(Matrix mat);
  
	/// <summary>
	/// The getCMassLocal*() methods retrieve the center of mass pose relative to the actor. 
	/// </summary>
	//void getCMassLocalPose( Matrix% cMass );

	/// <summary>
	/// The getCMassLocal*() methods retrieve the center of mass pose relative to the actor. 
	/// </summary>
	/// <param name="cMass">The vector that will hold the result.</param>
	void getCMassLocalPosition( EngineMath::Vector3% cMass );
  
	/// <summary>
	/// The getCMassLocal*() methods retrieve the center of mass pose relative to the actor. 
	/// </summary>
	//void getCMassLocalOrientation( Matrix% cMass );
	
	/// <summary>
	/// The getCMassGlobal*() methods retrieve the center of mass pose in world space. 
	/// </summary>
	//void getCMassGlobalPose( Matrix% cMass );

	/// <summary>
	/// The getCMassGlobal*() methods retrieve the center of mass pose in world space. 
	/// </summary>
	/// <param name="cMass">The vector that will hold the result.</param>
	void getCMassGlobalPosition( EngineMath::Vector3% cMass );
	
	/// <summary>
	/// The getCMassGlobal*() methods retrieve the center of mass pose in world space. 
	/// </summary>
	//void getCMassGlobalOrientation( Matrix% cMass );

	/// <summary>
	/// Sets the mass of a dynamic actor. 
	/// </summary>
	/// <param name="mass">New mass value for the actor. Range: (0,inf)</param>
	void setMass(float mass);
  
	/// <summary>
	/// Retrieves the mass of the actor. 
	/// </summary>
	/// <returns>The mass.</returns>
	float getMass();

	/// <summary>
	/// Sets the inertia tensor, using a parameter specified in mass space coordinates. 
	/// </summary>
	/// <remarks>
	/// Note that such matrices are diagonal -- the passed vector is the diagonal.
	/// If you have a non diagonal world/actor space inertia tensor(3x3 matrix). 
	/// Then you need to diagonalize it and set an appropriate mass space transform. 
	/// See setCMassOffsetLocalPose().
	/// The actor must be dynamic.
	/// </remarks>
	/// <param name="m">New mass space inertia tensor for the actor. Range: inertia vector.</param>
	void setMassSpaceInertiaTensor(EngineMath::Vector3% m);
  
	/// <summary>
	/// Retrieves the diagonal inertia tensor of the actor relative to the mass coordinate frame. 
	/// </summary>
	/// <param name="inertiaTensor">The Vector3 that will hold the result.</param>
	void getMassSpaceInertiaTensor( EngineMath::Vector3% inertiaTensor );
  
	/// <summary>
	/// Retrieves the inertia tensor of the actor relative to the world coordinate frame. 
	/// </summary>
	//void getGlobalInertiaTensor( Matrix% inertiaTensor );
	
	/// <summary>
	/// Retrieves the inverse of the inertia tensor of the actor relative to the world 
	/// coordinate frame. 
	/// </summary>
	//void getGlobalInertiaTensorInverse( Matrix% inertiaTensorInv );

	/// <summary>
	/// Recomputes a dynamic actor's mass properties from its shapes. 
	/// </summary>
	/// <param name="density">Density scale factor of the shapes belonging to the actor. Range: [0,inf)</param>
	/// <param name="totalMass">Total mass of the actor(or zero). Range: [0,inf)</param>
	void updateMassFromShapes(float density, float totalMass);

	//-------------
	//Damping
	//-------------

	/// <summary>
	/// Sets the linear damping coefficient. 
	/// </summary>
	/// <param name="linDamp">Linear damping coefficient. Range: [0,inf)</param>
	void setLinearDamping(float linDamp);
	
	/// <summary>
	/// Retrieves the linear damping coefficient. 
	/// </summary>
	/// <returns>The linear damping coefficient associated with this actor.</returns>
	float getLinearDamping();
	  
	/// <summary>
	/// Sets the angular damping coefficient. 
	/// </summary>
	/// <param name="angDamp">Linear damping coefficient. Range: [0,inf)</param>
	void setAngularDamping(float angDamp);
	  
	/// <summary>
	/// Retrieves the angular damping coefficient. 
	/// </summary>
	/// <returns>The angular damping coefficient associated with this actor.</returns>
	float getAngularDamping();  

	//-------------
	//Velocity
	//-------------

	/// <summary>
	/// Sets the linear velocity of the actor.
	/// </summary>
	/// <param name="linVel">The velocity to set.</param>
	void setLinearVelocity(EngineMath::Vector3 linVel); 
   
	/// <summary>
	/// Sets the angular velocity of the actor. 
	/// </summary>
	/// <param name="angVel">The velocity to set.</param>
	void setAngularVelocity(EngineMath::Vector3 angVel);
	  
	/// <summary>
	/// Retrieves the linear velocity of an actor. 
	/// </summary>
	/// <param name="linVel">The vector3 that will hold the result.</param>
	void getLinearVelocity( EngineMath::Vector3% linVel );
	  
	/// <summary>
	/// Retrieves the angular velocity of the actor.
	/// </summary>
	/// <param name="angVel">The vector3 that will hold the result.</param>
	void getAngularVelocity( EngineMath::Vector3% angVel );
	   
	/// <summary>
	/// Lets you set the maximum angular velocity permitted for this actor.
	/// </summary>
	/// <param name="maxAngVel">Max allowable angular velocity for actor. Range: (0,inf)</param>
	void setMaxAngularVelocity(float maxAngVel);
	   
	/// <summary>
	/// Retrieves the maximum angular velocity permitted for this actor. 
	/// </summary>
	/// <returns>The max angular velocity.</returns>
	float getMaxAngularVelocity();

	//-------------------
	//Momentum
	//-------------------

	/// <summary>
	/// Sets the linear momentum of the actor.
	/// </summary>
	/// <param name="linMoment">New linear momentum. Range: momentum vector</param>
	void setLinearMomentum(EngineMath::Vector3 linMoment);
   
	/// <summary>
	/// Sets the angular momentum of the actor. 
	/// </summary>
	/// <param name="angMoment">New linear momentum. Range: momentum vector</param>
	void setAngularMomentum(EngineMath::Vector3 angMoment);
  
	/// <summary>
	/// Retrieves the linear momentum of an actor. 
	/// </summary>
	/// <param name="momentum">The vector to hold the result.</param>
	void getLinearMomentum( EngineMath::Vector3% momentum );
	  
	/// <summary>
	/// Retrieves the angular momentum of an actor.  
	/// </summary>
	/// <param name="momentum">The vector to hold the result.</param>
	void getAngularMomentum( EngineMath::Vector3% momentum );

	//---------------
	//Point Velocity
	//---------------

	/// <summary>
	/// Computes the velocity of a point given in world coordinates if it were 
	/// attached to the actor and moving with it. 
	/// </summary>
	/// <param name="point">Point we wish to determine the velocity for, defined in the global frame. Range: position vector.</param>
	/// <param name="result">The vector to hold the result.</param>
	void getPointVelocity(EngineMath::Vector3 point, EngineMath::Vector3% result);
  
	/// <summary>
	/// Computes the velocity of a point given in body local coordinates as if it 
	/// were attached to the actor and moving with it.  
	/// </summary>
	/// <param name="point">Point we wish to determine the velocity for, defined in the local frame. Range: position vector.</param>
	/// <param name="result">The vector to hold the result.</param>
	void getLocalPointVelocity(EngineMath::Vector3 point, EngineMath::Vector3% result);

	//-------
	//Shapes
	//-------

	/// <summary>
	/// Creates a new shape and adds it to the list of shapes of this actor. 
	/// <para>
	/// This invalidates the list returned by getShapes because the new shape
    /// will not be in the list.
	/// </para>
	/// <para>
	/// Mass properties of dynamic actors will not automatically be recomputed
	/// to reflect the new mass distribution implied by the shape. Follow this
    /// call with a call to updateMassFromShapes() to do that. Creating
    /// compounds with a very large number of shapes may adversly affect
    /// performance and stability. When performing collision tests between a
    /// pair of actors containing multiple shapes, a collision check is
    /// performed between each pair of shapes. This results in N^2 running time.
	/// </para>
	/// <para>
	/// Sleeping: Does NOT wake the actor up automatically. Only a subset of the
	/// shape types are supported in hardware scenes (others will fall back to
    /// running in software):
	/// </para>
	/// <list>
	/// <listheader>
	/// Fluids:
	/// </listheader>
	/// <item>Compounds are supported</item>
	/// <item>NxBoxShape</item>
	/// <item>NxCapsuleShape</item>
	/// <item>NxSphereShape</item>
	/// <item>NxConvexShape</item>
	/// </list>
	/// <list>
	/// <listheader>
	/// Hardware Rigid bodies:
	/// </listheader>
	/// <item>Compounds are supported</item>
	/// <item>NxBoxShape</item>
	/// <item>NxSphereShape</item>
	/// <item>NxCapsuleShape</item>
	/// <item>NxConvexShape (software fallback for > 32 vertices or faces)</item>
	/// <item>NxTriangleMeshShape</item>
	/// <item>NxPlaneShape</item>
	/// </list>
	/// <para>
	/// In addition mesh pages must be mapped into PPU memory for hardware
    /// scenes. No collision detection will be performed with portions of the
    /// mesh which have not been mapped to PPU memory. See
    /// NxTriangleMeshShape.mapPageInstance()
	/// </para>
	/// </summary>
	/// <param name="desc">The descriptor for the new shape. See e.g. NxSphereShapeDesc.</param>
	/// <returns>The newly create shape.</returns>
	PhysShape^ createShape(PhysShapeDesc^ desc);

	/// <summary>
	/// Deletes the specified shape. 
	/// <para>
	/// This invalidates the list returned by getShapes(). Avoid release
    /// calls while the scene is simulating (in between simulate() and
    /// fetchResults() calls). Note that mass properties for the actor are
    /// unchanged by this call unless updateMassFromShapes is also called.
	/// </para>
	/// <para>
	/// Sleeping: Does NOT wake the actor up automatically.
	/// </para>
	/// </summary>
	/// <param name="shape">Shape to be released.</param>
	void releaseShape(PhysShape^ shape);

	/// <summary>
	/// Returns the number of shapes assigned to the actor. 
	/// <para>
	/// You can use getShapes() to retrieve an array of shape pointers.
	/// </para>
	/// <para>
	/// For static actors it is not possible to release all actors associated
    /// with the shape. An attempt to remove the last shape will be ignored.
	/// </para>
	/// </summary>
	/// <returns>Number of shapes associated with this actor.</returns>
	unsigned int getNbShapes();

	/// <summary>
	/// Get a list of the shapes belonging to this actor. Adding or removing
    /// shapes will cause this list to no longer be valid. The list returned is
    /// a unique copy each time so avoid multiple calls.
	/// </summary>
	/// <returns>A new list with all the shapes in this actor.</returns>
	ShapeEnumerator^ getShapes();
};

}

}