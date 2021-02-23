using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    /// <summary>
    /// A proxy for disposables so you can change the initial value set in a using statement.
    /// This still guards against multiple sets, which will throw an exception. Useful in coroutines
    /// and other async situations where using statements would otherwise not be possible.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UsingProxy<T> : IDisposable
        where T : IDisposable
    {
        private T wrapped;

        public T Value
        {
            set
            {
                if(this.wrapped != null)
                {
                    throw new InvalidOperationException("Can only set a using proxy value one time.");
                }
                this.wrapped = value;
            }
            get
            {
                return this.wrapped;
            }
        }

        public void Dispose()
        {
            wrapped?.Dispose();
        }
    }
}
