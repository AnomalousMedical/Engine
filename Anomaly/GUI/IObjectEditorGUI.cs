using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Anomaly
{
    public delegate void ObjectEditorGUIEvent(EditInterface editInterface, object editingObject);

    /// <summary>
    /// This interface abstracts what gui is being used to edit an object.
    /// </summary>
    public interface IObjectEditorGUI
    {
        /// <summary>
        /// Called when the main object in this ui has changed.
        /// </summary>
        event ObjectEditorGUIEvent MainInterfaceChanged;

        /// <summary>
        /// Called when the active interface being edited has changed.
        /// </summary>
        event ObjectEditorGUIEvent ActiveInterfaceChanged;

        /// <summary>
        /// Called when a field has been changed.
        /// </summary>
        event ObjectEditorGUIEvent FieldChanged;

        /// <summary>
        /// Set the EditInterface this gui is editing.
        /// </summary>
        /// <param name="editInterface">The EditInterface to edit.</param>
        /// <param name="editingObject">The object being edited. Can be null it is only passed back to the callback.</param>
        /// <param name="FieldChangedCallback">Callback for when a field is changed.</param>
        /// <param name="EditingCompletedCallback">Callback for when editing is complted.</param>
        void setEditInterface(EditInterface editInterface, object editingObject, ObjectEditorGUIEvent FieldChangedCallback);

        /// <summary>
        /// Clear any EditInterface that is shown.
        /// </summary>
        void clearEditInterface();
    }
}
