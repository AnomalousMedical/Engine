using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ObjectManagement
{
    public class GenericSimObject : SimObjectBase
    {
        private String name;
        private bool enabled = false;
        private Vector3 translation = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 scale = Vector3.ScaleIdentity;
        private List<SimElement> elements = new List<SimElement>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of this sim object.</param>
        public GenericSimObject(String name, Vector3 translation, Quaternion rotation, Vector3 scale, bool enabled)
        {
            this.name = name;
            this.translation = translation;
            this.rotation = rotation;
            this.scale = scale;
            this.enabled = enabled;
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public override void Dispose()
        {
            foreach (SimElement element in elements)
            {
                element.DoDispose();
            }
        }

        /// <summary>
        /// Add a SimElement to this SimObject.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public override void addElement(SimElement element)
        {
            elements.Add(element);
            element.setSimObject(this);
        }

        /// <summary>
        /// Remove a SimElement from this SimObject.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public void removeElement(SimElement element)
        {
            if (elements.Contains(element))
            {
                elements.Remove(element);
                element.setSimObject(null);
            }
        }

        /// <summary>
        /// Get the SimElement specified by name. If the element is not found or
        /// the type does not match T null will be returned. Warning, this
        /// method will traverse the list of all elements to find the match so
        /// it can be very slow.
        /// </summary>
        /// <typeparam name="T">The type of the element to retrieve.</typeparam>
        /// <param name="name">The name of the element to retrieve.</param>
        /// <returns>The element or null if it was not found or was the wrong type.</returns>
        public T getSimElement<T>(String name)
            where T : SimElement
        {
            foreach (SimElement element in elements)
            {
                if (element.Name == name)
                {
                    return element as T;
                }
            }
            return null;
        }

        /// <summary>
        /// Update the position of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public override void updatePosition(ref Vector3 translation, ref Quaternion rotation, SimElement trigger)
        {
            this.translation = translation;
            this.rotation = rotation;
            foreach (SimElement element in elements)
            {
                if (element != trigger && (element.Subscription | Subscription.PositionUpdate) != 0)
                {
                    element.alertUpdatePosition(ref translation, ref rotation);
                }
            }
        }

        /// <summary>
        /// Update the translation of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public override void updateTranslation(ref Vector3 translation, SimElement trigger)
        {
            this.translation = translation;
            foreach (SimElement element in elements)
            {
                if (element != trigger && (element.Subscription | Subscription.PositionUpdate) != 0)
                {
                    element.alertUpdateTranslation(ref translation);
                }
            }
        }

        /// <summary>
        /// Update the rotation of the SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public override void updateRotation(ref Quaternion rotation, SimElement trigger)
        {
            this.rotation = rotation;
            foreach (SimElement element in elements)
            {
                if (element != trigger && (element.Subscription | Subscription.PositionUpdate) != 0)
                {
                    element.alertUpdateRotation(ref rotation);
                }
            }
        }

        /// <summary>
        /// Update the scale of the SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public override void updateScale(ref Vector3 scale, SimElement trigger)
        {
            this.scale = scale;
            foreach (SimElement element in elements)
            {
                if (element != trigger && (element.Subscription | Subscription.ScaleUpdate) != 0)
                {
                    element.alertUpdateScale(ref scale);
                }
            }
        }

        /// <summary>
        /// Save this SimObject to a SimObjectDefinition.
        /// </summary>
        /// <param name="definitionName">The name to give the SimObjectDefinition.</param>
        /// <returns>A new SimObjectDefinition for this SimObject.</returns>
        public override SimObjectDefinition saveToDefinition(String definitionName)
        {
            GenericSimObjectDefinition definition = new GenericSimObjectDefinition(definitionName);
            definition.Enabled = Enabled;
            definition.Rotation = Rotation;
            definition.Scale = Scale;
            definition.Translation = Translation;
            foreach (SimElement element in elements)
            {
                definition.addElement(element.saveToDefinition());
            }
            return definition;
        }

        /// <summary>
        /// Get a particular SimElement from the SimObject. This will return
        /// null if the element cannot be found. This method could potentially
        /// be fairly slow so it is best to cache the values returned from this
        /// function somehow.
        /// </summary>
        /// <param name="name">The name of the SimElement to retrieve.</param>
        /// <returns>The SimElement specified by name or null if it cannot be found.</returns>
        public override SimElement getElement(String name)
        {
            foreach (SimElement element in elements)
            {
                if (element.Name == name)
                {
                    return element;
                }
            }
            return null;
        }

        /// <summary>
        /// Get an iterator over all sim elements.
        /// </summary>
        /// <returns>An IEnumerable over all sim elements.</returns>
        public override IEnumerable<SimElement> getElementIter()
        {
            return elements;
        }

        /// <summary>
        /// Get the name of this SimObject.
        /// </summary>
        public override String Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Get the enabled status of this SimObject.
        /// </summary>
        public override bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                this.enabled = value;
                foreach (SimElement element in elements)
                {
                    element.fireSetEnabled(enabled);
                }
            }
        }

        /// <summary>
        /// Get the translation of this SimObject.
        /// </summary>
        public override Vector3 Translation
        {
            get
            {
                return translation;
            }
        }

        /// <summary>
        /// Get the rotation of the SimObject.
        /// </summary>
        public override Quaternion Rotation
        {
            get
            {
                return rotation;
            }
        }

        /// <summary>
        /// Get the scale of the SimObject.
        /// </summary>
        public override Vector3 Scale
        {
            get
            {
                return scale;
            }
        }
    }
}
