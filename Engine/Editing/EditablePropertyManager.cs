using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    /// <summary>
    /// This class makes it easier to manage EditableProperties for a base type.
    /// You can add properties with the real object as a key and the EditableProperty as the value
    /// and then easily remove them later.
    /// </summary>
    class EditablePropertyManager
    {
        private EditInterface editInterface;
        private Dictionary<Object, EditableProperty> properties = new Dictionary<Object, EditableProperty>();
        private Dictionary<EditableProperty, Object> propertiesToObjects = new Dictionary<EditableProperty, object>();

        public EditablePropertyManager(EditInterface editInterface)
        {
            this.editInterface = editInterface;
        }

        public void addProperty(Object key, EditableProperty property)
        {
            properties.Add(key, property);
            propertiesToObjects.Add(property, key);
            editInterface.addEditableProperty(property);
        }

        public void removeProperty(Object key)
        {
            var property = properties[key];
            properties.Remove(key);
            propertiesToObjects.Remove(property);
            editInterface.removeEditableProperty(property);
        }

        public Object this[EditableProperty property]
        {
            get
            {
                return propertiesToObjects[property];
            }
        }
    }
}
