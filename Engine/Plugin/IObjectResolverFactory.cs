namespace Engine
{
    public interface IObjectResolverFactory
    {
        /// <summary>
        /// Create a new IObjectResolver. The caller must dispose the instance.
        /// </summary>
        /// <returns></returns>
        IObjectResolver Create();
    }
}