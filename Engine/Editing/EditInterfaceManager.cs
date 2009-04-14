using System;
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

        public EditInterfaceManager(EditInterface editInterface)
        {
            this.editInterface = editInterface;
        }

        public void addSubInterface(T source, EditInterface subInterface)
        {
            editInterface.addSubInterface(subInterface);
            subInterface.ManagerBinding = source;
            interfaceDictionary.Add(source, subInterface);
        }

        public void removeSubInterface(T source)
        {
            EditInterface subInterface = interfaceDictionary[source];
            editInterface.removeSubInterface(subInterface);
            interfaceDictionary.Remove(source);
        }

        public T resolveSourceObject(EditInterface subInterface)
        {
            return (T)subInterface.ManagerBinding;
        }
    }
}
