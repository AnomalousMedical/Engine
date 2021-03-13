namespace Engine
{
    public interface IObjectResolverFactory
    {
        /// <summary>
        /// Create a new IObjectResolver. The caller must dispose the instance.
        /// </summary>
        /// <returns></returns>
        IObjectResolver Create();

        /// <summary>
        /// Flush any objects that need destruction. It should be called each frame, or on a regular enough
        /// basis that the objects are cleaned up. Otherwise any objects that request destruction are not
        /// actually destroyed. This will flush objects across all object resolvers this factory created.
        /// </summary>
        void Flush();
    }
}