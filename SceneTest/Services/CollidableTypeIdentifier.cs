using BepuPhysics.Collidables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class CollidableTypeIdentifier : ICollidableTypeIdentifier
    {
        private Dictionary<CollidableReference, Object> identifiers = new Dictionary<CollidableReference, object>();

        public void AddIdentifier<T>(CollidableReference collidable, T reference)
        {
            identifiers.Add(collidable, reference);
        }

        public void RemoveIdentifier(CollidableReference collidable)
        {
            identifiers.Remove(collidable);
        }

        public bool TryGetIdentifier(CollidableReference collidable, out Object value)
        {
            return TryGetIdentifier<Object>(collidable, out value);
        }

        public bool TryGetIdentifier<T>(CollidableReference collidable, out T value)
        {
            identifiers.TryGetValue(collidable, out var obj);
            if (obj is T)
            {
                value = (T)obj;
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
