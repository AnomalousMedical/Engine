#pragma once

class NxShapeDesc;
#include "Enums.h"

namespace PhysXWrapper
{

ref class PhysMaterial;

/// <summary>
/// Super class for shape descriptors.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysShapeDesc
{
protected:
	PhysShapeDesc(NxShapeDesc* shapeDesc, System::String^ name);
	System::String^ name;
	System::String^ materialName;

internal:
	NxShapeDesc* shapeDesc;

public:
	/// <summary>
	/// Set the pose of the shape in the coordinate frame of the owning actor.
	/// </summary>
	void setLocalPose(EngineMath::Vector3 trans, EngineMath::Quaternion rot);

	/// <summary>
	/// Set the material of this shape.
	/// </summary>
	void setMaterial(PhysMaterial^ material);

	/// <summary>
	/// A combination of ShapeFlag values.
	/// </summary>
	property ShapeFlag ShapeFlags
	{
		ShapeFlag get();
		void set(ShapeFlag shapeFlags);
	}

	/// <summary>
	/// The shape group for the shape.
	/// </summary>
	property unsigned short Group
	{
		unsigned short get();
		void set(unsigned short group);
	}

	/// <summary>
	/// The material index of the shape.
	/// </summary>
	property unsigned short MaterialIndex
	{
		unsigned short get();
	}

	/// <summary>
	/// The name of the material assigned to this shape.
	/// </summary>
	property System::String^ MaterialName
	{
		System::String^ get();
	}
	
	//property nxccdskeleton

	/// <summary>
	/// density of this individual shape when computing mass inertial properties 
	/// for a rigidbody (unless a valid mass >0.0 is provided). Note that this will 
	/// only be used if the body has a zero inertia tensor, or if you call 
	/// PhysActor::updateMassFromShapes explicitly.
	/// </summary>
	property float Density
	{
		float get();
		void set(float density);
	}

	/// <summary>
	/// Mass of this individual shape when computing mass inertial properties for a 
	/// rigidbody. When mass &lt; = 0.0 then density and volume determine the mass. Note that 
	/// this will only be used if the body has a zero inertia tensor, or if you call 
	/// PhysActor::updateMassFromShapes explicitly.
	/// </summary>
	property float Mass
	{
		float get();
		void set(float mass);
	}

	/// <summary>
	/// Specifies by how much shapes can interpenetrate.
	/// </summary>
	property float SkinWidth
	{
		float get();
		void set(float skinWidth);
	}

	/// <summary>
	/// The name of the shape.
	/// </summary>
	property System::String^ Name
	{
		System::String^ get();
		void set(System::String^ name);
	}

	//property NxGroupsMask groupsMask

	/// <summary>
	/// A combination of NxShapeCompartmentType values.
	/// </summary>
	property unsigned int NonInteractingCompartmentTypes
	{
		unsigned int get();
		void set(unsigned int nonInteractingCompartmentTypes);
	}
};

}