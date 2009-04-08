using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;

namespace Editor
{
    public partial class ObjectEditorPanel : UserControl
    {
        #region Constructors

        public ObjectEditorPanel()
        {
            InitializeComponent();
            editInterfaceView.OnEditInterfaceSelectionChanged += new EditInterfaceSelectionChanged(propertiesTable.showEditableProperties);
        }

        #endregion Constructors

        #region Functions

        public void setEditInterface(EditInterface editor)
        {
            editInterfaceView.setEditInterface(editor);
            propertiesTable.showEditableProperties(editor);
        }

        #endregion Functions

        
    }
}
