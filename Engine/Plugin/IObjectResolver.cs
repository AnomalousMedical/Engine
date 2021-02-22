using System;

namespace Engine
{
    public interface IObjectResolver : IDisposable
    {
        T Resolve<T>();
    }
}