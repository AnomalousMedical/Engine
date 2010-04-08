using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Editor
{
    public delegate void ObjectEditorGUIEvent(EditInterface editInterface, object editingObject);

    /// <summary>
    /// This interface abstracts what gui is being used to edit an object.
    /// </summary>
    public interface IObjectEditorGUI
    {
        /// <summary>
        /// Set the EditInterface this gui is editing.
        /// </summary>
        /// <param name="editInterface">The EditInterface to edit.</param>
        /// <param name="editingObject">The object being edited. Can be null it is only passed back to the callback.</param>
        /// <param name="CommitObjectChangesCallback">The function to call when the object has been changed. Null to call nothing.</param>
        void setEditInterface(EditInterface editInterface, object editingObject, ObjectEditorGUIEvent CommitObjectChangesCallback);

        /// <summary>
        /// Clear any EditInterface that is shown.
        /// </summary>
        void clearEditInterface();
    }
}
