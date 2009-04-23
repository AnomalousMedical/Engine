using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;

namespace Engine.ObjectManagement
{
    public class GenericSimObject : SimObject
    {
        #region Fields

        private String name;
        private bool enabled = false;
        private Vector3 translation = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 scale = Vector3.ScaleIdentity;
        private List<SimElement> elements = new List<SimElement>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of this sim object.</param>
        public GenericSimObject(String name)
        {
            this.name = name;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            foreach (SimElement element in elements)
            {
                element.Dispose();
            }
        }

        /// <summary>
        /// Add a SimElement to this SimObject.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public void addElement(SimElement element)
        {
            elements.Add(element);
            element.SimObject = this;
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
                element.SimObject = null;
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
        public void updatePosition(ref Vector3 translation, ref Quaternion rotation, SimElement trigger)
        {
            this.translation = translation;
            this.rotation = rotation;
            foreach (SimElement element in elements)
            {
                if (element != trigger && (element.Subscription | Subscription.PositionUpdate) != 0)
                {
                    element.updatePosition(ref translation, ref rotation);
                }
            }
        }

        /// <summary>
        /// Update the translation of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public void updateTranslation(ref Vector3 translation, SimElement trigger)
        {
            this.translation = translation;
            foreach (SimElement element in elements)
            {
                if (element != trigger && (element.Subscription | Subscription.PositionUpdate) != 0)
                {
                    element.updateTranslation(ref translation);
                }
            }
        }

        /// <summary>
        /// Update the rotation of the SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public void updateRotation(ref Quaternion rotation, SimElement trigger)
        {
            this.rotation = rotation;
            foreach (SimElement element in elements)
            {
                if (element != trigger && (element.Subscription | Subscription.PositionUpdate) != 0)
                {
                    element.updateRotation(ref rotation);
                }
            }
        }

        /// <summary>
        /// Update the scale of the SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public void updateScale(ref Vector3 scale, SimElement trigger)
        {
            this.scale = scale;
            foreach (SimElement element in elements)
            {
                if (element != trigger && (element.Subscription | Subscription.ScaleUpdate) != 0)
                {
                    element.updateScale(ref scale);
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
            foreach (SimElement element in elements)
            {
                element.setEnabled(enabled);
            }
        }

        /// <summary>
        /// Save this SimObject to a SimObjectDefinition.
        /// </summary>
        /// <param name="definitionName">The name to give the SimObjectDefinition.</param>
        /// <returns>A new SimObjectDefinition for this SimObject.</returns>
        public SimObjectDefinition saveToDefinition(String definitionName)
        {
            GenericSimObjectDefinition definition = new GenericSimObjectDefinition(definitionName);
            foreach (SimElement element in elements)
            {
                definition.addElement(element.saveToDefinition());
            }
            return definition;
        }

        /// <summary>
        /// Save the instance specific information for this SimObject.
        /// </summary>
        /// <returns>A new SimObjectInstanceDefinition.</returns>
        public SimObjectInstanceDefinition saveInstanceDefinition()
        {
            GenericSimObjectInstanceDefinition instance = new GenericSimObjectInstanceDefinition(Name);
            instance.Enabled = Enabled;
            instance.Rotation = Rotation;
            instance.Scale = Scale;
            instance.Translation = Translation;
            return instance;
        }

        #endregion Functions

        #region Properties

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

        #endregion Properites
    }
}
