using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using System;
using System.Numerics;

namespace BepuPlugin
{
    //Bepuphysics v2 doesn't have any concept of 'events'. It has callbacks that report the current status of contact manifolds.
    //Events can be built around those callbacks. This demo shows one way of doing that.

    //It's worth noting a few things about this event handler approach:
    //1) Every contact event is passed through the save event handler logic. It doesn't support hooking up unique logic to individual collidables.
    //That can be worked around by rewriting it to be a little more idiomatic, wrapping the handler in another system which uses delegates, or waiting for C# to support unmanaged delegates:
    //https://github.com/dotnet/csharplang/blob/master/proposals/static-delegates.md
    //2) This only implements 'on contact added' events. It wouldn't be very difficult to add OnContactRemoved or OnTouching events.
    //3) All event handlers execute from the multithreaded context of the simulation execution, so you have to be careful about what the event handlers do.
    //A "deferred" event model could be built on top of this.
    //4) This model doesn't expose the contact data for direct modification; it operates strictly as a notification.
    //5) This does quite a bit of work and slows down the narrow phase. 
    //6) This provides no insight into the constraints associated with these contacts. For example, if you want to spawn particles in response to heavy collisions or play sounds
    //with volume dependent on the impact force, you will need to go pull impulse data from the solver. Further, those sorts of use cases often only apply to nearby objects,
    //so a listener based model isn't ideal. Instead, querying nearby active objects and examining their contact constraints would be much more direct.
    //7) Contacts can be created speculatively. A contact existing does not guarantee a nonnegative penetration depth! We make no attempt to hide this fact in the demos.
    //You could modify this to only consider nonnegative depth contacts as existing for the purposes of events, but it gets more complicated.
    //8) There are other ways of pulling contact data. For example, check out how the contact line extractor works in the DemoRenderer. 
    //It pulls data directly from the solver data and could be extended to pull other information like impulses.

    public interface IContactEventHandler
    {
        void OnContactContinues<TManifold>(CollidableReference eventSource, CollidablePair pair, ref TManifold contactManifold,
            in Vector3 contactOffset, in Vector3 contactNormal, float depth, int featureId, int contactIndex, int workerIndex) where TManifold : struct, IContactManifold<TManifold>;

        void OnContactAdded<TManifold>(CollidableReference eventSource, CollidablePair pair, ref TManifold contactManifold,
            in Vector3 contactOffset, in Vector3 contactNormal, float depth, int featureId, int contactIndex, int workerIndex) where TManifold : struct, IContactManifold<TManifold>;
        void AddContinueHandler(CollidableReference collidable, Action<CollisionEvent> handler);
        void RemoveContinueHandler(CollidableReference collidable);
    }
}
