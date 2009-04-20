﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    /// <summary>
    /// This class will maintain a binding between an EditInterface and the
    /// object it was created from.
    /// </summary>
    /// <typeparam name="T">The class this manager binds to.</typeparam>
    public class EditInterfaceManager<T>
    {
        private Dictionary<T, EditInterface> interfaceDictionary = new Dictionary<T, EditInterface>();
        private EditInterface editInterface;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="editInterface">The EditInterface to manage subinterfaces for.</param>
        public EditInterfaceManager(EditInterface editInterface)
        {
            this.editInterface = editInterface;
        }

        /// <summary>
        /// Add a subinterface bound to source.
        /// </summary>
        /// <param name="source">The class that provided the subinterface.</param>
        /// <param name="subInterface">The subinterface to bind to source.</param>
        public void addSubInterface(T source, EditInterface subInterface)
        {
            editInterface.addSubInterface(subInterface);
            subInterface.ManagerBinding = source;
            interfaceDictionary.Add(source, subInterface);
        }

        /// <summary>
        /// Remove the subinterface bound to source.
        /// </summary>
        /// <param name="source">The class that provided the interface to remove.</param>
        public void removeSubInterface(T source)
        {
            EditInterface subInterface = interfaceDictionary[source];
            editInterface.removeSubInterface(subInterface);
            interfaceDictionary.Remove(source);
        }

        /// <summary>
        /// Find the source object that was used to create the given EditInterface.
        /// </summary>
        /// <param name="subInterface">The EditInterface to discover the creator of.</param>
        /// <returns>The creator of subInterface according to this binding.</returns>
        public T resolveSourceObject(EditInterface subInterface)
        {
            return (T)subInterface.ManagerBinding;
        }
    }
}
