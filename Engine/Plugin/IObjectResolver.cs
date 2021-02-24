using System;

namespace Engine
{
    public interface IObjectResolver : IDisposable
    {
        /// <summary>
        /// Resolve an instance of T. This will use the dependency injector to find T and resolve it.
        /// 
        /// You will not need to call Dispose on any of these instances even if they implement it. 
        /// In fact you should not do this. If the resolved object offers a RequestDestrution method 
        /// you can use that to request that it be disposed later at an appropriate time.
        /// </summary>
        /// <typeparam name="T">The type of the object to resolve.</typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// Resolve an instance of T, but first get an instance of TConfig that can be filled out.
        /// </summary>
        /// <typeparam name="T">The type to instantiate.</typeparam>
        /// <typeparam name="TConfig">The config for the type to create.</typeparam>
        /// <param name="configure">An action method to configure the new instance's config object.</param>
        /// <returns></returns>
        T Resolve<T, TConfig>(Action<TConfig> configure);

        /// <summary>
        /// Flush any objects that need destruction. It should be called each frame, or on a regular enough
        /// basis that the objects are cleaned up. Otherwise any objects that request destruction are not
        /// actually destroyed.
        /// </summary>
        void Flush();
    }
}