namespace Engine.Platform.Input
{
    /// <summary>
    /// Inject the object to use as the event layer key.
    /// T is the target class that this will be injected into not the key type.
    /// </summary>
    public interface IEventLayerKeyInjector<T>
    {
        /// <summary>
        /// The key.
        /// </summary>
        object Key { get; }
    }
}