using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Platform.Input
{
    /// <summary>
    /// Default implementation of IEventLayerKeyInjector
    /// </summary>
    public class EventLayerKeyInjector<T> : IEventLayerKeyInjector<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key"></param>
        public EventLayerKeyInjector(Object key = null)
        {
            this.Key = key;
        }

        /// <summary>
        /// The key.
        /// </summary>
        public Object Key { get; set; }
    }
}
