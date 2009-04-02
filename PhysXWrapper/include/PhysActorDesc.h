#pragma once

#include "AutoPtr.h"
#include "Enums.h"

class NxActorDesc;

namespace Physics
{

ref class PhysShapeDesc;
ref class PhysBodyDesc;

typedef System::Collections::Generic::LinkedList<PhysShapeDesc^> ShapeList;

/// <summary>
/// Wrapper for the NxActorDesc class.  
/// Actor Descriptor. This structure is used to save and load the state of PhysActor objects.
/// See PhysX docs for more information.
/// </summary>
[Engine::Attributes::DoNotSaveAttribute]
public ref class PhysActorDesc
{
private:
	PhysBodyDesc^ body;
	ShapeList^ shapeList;

internal:
	//NxActorDesc is avaliable internally for easy access.
	AutoPtr<NxActorDesc> actorDesc;

public:
	/// <summary>
	/// Constructor, takes a unique name.
	/// </summary>
	PhysActorDesc();

	/// <summary>
	/// (re)sets the description to the default.
	/// </summary>
	void setToDefault();

	/// <summary>
	/// Returns true if the descriptor is valid.
	/// </summary>
	bool isValid();

	/// <summary>
	/// Adds a shape to the actor desc.
	/// </summary>
	/// <param name="shape">The shape to add.</param>
	void addShape(PhysShapeDesc^ shape);

	/// <summary>
	/// Removes a shape from the actor desc.
	/// </summary>
	/// <param name="shape">The shape to remove.</param>
	void removeShape(PhysShapeDesc^ shape);

	/// <summary>
	/// Clears all shapes on the actor desc.
	/// </summary>
	void clearShapes();

	/// <summary>
	/// Set the global position of the actor that will be created by this description.
	/// </summary>
	/// <param name="translation"></param>
	/// <param name="rotation"></param>
	void setGlobalPose(EngineMath::Vector3 translation, EngineMath::Quaternion rotation);

	/// <summary>
	/// Body descriptor, null for static actors.
	/// </summary>
	property PhysBodyDesc^ Body
	{
		PhysBodyDesc^ get();
		void set(PhysBodyDesc^ desc);
	}

	/// <summary>
	/// Density used during mass/inertia computation.
	/// </summary>
	property float Density
	{
		float get();
		void set(float density);
	}

	/// <summary>
	/// Combination of ActorFlag flags.  Default None.
	/// </summary>
	property unsigned int Flags
	{
		unsigned int get();
		void set(unsigned int flags);
	}

	/// <summary>
	/// The actors group. Default 0.
	/// </summary>
	property unsigned short Group
	{
		unsigned short get();
		void set(unsigned short group);
	}

	/// <summary>
	/// Dominance group for this actor.
	/// </summary>
	/// <remarks>
	/// NxDominanceGroup is a 5 bit group identifier (legal range from 0 to 31). 
	/// The NxScene::setDominanceGroupPair() lets you set certain behaviors for 
	/// pairs of dominance groups. By default every actor is created in group 0. 
	/// Static actors must stay in group 0.
	/// </remarks>
	property unsigned short DominanceGroup
	{
		unsigned short get();
		void set(unsigned short group);
	}

	/// <summary>
	/// Combination of ContactPairFlag flags. Default None.
	/// </summary>
	property ContactPairFlag ContactReportFlags
	{
		ContactPairFlag get();
		void set(ContactPairFlag flags);
	}

	/// <summary>
	/// Force Field Material Index, index != 0 has to be created.  Default 0.
	/// </summary>
	property unsigned short ForceFieldMaterial
	{
		unsigned short get();
		void set(unsigned short mat);
	}

	//NxCompartment compartment
};

}