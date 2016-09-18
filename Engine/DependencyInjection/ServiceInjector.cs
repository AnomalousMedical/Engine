using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class ServiceInjector
    {
        private Dictionary<Type, Object> singletons = new Dictionary<Type, object>();
        private Dictionary<Type, Action> singletonCreators = new Dictionary<Type, Action>();

        public void addSingleton<T>(Func<T> creator)
        {
            var type = typeof(T);
            singletonCreators.Add(type, () =>
            {
                singletons.Add(type, creator());
                singletonCreators.Remove(type);
            });
        }

        public T getService<T>()
        {
            var type = typeof(T);
            Object instance;
            if(singletons.TryGetValue(type, out instance))
            {
                return (T)instance;
            }
            Action creator;
            if(singletonCreators.TryGetValue(type, out creator))
            {
                creator();
                return (T)singletons[type];
            }
            return default(T);
        }
    }
}
