using Anomalous.GuiFramework;
using Engine;
using Logging;
using MyGUIPlugin;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgreModelEditor
{
    class CustomParametersWindow : MDIDialog
    {
        private Entity entity;
        private NumericEdit index;
        private EditBox value;
        private EditBox material;

        public CustomParametersWindow()
            : base("OgreModelEditor.GUI.CustomParametersWindow.CustomParametersWindow.layout")
        {
            index = new NumericEdit(window.findWidget("Index") as EditBox);
            index.IntValue = 0;
            index.AllowFloat = false;

            value = window.findWidget("Value") as EditBox;
            material = window.findWidget("Material") as EditBox;

            Button setValue = window.findWidget("SetValue") as Button;
            setValue.MouseButtonClick += setValue_MouseButtonClick;

            Button getValue = window.findWidget("GetValue") as Button;
            getValue.MouseButtonClick += getValue_MouseButtonClick;

            Button applyMaterial = window.findWidget("ApplyMaterial") as Button;
            applyMaterial.MouseButtonClick += applyMaterial_MouseButtonClick;
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
                    material.OnlyText = mesh.Value.getSubMesh(0).getMaterialName();
                    Log.Debug("Model vertex buffer usage is {0}", mesh.Value.getVertexBufferUsage());
                    Log.Debug("Model index buffer usage is {0}", mesh.Value.getIndexBufferUsage());
                    Log.Debug("Entity isHardwareAnimationEnabled {0}", entity.isHardwareAnimationEnabled());
                }
            }
        }

        void setValue_MouseButtonClick(Widget source, EventArgs e)
        {
            if (SubEntity != null)
            {
                Quaternion value = new Quaternion();
                if (value.setValue(this.value.OnlyText))
                {
                    SubEntity.setCustomParameter(index.IntValue, value);
                }
                else
                {
                    MessageBox.show("Could not interpret the value please enter 4 numbers separated by commas.", "Error", MessageBoxStyle.IconError | MessageBoxStyle.Ok);
                }
            }
        }

        void getValue_MouseButtonClick(Widget source, EventArgs e)
        {
            try
            {
                if (SubEntity != null)
                {
                    Quaternion value = SubEntity.getCustomParameter(index.IntValue);
                    this.value.OnlyText = value.ToString();
                }
            }
            catch (OgreException)
            {
                this.value.OnlyText = "";
            }
        }

        void applyMaterial_MouseButtonClick(Widget source, EventArgs e)
        {
            using (MeshPtr mesh = entity.getMesh())
            {
                mesh.Value.getSubMesh(0).setMaterialName(material.OnlyText);
            }
        }
    }
}
