using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine;

namespace Engine
{
    /// <summary>
    /// A SimObject is a mediator between various SimComponent instances. This
    /// allows a SimObject to represent any kind of object in a 3d scene
    /// utilizing whatever subsystems are needed for it to do work. For example
    /// it could be composed of a mesh from a renderer and a rigid body from a
    /// physics engine allowing it to move and be rendered. The updates will be
    /// fired appropriatly between these subsystems to keep everything in sync.
    /// </summary>
    /// <seealso cref="T:System.IDisposable"/>
    public class SimObject : IDisposable
    {
        private String name;
        private bool enabled = false;

        private Vector3 translation = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);

        Dictionary<String, SimComponent> components = new Dictionary<String, SimComponent>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of this sim object.</param>
        public SimObject(String name)
        {
            this.name = name;
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            foreach (SimComponent component in components.Values)
            {
                component.Dispose();
            }
        }

        /// <summary>
        /// Add a SimComponent to this SimObject.
        /// </summary>
        /// <param name="component">The component to add.</param>
        public void addComponent(SimComponent component)
        {
            components.Add(component.Name, component);
        }

        /// <summary>
        /// Remove a SimComponent from this SimObject.
        /// </summary>
        /// <param name="component">The component to remove.</param>
        public void removeComponent(SimComponent component)
        {
            if (components.ContainsKey(component.Name))
            {
                components.Remove(component.Name);
            }
        }

        /// <summary>
        /// Get the SimComponent specified by name. If the component is not
        /// found or the type does not match T null will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve.</typeparam>
        /// <param name="name">The name of the component to retrieve.</param>
        /// <returns>The component or null if it was not found or was the wrong type.</returns>
        public T getSimComponent<T>(String name)
            where T : SimComponent
        {
            if (components.ContainsKey(name))
            {
                return components[name] as T;
            }
            return null;
        }

        /// <summary>
        /// Update the position of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public void updatePosition(ref Vector3 translation, ref Quaternion rotation, SimComponent trigger)
        {
            foreach (SimComponent component in components.Values)
            {
                if (component != trigger && (component.Subscription | Subscription.PositionUpdate) != 0)
                {
                    component.updatePosition(ref translation, ref rotation);
                }
            }
        }

        /// <summary>
        /// Update the translation of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public void updateTranslation(ref Vector3 translation, SimComponent trigger)
        {
            foreach (SimComponent component in components.Values)
            {
                if (component != trigger && (component.Subscription | Subscription.PositionUpdate) != 0)
                {
                    component.updateTranslation(ref translation);
                }
            }
        }

        /// <summary>
        /// Update the rotation of the SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public void updateRotation(ref Quaternion rotation, SimComponent trigger)
        {
            foreach (SimComponent component in components.Values)
            {
                if (component != trigger && (component.Subscription | Subscription.PositionUpdate) != 0)
                {
                    component.updateRotation(ref rotation);
                }
            }
        }

        /// <summary>
        /// Update the scale of the SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public void updateScale(ref Vector3 scale, SimComponent trigger)
        {
            foreach (SimComponent component in components.Values)
            {
                if (component != trigger && (component.Subscription | Subscription.PositionUpdate) != 0)
                {
                    component.updateScale(ref scale);
                }
            }
        }

        /// <summary>
        /// Set the SimObject as enabled or disabled. The subsystems will
        /// determine the exact status that that their objects will go into when
        /// this is activated. However, this mode can be changed as quickly as
        /// possible.
        /// </summary>
        /// <param name="enabled">True to enable the SimObject, false to disable it.</param>
        public void setEnabled(bool enabled)
        {
            this.enabled = enabled;
            foreach (SimComponent component in components.Values)
            {
                component.setEnabled(enabled);
            }
        }

        /// <summary>
        /// Get the name of this SimObject.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Get the enabled status of this SimObject.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
        }

        /// <summary>
        /// Get the translation of this SimObject.
        /// </summary>
        public Vector3 Translation
        {
            get
            {
                return translation;
            }
        }

        /// <summary>
        /// Get the rotation of the SimObject.
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
        }

        /// <summary>
        /// Get the scale of the SimObject.
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return scale;
            }
        }
    }
}
