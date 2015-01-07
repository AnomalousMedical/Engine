﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Engine.Editing;
using Anomalous.GuiFramework.Editor;

namespace Editor
{
    public partial class VerticalObjectEditor : DockContent, IObjectEditorGUI
    {
        /// <summary>
        /// Called when the main object in this ui has changed.
        /// </summary>
        public event ObjectEditorGUIEvent MainInterfaceChanged;

        /// <summary>
        /// Called when the active interface has changed.
        /// </summary>
        public event ObjectEditorGUIEvent ActiveInterfaceChanged;

        /// <summary>
        /// Called when a field has been changed.
        /// </summary>
        public event ObjectEditorGUIEvent FieldChanged;

        private object currentEditingObject;
        private EditInterface currentEditInterface;
        private ObjectEditorGUIEvent currentFieldChangedCallback;

        public VerticalObjectEditor()
        {
            InitializeComponent();
            editInterfaceView.OnEditInterfaceSelectionChanging += editInterfaceView_OnEditInterfaceSelectionChanging;
            editInterfaceView.OnEditInterfaceSelectionChanged += editInterfaceView_OnEditInterfaceSelectionChanged;
            editInterfaceView.OnEditInterfaceAdded += editInterfaceView_OnEditInterfaceAdded;
            editInterfaceView.OnEditInterfaceRemoved += editInterfaceView_OnEditInterfaceRemoved;
            propertiesTable.EditablePropertyValueChanged += new EditablePropertyValueChanged(propertiesTable_EditablePropertyValueChanged);
            propertiesTable.EditablePropertyAdded += propertiesTable_EditablePropertyAdded;
            propertiesTable.EditablePropertyRemoved += propertiesTable_EditablePropertyRemoved;
        }

        /// <summary>
        /// Set the current EditInterface shown on this panel.
        /// </summary>
        /// <param name="editor">The editor to show.</param>
        public void setEditInterface(EditInterface editInterface, object editingObject, ObjectEditorGUIEvent FieldChangedCallback)
        {
            editInterfaceView.clearEditInterface();
            editInterfaceView.setEditInterface(editInterface);
            propertiesTable.showEditableProperties(editInterface);
            this.Text = editInterface.getName();

            currentEditInterface = editInterface;
            currentEditingObject = editingObject;
            currentFieldChangedCallback = FieldChangedCallback;

            if (MainInterfaceChanged != null)
            {
                MainInterfaceChanged.Invoke(editInterface, editingObject);
            }

            if (ActiveInterfaceChanged != null)
            {
                ActiveInterfaceChanged.Invoke(editInterface, editingObject);
            }
        }

        public void clearEditInterface()
        {
            currentEditInterface = null;
            currentEditingObject = null;
            currentFieldChangedCallback = null;

            editInterfaceView.clearEditInterface();
            propertiesTable.showEditableProperties(null);
            this.Text = "Properties";

            if (MainInterfaceChanged != null)
            {
                MainInterfaceChanged.Invoke(null, null);
            }
        }

        public bool AutoExpand
        {
            get
            {
                return editInterfaceView.AutoExpand;
            }
            set
            {
                editInterfaceView.AutoExpand = value;
            }
        }

        public EditInterface SelectedEditInterface
        {
            get
            {
                return editInterfaceView.getSelectedEditInterface();
            }
        }

        /// <summary>
        /// Callback for when the EditInterface changes.
        /// </summary>
        /// <param name="evt"></param>
        void editInterfaceView_OnEditInterfaceSelectionChanged(EditInterfaceViewEventArgs evt)
        {
            propertiesTable.showEditableProperties(evt.EditInterface);
            if (ActiveInterfaceChanged != null)
            {
                ActiveInterfaceChanged.Invoke(evt.EditInterface, null);
            }
        }

        /// <summary>
        /// Callback for when the EditInterface is about to change.
        /// </summary>
        /// <param name="evt"></param>
        void editInterfaceView_OnEditInterfaceSelectionChanging(EditInterfaceViewEventArgs evt)
        {
            String error;
            if (!propertiesTable.validateCurrentSettings(out error))
            {
                evt.Cancel = true;
                MessageBox.Show(this, error, "Invalid Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void propertiesTable_EditablePropertyValueChanged(EditableProperty var)
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(currentEditInterface, currentEditingObject);
            }
            if (FieldChanged != null)
            {
                FieldChanged.Invoke(propertiesTable.getCurrentEditInterface(), null);
            }
        }

        void propertiesTable_EditablePropertyRemoved()
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(currentEditInterface, currentEditingObject);
            }
            if (FieldChanged != null)
            {
                FieldChanged.Invoke(propertiesTable.getCurrentEditInterface(), null);
            }
        }

        void propertiesTable_EditablePropertyAdded()
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(currentEditInterface, currentEditingObject);
            }
            if (FieldChanged != null)
            {
                FieldChanged.Invoke(propertiesTable.getCurrentEditInterface(), null);
            }
        }

        void editInterfaceView_OnEditInterfaceRemoved(EditInterfaceViewEventArgs evt)
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(currentEditInterface, currentEditingObject);
            }
        }

        void editInterfaceView_OnEditInterfaceAdded(EditInterfaceViewEventArgs evt)
        {
            if (currentFieldChangedCallback != null)
            {
                currentFieldChangedCallback.Invoke(currentEditInterface, currentEditingObject);
            }
        }
    }
}
