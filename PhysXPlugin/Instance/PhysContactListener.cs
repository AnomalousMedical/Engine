using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;

namespace PhysXPlugin
{
    public interface PhysContactListener
    {
        /// <summary>
	    /// Called when contact with another object has occured. The
        /// ContactIterator is only valid for the duration of the function call.
        /// Note that this function can be called with null values for the
        /// contactWith and myself arguments. This will indicate that these
        /// elements have been deleted. This can happen during
        /// NX_NOTIFY_ON_END_TOUCH or NX_NOTIFY_ON_END_TOUCH_FORCE_THRESHOLD
        /// according to the PhysX documents.
	    /// </summary>
	    /// <remarks>
	    /// The simulation state must NOT be modified from within this callback.
	    /// </remarks>
	    /// <param name="contactWith">The item being contacted with. This will be null if the PhysActorElement being collided with is deleted.</param>
	    /// <param name="myself">The owner item contacting the other item. This will be null if the PhysActorElement is deleted.</param>
	    /// <param name="contacts">The contact iterator with the contact point information.</param>
	    /// <param name="contactType">The type of contact.</param>
        void onContact(PhysActorElement contactWith, PhysActorElement myself, ContactIterator contacts, ContactPairFlag contactType);
    }
}
