using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OgreWrapper;
using Engine;
using System.Runtime.InteropServices;
using Logging;

namespace OgreModelEditor
{
    public partial class CustomParameterControl : DockContent
    {
        private Entity entity;

        public CustomParameterControl()
        {
            InitializeComponent();
        }

        public SubEntity SubEntity
        {
            get
            {
                if (entity != null)
                {
                    return entity.getSubEntity(0);
                }
                return null;
            }
        }

        public Entity Entity
        {
            get
            {
                return entity;
            }
            set
            {
                entity = value;
                using (MeshPtr mesh = entity.getMesh())
                {
                    materialText.Text = mesh.Value.getSubMesh(0).getMaterialName();
                    Log.Debug("Model vertex buffer usage is {0}", mesh.Value.getVertexBufferUsage());
                    Log.Debug("Model index buffer usage is {0}", mesh.Value.getIndexBufferUsage());
                }
            }
        }

        private void setValue_Click(object sender, EventArgs e)
        {
            if (SubEntity != null)
            {
                Quaternion value = new Quaternion();
                if (value.setValue(valueText.Text))
                {
                    SubEntity.setCustomParameter((int)indexUpDown.Value, value);
                }
                else
                {
                    MessageBox.Show(this, "Could not interpret the value please enter 4 numbers separated by commas.");
                }
            }
        }

        private void getValue_Click(object sender, EventArgs e)
        {
            try
            {
                if (SubEntity != null)
                {
                    Quaternion value = SubEntity.getCustomParameter((int)indexUpDown.Value);
                    valueText.Text = value.ToString();
                }
            }
            catch (SEHException)
            {
                valueText.Text = "0, 0, 0, 0";
            }
        }

        private void applyMaterialButton_Click(object sender, EventArgs e)
        {
            using (MeshPtr mesh = entity.getMesh())
            {
                mesh.Value.getSubMesh(0).setMaterialName(materialText.Text);
            }
        }
    }
}
