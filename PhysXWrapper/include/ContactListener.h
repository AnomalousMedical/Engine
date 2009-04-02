#pragma once

#include "Enums.h"

namespace Physics
{

ref class PhysActor;
ref class ContactIterator;

/// <summary>
/// This interface allows clients to listen to Contact Reports.
/// </summary>
public interface class ContactListener
{
public:
	/// <summary>
	/// Called when contact with another object has occured.  The ContactIterator
	/// is only valid for the duration of the function call.
	/// </summary>
	/// <remarks>
	/// The simulation state must NOT be modified from within this callback.
	/// </remarks>
	/// <param name="contactWith">The item being contacted with.</param>
	/// <param name="myself">The owner item contacting the other item.</param>
	/// <param name="contacts">The contact iterator with the contact point information.</param>
	/// <param name="contactType">The type of contact.</param>
	void contact( PhysActor^ contactWith, PhysActor^ myself, ContactIterator^ contacts, ContactPairFlag contactType );

};

}