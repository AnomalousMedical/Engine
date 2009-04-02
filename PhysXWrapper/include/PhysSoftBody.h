#pragma once

#include "Enums.h"
#include <string>
#include "AutoPtr.h"

namespace Engine
{

namespace Physics
{

ref class PhysSoftBodyDesc;
ref class PhysSoftBodyMesh;
ref class PhysMeshData;
ref class PhysScene;
ref class PhysShape;

/// <summary>
/// A wrapper for the SoftBody class.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
[Engine::Attributes::NativeSubsystemType]
public ref class PhysSoftBody
{
private:

internal:
	NxSoftBody* softBody;
	
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="softBody">The soft body to wrap.</param>
	PhysSoftBody(NxSoftBody* softBody);

public:
	/// <summary>
	/// Destructor.
	/// </summary>
	~PhysSoftBody();

	/// <summary>
	/// Saves the soft body descriptor. 
	/// </summary>
	/// <param name="desc">The descriptor used to retrieve the state of the object.</param>
	/// <returns>True on success.</returns>
	bool saveToDesc(PhysSoftBodyDesc^ desc);

	/// <summary>
	/// Returns a pointer to the corresponding soft body mesh.
	/// </summary>
	/// <returns>The soft body mesh associated with this soft body.</returns>
	PhysSoftBodyMesh^ getSoftBodyMesh();

	/// <summary>
	/// Sets the soft body volume stiffness in the range from 0 to 1.
	/// </summary>
	/// <param name="stiffness">The volume stiffness of this soft body.</param>
	void setVolumeStiffness(float stiffness);

	/// <summary>
	/// Retrieves the soft body volume stiffness.
	/// </summary>
	/// <returns>Volume stiffness of the soft body.</returns>
	float getVolumeStiffness();

	/// <summary>
	/// Sets the soft body stretching stiffness in the range from 0 to 1.
	/// </summary>
	/// <param name="stiffness">Stiffness of the soft body.</param>
	void setStretchingStiffness(float stiffness);

	/// <summary>
	///   Retrieves the soft body stretching stiffness.
	/// </summary>
	/// <returns>Stretching stiffness of the soft body.</returns>
	float getStretchingStiffness();

	/// <summary>
	/// Sets the damping coefficient in the range from 0 to 1.
	/// </summary>
	/// <param name="dampingCoefficient">Damping coefficient of the soft body.</param>
	void setDampingCoefficient(float dampingCoefficient);

	/// <summary>
	/// Retrieves the damping coefficient.
	/// </summary>
	/// <returns>Damping coefficient of the soft body.</returns>
	float getDampingCoefficient();

	/// <summary>
	/// Sets the soft body friction coefficient in the range from 0 to 1.
	/// </summary>
	/// <param name="friction">The friction of the soft body.</param>
	void setFriction(float friction);

	/// <summary>
	/// Retrieves the soft body friction coefficient.
	/// </summary>
	/// <returns>Friction coefficient of the soft body.</returns>
	float getFriction();

	/// <summary>
	/// Sets the soft body tear factor (must be larger than one).
	/// </summary>
	/// <param name="factor">The tear factor for the soft body.</param>
	void setTearFactor(float factor);

	/// <summary>
	/// Retrieves the soft body tear factor.
	/// </summary>
	/// <returns>Tear factor of the soft body.</returns>
	float getTearFactor();

	/// <summary>
	/// Sets the soft body attachment tear factor (must be larger than one).
	/// </summary>
	/// <param name="factor">The attachment tear factor for the soft body.</param>
	void setAttachmentTearFactor(float factor);

	/// <summary>
	/// Retrieves the attachment soft body tear factor.
	/// </summary>
	/// <returns>Tear attachment factor of the soft body.</returns>
	float getAttachmentTearFactor();

	/// <summary>
	/// Sets the soft body particle radius (must be positive).
	/// </summary>
	/// <param name="particleRadius">The particle radius of the soft body.</param>
	void setParticleRadius(float particleRadius);

	/// <summary>
	/// Gets the soft body particle radius.
	/// </summary>
	/// <returns>Particle radius of the soft body.</returns>
	float getParticleRadius();

	/// <summary>
	/// Gets the soft body density.
	/// </summary>
	/// <returns>Density of the soft body.</returns>
	float getDensity();

	/// <summary>
	/// Gets the relative grid spacing for the broad phase. The soft body is
    /// represented by a set of world aligned cubical cells in broad phase. The
    /// size of these cells is determined by multiplying the length of the
    /// diagonal of the AABB of the initial soft body size with this constant. 
	/// </summary>
	/// <returns>Relative grid spacing.</returns>
	float getRelativeGridSpacing();

	/// <summary>
	/// Retrieves the soft body solver iterations.
	/// </summary>
	/// <returns>Solver iterations of the soft body.</returns>
	System::UInt32 getSolverIterations();

	/// <summary>
	/// Sets the soft body solver iterations. 
	/// </summary>
	/// <param name="iterations">The new solver iteration count for the soft body.</param>
	void setSolverIterations(System::UInt32 iterations);

	/// <summary>
	/// Attaches the soft body to a shape. All soft body vertices currently inside the shape are attached. 
	/// <para>
	/// This method only works with primitive and convex shapes. Since the inside of
	/// a general triangle mesh is not clearly defined. Collisions with attached
	/// shapes are automatically switched off to increase the stability.
	/// </para>
	/// </summary>
	/// <param name="shape">Shape to which the soft body should be attached to.</param>
	/// <param name="flags">One or two way interaction, tearable or non-tearable.</param>
	void attachToShape(PhysShape^ shape, PhysSoftBodyAttachmentFlag flags);

	/// <summary>
	/// Attaches the soft body to all shapes, currently colliding. 
	/// <para>
	/// This method only works with primitive and convex shapes. Since the
	/// inside of a general triangle mesh is not clearly defined. Collisions
    /// with attached shapes are automatically switched off to increase the
    /// stability.
	/// </para>
	/// </summary>
	/// <param name="attachmentFlags">One or two way interaction, tearable or non-tearable.</param>
	void attachToCollidingShapes(PhysSoftBodyAttachmentFlag attachmentFlags);

	/// <summary>
	/// Detaches the soft body from a shape it has been attached to before. If
    /// the soft body has not been attached to the shape before, the call has no
    /// effect.
	/// </summary>
	/// <param name="shape">Shape from which the soft body should be detached.</param>
	void detachFromShape(PhysShape^ shape);

	/// <summary>
	/// Attaches a soft body vertex to a local position within a shape.
	/// </summary>
	/// <param name="vertexId">Index of the vertex to attach.</param>
	/// <param name="shape">Shape to attach the vertex to.</param>
	/// <param name="localPos">The position relative to the pose of the shape.</param>
	/// <param name="attachmentFlags">One or two way interaction, tearable or non-tearable.</param>
	void attachVertexToShape(System::UInt32 vertexId, PhysShape^ shape, EngineMath::Vector3 localPos, PhysSoftBodyAttachmentFlag attachmentFlags);

	/// <summary>
	/// Attaches a soft body vertex to a position in world space.
	/// </summary>
	/// <param name="vertexId">Index of the vertex to attach.</param>
	/// <param name="pos">The position in world space.</param>
	void attachVertexToGlobalPosition(System::UInt32 vertexId, EngineMath::Vector3% pos);

	/// <summary>
	/// Frees a previously attached soft body vertex.
	/// </summary>
	/// <param name="vertexId">Index of the vertex to free.</param>
	void freeVertex(System::UInt32 vertexId);

	/// <summary>
	/// [Experimental] Tears the soft body at a given vertex. 
	/// <para>
	/// Experimental feature, not yet fully supported. First the vertex is
	/// duplicated. The tetrahedra on one side of the split plane keep the
	/// original vertex. For all tetrahedra on the opposite side the original
    /// vertex is replaced by the new one. The split plane is defined by the
    /// world location of the vertex and the normal provided by the user. Note:
    /// TearVertex performs a user defined vertex split in contrast to an
    /// automatic split that is performed when the flag NX_SBF_TEARABLE is set.
    /// Therefore, tearVertex works even if NX_SBF_TEARABLE is not set in
    /// NxSoftBodyDesc.flags.
	/// </para>
	/// <para>
	/// Note: For tearVertex to work in hardware mode, the softBodyMesh has to
    /// be cooked with the flag NX_SOFTBODY_MESH_TEARABLE set in
    /// NxSoftBodyMeshDesc.flags.
	/// </para>
	/// </summary>
	/// <param name="vertexId">Index of the vertex to tear.</param>
	/// <param name="normal">The normal of the split plane.</param>
	/// <returns>true if the split had an effect (i.e. there were tetrahedra on both sides of the split plane)</returns>
	bool tearVertex(System::UInt32 vertexId, EngineMath::Vector3% normal);

	/// <summary>
	/// Executes a raycast against the soft body.
	/// </summary>
	/// <param name="worldRay">The ray in world space.</param>
	/// <param name="hit">The hit position.</param>
	/// <param name="vertexId">Index to the nearest vertex hit by the raycast.</param>
	/// <returns>true if the ray hits the soft body.</returns>
	bool raycast(EngineMath::Ray3% worldRay, EngineMath::Vector3% hit, System::UInt32 vertexId);

	/// <summary>
	/// Sets which collision group this soft body is part of.
	/// </summary>
	/// <param name="collisionGroup">The collision group for this soft body.</param>
	void setGroup(System::UInt16 collisionGroup);

	/// <summary>
	/// Retrieves the value set with setGroup(). 
	/// </summary>
	/// <returns>The collision group this soft body belongs to.</returns>
	System::UInt16 getGroup();

	/// <summary>
	/// Sets the user buffer wrapper for the soft body mesh.
	/// </summary>
	/// <param name="meshData">User buffer wrapper.</param>
	void setMeshData(PhysMeshData^ meshData);

	/// <summary>
	/// Returns a copy of the user buffer wrapper for the soft body mesh.
	/// </summary>
	/// <returns>User buffer wrapper.</returns>
	PhysMeshData^ getMeshData();

	/// <summary>
	/// Sets the position of a particular vertex of the soft body.
	/// </summary>
	/// <param name="position">New position of the vertex.</param>
	/// <param name="vertexId">Index of the vertex.</param>
	void setPosition(EngineMath::Vector3% position, System::UInt32 vertexId);

	/// <summary>
	/// Sets the positions of the soft body. 
	/// <para>
	/// The user must supply a buffer containing all positions (i.e same number of elements as number of vertices).
	/// </para>
	/// </summary>
	/// <param name="buffer">The user supplied buffer containing all positions for the soft body.</param>
	/// <param name="byteStride">The stride in bytes between the position vectors in the buffer.</param>
	void setPositions(void* buffer, System::UInt32 byteStride);

	/// <summary>
	/// Gets the position of a particular vertex of the soft body.
	/// </summary>
	/// <param name="vertexId">Index of the vertex.</param>
	/// <returns>The position of the vertex.</returns>
	EngineMath::Vector3 getPosition(System::UInt32 vertexId);

	/// <summary>
	/// Gets the positions of the soft body. 
	/// <para>
	/// The user must supply a buffer containing all positions (i.e same number of elements as number of vertices).
	/// </para>
	/// </summary>
	/// <param name="buffer">The user supplied buffer to hold all positions of the soft body.</param>
	/// <param name="byteStride">The stride in bytes between the position vectors in the buffer.</param>
	void getPositions(void* buffer, System::UInt32 byteStride);

	/// <summary>
	/// Sets the velocity of a particular vertex of the soft body.
	/// </summary>
	/// <param name="velocity">New velocity of the vertex.</param>
	/// <param name="vertexId">Index of the vertex.</param>
	void setVelocity(EngineMath::Vector3% velocity, System::UInt32 vertexId);

	/// <summary>
	/// Sets the velocities of the soft body. 
	/// <para>
	/// The user must supply a buffer containing all velocities (i.e same number of elements as number of vertices).
	/// </para>
	/// </summary>
	/// <param name="buffer">The user supplied buffer to hold all positions of the soft body.</param>
	/// <param name="byteStride">The stride in bytes between the position vectors in the buffer.</param>
	void setVelocities(void* buffer, System::UInt32 byteStride);

	/// <summary>
	/// Gets the velocity of a particular vertex of the soft body. 
	/// </summary>
	/// <param name="vertexId">Index of the vertex.</param>
	/// <returns>The velocity of index.</returns>
	EngineMath::Vector3 getVelocity(System::UInt32 vertexId);

	/// <summary>
	/// Gets the velocities of the soft body. 
	/// <para>
	/// The user must supply a buffer large enough to hold all velocities (i.e same number of elements as number of vertices).
	/// </para>
	/// </summary>
	/// <param name="buffer">The user supplied buffer to hold all velocities of the soft body.</param>
	/// <param name="byteStride">The stride in bytes between the velocity vectors in the buffer.</param>
	void getVelocities(void* buffer, System::UInt32 byteStride);

	/// <summary>
	///   Gets the number of soft body particles.
	/// </summary>
	/// <returns>The number of soft body particles.</returns>
	System::UInt32 getNumberOfParticles();

	/// <summary>
	/// Queries the soft body for the currently interacting shapes. Must be
    /// called prior to saveStateToStream in order for attachments and collisons
    /// to be saved.
	/// </summary>
	/// <returns></returns>
	System::UInt32 queryShapePointers();

	/// <summary>
	/// Gets the byte size of the current soft body state.
	/// </summary>
	/// <returns>The byte size of the current state.</returns>
	System::UInt32 getStateByteSize();

	/// <summary>
	/// Sets the collision response coefficient. 
	/// </summary>
	/// <param name="coefficient">The collision response coefficient (0 or greater).</param>
	void setCollisionResponseCoefficient(float coefficient);

	/// <summary>
	/// Retrieves the collision response coefficient. 
	/// </summary>
	/// <returns>The collision response coefficient.</returns>
	float getCollisionResponseCoefficient();

	/// <summary>
	/// Sets the attachment response coefficient.
	/// </summary>
	/// <param name="coefficient">The attachment response coefficient in the range from 0 to 1.</param>
	void setAttachmentResponseCoefficient(float coefficient);

	/// <summary>
	/// Retrieves the attachment response coefficient.
	/// </summary>
	/// <returns>The attachment response coefficient.</returns>
	float getAttachmentResponseCoefficient();

	/// <summary>
	/// Sets the response coefficient for collisions from fluids to this soft body.
	/// </summary>
	/// <param name="coefficient">The response coefficient.</param>
	void setFromFluidResponseCoefficient(float coefficient);

	/// <summary>
	/// Retrieves response coefficient for collisions from fluids to this soft body.
	/// </summary>
	/// <returns>The response coefficient.</returns>
	float getFromFluidResponseCoefficient();

	/// <summary>
	/// Sets the response coefficient for collisions from this soft body to fluids.
	/// </summary>
	/// <param name="coefficient">The response coefficient.</param>
	void setToFluidResponseCoefficient(float coefficient);

	/// <summary>
	/// Retrieves response coefficient for collisions from this soft body to fluids.
	/// </summary>
	/// <returns>The response coefficient.</returns>
	float getToFluidResponseCoefficient();

	/// <summary>
	/// Sets an external acceleration which affects all non attached particles of the soft body.
	/// </summary>
	/// <param name="acceleration">The acceleration vector (unit length / s^2).</param>
	void setExternalAcceleration(EngineMath::Vector3 acceleration);

	/// <summary>
	/// Retrieves the external acceleration which affects all non attached particles of the soft body.
	/// </summary>
	/// <returns>The acceleration vector (unit length / s^2).</returns>
	EngineMath::Vector3 getExternalAcceleration();

	/// <summary>
	/// If the NX_SBF_ADHERE flag is set the soft body moves partially in the
    /// frame of the attached actor. 
	/// <para>
	/// This feature is useful when the soft body is attached to a fast moving
    /// shape. In that case the soft body adheres to the shape it is attached to
    /// while only velocities below the parameter minAdhereVelocity are used for
    /// secondary effects.
	/// </para>
	/// </summary>
	/// <param name="velocity">The minimal velocity for the soft body to adhere (unit length / s).</param>
	void setMinAdhereVelocity(float velocity);

	/// <summary>
	/// If the NX_SBF_ADHERE flag is set the soft body moves partially in the
    /// frame of the attached actor.
	/// <para>
	/// This feature is useful when the soft body is attached to a fast moving
    /// shape. In that case the soft body adheres to the shape it is attached to
    /// while only velocities below the parameter minAdhereVelocity are used for
    /// secondary effects.
	/// </para>
	/// </summary>
	/// <returns>Returns the minimal velocity for the soft body to adhere (unit length / s).</returns>
	float getMinAdhereVelocity();

	/// <summary>
	/// Returns true if this soft body is sleeping. 
	/// <para>
	/// When a soft body does not move for a period of time, it is no longer
    /// simulated in order to save time. This state is called sleeping. However,
    /// because the object automatically wakes up when it is either touched by
    /// an awake object, or one of its properties is changed by the user, the
    /// entire sleep mechanism should be transparent to the user.
	/// </para>
	/// <para>
	/// If a soft body is asleep after the call to NxScene::fetchResults()
    /// returns, it is guaranteed that the position of the soft body vertices
    /// was not changed. You can use this information to avoid updating
    /// dependent objects.
	/// </para>
	/// </summary>
	/// <returns>True if the soft body is sleeping.</returns>
	bool isSleeping();

	/// <summary>
	/// Returns the linear velocity below which a soft body may go to sleep. A
    /// soft body whose linear velocity is above this threshold will not be put
    /// to sleep.
	/// </summary>
	/// <returns>The threshold linear velocity for sleeping.</returns>
	float getSleepLinearVelocity();

	/// <summary>
	/// Sets the linear velocity below which a soft body may go to sleep. A soft
    /// body whose linear velocity is above this threshold will not be put to
    /// sleep.
	/// <para>
	/// If the threshold value is negative, the velocity threshold is set using
    /// the NxPhysicsSDK's NX_DEFAULT_SLEEP_LIN_VEL_SQUARED parameter.
	/// </para>
	/// </summary>
	/// <param name="threshold">Linear velocity below which a soft body may sleep. Range: (0,inf]</param>
	void setSleepLinearVelocity(float threshold);

	/// <summary>
	/// Wakes up the soft body if it is sleeping. The wakeCounterValue
    /// determines how long until the soft body is put to sleep, a value of zero
    /// means that the soft body is sleeping. wakeUp(0) is equivalent to
    /// NxSoftBody::putToSleep().
	/// </summary>
	/// <param name="wakeCounterValue">New sleep counter value. Range: [0,inf]</param>
	void wakeUp(float wakeCounterValue);

	/// <summary>
	/// Forces the soft body to sleep. 
	/// </summary>
	void putToSleep();

	/// <summary>
	/// Set the flags.
	/// </summary>
	/// <param name="flags">The flags to set.</param>
	void setFlags(PhysSoftBodyFlag flags);

	/// <summary>
	/// Get the flags.
	/// </summary>
	/// <returns>The current flags.</returns>
	PhysSoftBodyFlag getFlags();

	/// <summary>
	/// Applies a force (or impulse) defined in the global coordinate frame, to
    /// a particular vertex of the soft body. 
	/// <para>
	/// Because forces are reset at the end of every timestep, you can maintain
    /// a total external force on an object by calling this once every frame.
	/// </para>
	/// <para>
	/// NxForceMode determines if the force is to be conventional or impulsive.
	/// </para>
	/// </summary>
	/// <param name="force">Force/impulse to add, defined in the global frame. Range: force vector</param>
	/// <param name="vertexId">Number of the vertex to add the force at. Range: position vector</param>
	/// <param name="mode">The mode to use when applying the force/impulse (see NxForceMode, supported modes are NX_FORCE, NX_IMPULSE, NX_ACCELERATION, NX_VELOCITY_CHANGE)</param>
	void addForceAtVertex(EngineMath::Vector3% force, System::UInt32 vertexId, ForceMode mode);

	/// <summary>
	/// Applies a radial force (or impulse) at a particular position. All
    /// vertices within radius will be affected with a quadratic drop-off. 
	/// <para>
	/// Because forces are reset at the end of every timestep, you can maintain
    /// a total external force on an object by calling this once every frame.
	/// </para>
	/// <para>
	/// NxForceMode determines if the force is to be conventional or impulsive.
	/// </para>
	/// </summary>
	/// <param name="position">Position to apply force at.</param>
	/// <param name="magnitude">Magnitude of the force/impulse to apply.</param>
	/// <param name="radius">The sphere radius in which particles will be affected. Range: position vector</param>
	/// <param name="mode">The mode to use when applying the force/impulse (see NxForceMode, supported modes are NX_FORCE, NX_IMPULSE, NX_ACCELERATION, NX_VELOCITY_CHANGE).</param>
	void addForceAtPos(EngineMath::Vector3% position, float magnitude, float radius, ForceMode mode);

	/// <summary>
	/// Retrieves the actor's force field material index, default index is 0.
	/// </summary>
	/// <returns>The force field material.</returns>
	System::UInt16 getForceFieldMaterial();

	/// <summary>
	/// Sets the actor's force field material index, default index is 0.
	/// </summary>
	/// <param name="material">The index of the force field material.</param>
	void setForceFieldMaterial(System::UInt16 material);
};

}

}

/*****
* Not implemented
	void getWorldBounds(NxBounds3& bounds);

	void attachToShape(NxShape *shape, System::UInt32 attachmentFlags);

	void detachFromShape(NxShape *shape);

	void attachVertexToShape(System::UInt32 vertexId, NxShape *shape, EngineMath::Vector3 &localPos, System::UInt32 attachmentFlags);

	void setGroupsMask(NxGroupsMask& groupsMask);

	NxGroupsMask getGroupsMask();

	void setSplitPairData(NxSoftBodySplitPairData& splitPairData);

	NxSoftBodySplitPairData getSplitPairData();

	void setValidBounds(NxBounds3& validBounds);

	void getValidBounds(NxBounds3& validBounds);

	void getShapePointers(NxShape** shapePointers,System::UInt32 *flags);

	void setShapePointers(NxShape** shapePointers,unsigned int numShapes);

	void saveStateToStream(NxStream& stream, bool permute);

	void loadStateFromStream(NxStream& stream);

	bool overlapAABBTetrahedra(NxBounds3& bounds, System::UInt32& nb, System::UInt32*& indices);

	NxCompartment* getCompartment();

	PhysScene^ getScene();
*******/