using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This interface can be injected if an object wishes to offer a destroy method for
    /// its users. This will schedule the object for Dispose on the next call to IObjectResolver.Flush.
    /// </summary>
    public interface IDestructionRequest
    {
        /// <summary>
        /// Request the owning object be destroyed on the next flush.
        /// </summary>
        void RequestDestruction();

        /// <summary>
        /// Block the destruction of this object until the returned handle is disposed. Once the
        /// handle is disposed, the object may be scheduled for destruction right away depending
        /// on its current status.
        /// <para>
        /// Call this when starting a background load using a coroutine. Do it on the first line of
        /// the coroutine before you give up control. Use a using statement and the block will be
        /// automatically released when the coroutine completes and loading is complete.
        /// </para>
        /// <para>
        /// This is only intended to be called from the main thread.
        /// </para>
        /// </summary>
        /// <returns>A handle that must be disposed to allow the object to be destroyed.</returns>
        IDisposable BlockDestruction();

        /// <summary>
        /// This will return true if the object has been requested for destruction. This can be checked
        /// during background load to cancel loading the rest of the way it if desired.
        /// </summary>
        bool DestructionRequested { get; }
    }
}
