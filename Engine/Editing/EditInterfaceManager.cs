﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace Engine.Editing
{
    /// <summary>
    /// This class will maintain a binding between an EditInterface and the
    /// object it was created from.
    /// </summary>
    /// <typeparam name="T">The class this manager binds to.</typeparam>
    [DoNotCopy]
    [DoNotSave]
    public class EditInterfaceManager<T>
        where T : class
    {
        /// <summary>
        /// This delegate creates an EditInterface for an item in a EditInterfaceManager.
        /// </summary>
        /// <param name="item">The item to create an EditInterface for.</param>
        /// <returns>The EditInterface for item.</returns>
        public delegate EditInterface CreateEditInterfaceDelegate(T item);

        private CreateEditInterfaceDelegate createEditInterface;
        private Dictionary<T, EditInterface> interfaceDictionary = new Dictionary<T, EditInterface>();
        private LinkedList<EditInterfaceCommand> commandList = new LinkedList<EditInterfaceCommand>();
        private EditInterface editInterface;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="editInterface">The EditInterface to manage subinterfaces for.</param>
        internal EditInterfaceManager(EditInterface editInterface)
        {
            this.editInterface = editInterface;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="editInterface">The EditInterface to manage subinterfaces for.</param>
        /// <param name="createEditInterface">The function to call to create EditInterfaces for T types.</param>
        /// <param name="items">The initial list of items, can be null.</param>
        internal EditInterfaceManager(EditInterface editInterface, CreateEditInterfaceDelegate createEditInterface, IEnumerable<T> items)
            :this(editInterface)
        {
            this.createEditInterface = createEditInterface;
            if (items != null)
            {
                foreach (var item in items)
                {
                    addItem(item);
                }
            }
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
            foreach (EditInterfaceCommand command in commandList)
            {
                subInterface.addCommand(command);
            }
        }

        /// <summary>
        /// Remove the subinterface bound to source.
        /// </summary>
        /// <param name="source">The class that provided the interface to remove.</param>
        public void removeSubInterface(T source)
        {
            EditInterface subInterface;
            if (interfaceDictionary.TryGetValue(source, out subInterface))
            {
                editInterface.removeSubInterface(subInterface);
                interfaceDictionary.Remove(source);
            }
        }

        /// <summary>
        /// Remove all SubInterfaces controlled by this manager.
        /// </summary>
        public void clearSubInterfaces()
        {
            foreach (EditInterface subInterface in interfaceDictionary.Values)
            {
                editInterface.removeSubInterface(subInterface);
            }
            interfaceDictionary.Clear();
        }

        /// <summary>
        /// Find the source object that was used to create the given EditInterface.
        /// </summary>
        /// <param name="subInterface">The EditInterface to discover the creator of.</param>
        /// <returns>The creator of subInterface according to this binding.</returns>
        public T resolveSourceObject(EditInterface subInterface)
        {
            return subInterface.ManagerBinding as T;
        }

        /// <summary>
        /// Determine if an EditInterface exists for source.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns>True if an EditInterface exists.</returns>
        public bool hasEditInterface(T source)
        {
            return interfaceDictionary.ContainsKey(source);
        }

        /// <summary>
        /// Get the EditInterface for the specified source.
        /// </summary>
        /// <param name="source">The class that provided the interface to find.</param>
        /// <returns>The matching EditInterface.</returns>
        public EditInterface getEditInterface(T source)
        {
            return interfaceDictionary[source];
        }

        /// <summary>
        /// Add a command that will be added to every sub interface added to
        /// this manager. This must be done before any sub interfaces are added
        /// or else they will not have the command. This will not add commands
        /// to any existing interfaces.
        /// </summary>
        /// <param name="command"></param>
        public void addCommand(EditInterfaceCommand command)
        {
            if (!commandList.Contains(command))
            {
                commandList.AddLast(command);
            }
        }

        internal void addItem(T item)
        {
            addSubInterface(item, createEditInterface(item));
        }
    }
}
