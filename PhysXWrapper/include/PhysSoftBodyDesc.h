#pragma once

#include "AutoPtr.h"
#include "Enums.h"

class NxSoftBodyDesc;

namespace PhysXWrapper
{

ref class PhysSoftBodyMesh;
ref class PhysMeshData;

/// <summary>
/// Descriptor class for PhysSoftBody. 
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysSoftBodyDesc
{
private:
	PhysSoftBodyMesh^ softBodyMesh;
	PhysMeshData^ meshData;

internal:
	AutoPtr<NxSoftBodyDesc> desc;

public:
	/// <summary>
	/// Constructor.
	/// </summary>
	PhysSoftBodyDesc();

	/// <summary>
	/// Set the global position of the soft body.
	/// </summary>
	/// <param name="position">The position of the soft body.</param>
	/// <param name="rotation">The rotation of the soft body.</param>
	void setGlobalPose(EngineMath::Vector3 position, EngineMath::Quaternion rotation);
	
	/// <summary>
	/// The cooked soft body mesh.
	/// </summary>
	/// <value></value>
	property PhysSoftBodyMesh^ SoftBodyMesh 
	{
		PhysSoftBodyMesh^ get();
		void set(PhysSoftBodyMesh^ value);
	}

	/// <summary>
	/// Size of the particles used for collision detection. 
	/// <para>
	/// The particle radius is usually a fraction of the overall extent of the
    /// soft body and should not be set to a value greater than that. A good
    /// value is the maximal distance between two adjacent soft body particles
    /// in their rest pose. Visual artifacts or collision problems may appear if
    /// the particle radius is too small.
	/// </para>
	/// </summary>
	property float ParticleRadius 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Density of the soft body (mass per volume).
	/// </summary>
	property float Density 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Volume stiffness of the soft body in the range 0 to 1. 
	/// <para>
	/// Defines how strongly the soft body counteracts deviations from the rest
    /// volume. Only has an effect if the flag NX_SBF_VOLUME_CONSERVATION is
    /// set.
	/// </para>
	/// <para>
	/// Default: 1.0 
	/// </para>
	/// </summary>
	property float VolumeStiffness 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Stretching stiffness of the soft body in the range 0 to 1. 
	/// <para>
	/// Defines how strongly the soft body counteracts deviations from the rest
    /// lengths of edges.
	/// </para>
	/// <para>
	/// Note: stretching stiffness must be larger than 0. Even if the stretching
    /// stiffness is set very low, tetrahedra edges will cease to stretch
    /// further if their length exceeds a certain internal limit. This is done
    /// to prevent heavily degenerated tetrahedral elements which could occur
    /// otherwise.
	/// </para>
	/// <para>
	/// Default: 1.0 
	/// </para>
	/// </summary>
	property float StretchingStiffness 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Spring damping of the soft body in the range 0 to 1. 
	/// <para>
	/// Only has an effect if the flag NX_SBF_DAMPING is set.
	/// </para>
	/// <para>
	/// Default: 0.5 
	/// </para>
	/// </summary>
	property float DampingCoefficient 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Friction coefficient in the range 0 to 1. 
	/// <para>
	///	Defines the damping of the velocities of soft body particles that are in contact.
	/// </para>
	/// <para>
	/// Default: 0.5 
	/// </para>
	/// </summary>
	property float Friction 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// If the flag NX_SBF_TEARABLE is set, this variable defines the elongation factor that causes the soft body to tear. 
	/// <para>
	/// Note:
	/// Experimental feature.
	/// Must be larger than 1. Make sure meshData.maxVertices and the corresponding buffers in meshData are substantially larger (e.g. 2x) then the number of original vertices since tearing will generate new vertices.
	/// When the buffer cannot hold the new vertices anymore, tearing stops.
	/// </para>
	/// <para>
	/// Default: 1.5 
	/// </para>
	/// <para>
	/// Range: (1,inf)
	/// </para>
	/// </summary>
	property float TearFactor 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Defines a factor for the impulse transfer from the soft body to colliding rigid bodies. 
	/// <para>
	/// Only has an effect if NX_SBF_COLLISION_TWOWAY is set.
	/// </para>
	/// <para>
	/// Default: 0.2 
	/// </para>
	/// <para>
	/// Range: [0,inf)
	/// </para>
	/// </summary>
	property float CollisionResponseCoefficient 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Defines a factor for the impulse transfer from the soft body to attached rigid bodies. 
	/// <para>
	/// Only has an effect if the mode of the attachment is set to NX_SOFTBODY_ATTACHMENT_TWOWAY.
	/// </para>
	/// <para>
	/// Default: 0.2 
	/// </para>
	/// <para>
	/// Range: [0,1]
	/// </para>
	/// </summary>
	property float AttachmentResponseCoefficient 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// If the flag NX_SOFTBODY_ATTACHMENT_TEARABLE is set in the attachment
    /// method of NxSoftBody, this variable defines the elongation factor that
    /// causes the attachment to tear. 
	/// <para>
	/// Must be larger than 1.
	/// </para>
	/// <para>
	/// Default: 1.5 
	/// </para>
	/// <para>
	/// Range: (1,inf)
	/// </para>
	/// </summary>
	property float AttachmentTearFactor 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Defines a factor for the impulse transfer from this soft body to colliding fluids. 
	/// <para>
	/// Only has an effect if the NX_SBF_FLUID_COLLISION flag is set.
	/// </para>
	/// <para>
	/// Default: 1.0 
	/// </para>
	/// <para>
	/// Range: [0,inf)
	/// </para>
	/// <para>
	/// Note: Large values can cause instabilities
	/// </para>
	/// </summary>
	property float ToFluidResponseCoefficient 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Defines a factor for the impulse transfer from colliding fluids to this soft body. 
	/// <para>
	/// Only has an effect if the NX_SBF_FLUID_COLLISION flag is set.
	/// </para>
	/// <para>
	/// Default: 1.0 
	/// </para>
	/// <para>
	/// Range: [0,inf)
	/// </para>
	/// <para>
	/// Note: Large values can cause instabilities
	/// </para>
	/// </summary>
	property float FromFluidResponseCoefficient 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// If the NX_SBF_ADHERE flag is set the soft body moves partially in the frame of the attached actor. 
	/// <para>
	/// This feature is useful when the soft body is attached to a fast moving shape. In that case the soft body adheres to the shape it is attached to while only velocities below the parameter minAdhereVelocity are used for secondary effects.
	/// </para>
	/// <para>
	/// Default: 1.0 
	/// </para>
	/// <para>
	/// Range: [0,inf)
	/// </para>
	/// </summary>
	property float MinAdhereVelocity 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Number of solver iterations. 
	/// <para>
	/// Note: Small numbers make the simulation faster while the soft body gets less stiff.
	/// </para>
	/// <para>
	/// Default: 5 Range: [1,inf)
	/// </para>
	/// </summary>
	property System::UInt32 SolverIterations 
	{
		System::UInt32 get();
		void set(System::UInt32 value);
	}

	/// <summary>
	/// External acceleration which affects all non attached particles of the soft body. 
	/// Default: (0,0,0)
	/// </summary>
	property EngineMath::Vector3 ExternalAcceleration 
	{
		EngineMath::Vector3 get();
		void set(EngineMath::Vector3 value);
	}

	/// <summary>
	/// The soft body wake up counter. 
	/// <para>
	/// Range: [0,inf)
	/// </para>
	/// <para>
	/// Default: 20.0f*0.02f
	/// </para>
	/// </summary>
	property float WakeUpCounter 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Maximum linear velocity at which the soft body can go to sleep. 
	/// <para>
	/// If negative, the global default will be used.
	/// </para>
	/// <para>
	/// Range: [0,inf)
	/// </para>
	/// <para>
	/// Default: -1.0
	/// </para>
	/// </summary>
	property float SleepLinearVelocity 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// The buffers in meshData are used to communicate the dynamic data of the
    /// soft body back to the user. 
	/// <para>
	/// For example vertex positions and connectivity (tetrahedra). The internal
    /// order of the contents of meshData's buffers will remain the same as that
    /// in the initial mesh data used to create the mesh.
	/// </para>
	/// </summary>
	property PhysMeshData^ MeshData 
	{
		PhysMeshData^ get();
	}

	/// <summary>
	/// Sets which collision group this soft body is part of. 
	/// <para>
	/// Range: [0, 31] Default: 0
	/// </para>
	/// </summary>
	property System::UInt16 CollisionGroup 
	{
		System::UInt16 get();
		void set(System::UInt16 value);
	}

	/// <summary>
	/// Force Field Material Index, index != 0 has to be created. 
	/// Default: 0 
	/// </summary>
	property System::UInt16 ForceFieldMaterial 
	{
		System::UInt16 get();
		void set(System::UInt16 value);
	}

	/// <summary>
	/// This parameter defines the size of grid cells for collision detection. 
	/// <para>
	/// The soft body is represented by a set of world aligned cubical cells in
    /// broad phase. The size of these cells is determined by multiplying the
    /// length of the diagonal of the AABB of the initial soft body size with
    /// this constant.
	/// </para>
	/// <para>
	/// Range: [0.01,inf)
	/// </para>
	/// <para>
	/// Default: 0.25 
	/// </para>
	/// </summary>
	property float RelativeGridSpacing 
	{
		float get();
		void set(float value);
	}

	/// <summary>
	/// Flag bits. 
	/// Default: NX_SBF_GRAVITY | NX_SBF_VOLUME_CONSERVATION
	/// </summary>
	property PhysSoftBodyFlag Flags 
	{
		PhysSoftBodyFlag get();
		void set(PhysSoftBodyFlag value);
	}
};

}