using Anomalous.GuiFramework;
using Anomalous.OSPlatform;
using Engine;
using MyGUIPlugin;
using OgrePlugin;
using OgrePlugin.VirtualTexture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework.Debugging
{
    public class VirtualTextureDebugger : MDIDialog
    {
        ComboBox textureCombo;
        ImageBox textureImage;
        VirtualTextureManager virtualTextureManager;

        public VirtualTextureDebugger(VirtualTextureManager virtualTextureManager)
            : base("Anomalous.GuiFramework.Debugging.GUI.VirtualTextureDebugger.VirtualTextureDebugger.layout")
        {
            this.virtualTextureManager = virtualTextureManager;

            textureCombo = window.findWidget("TextureCombo") as ComboBox;
            textureCombo.EventComboAccept += textureCombo_EventComboAccept;

            textureImage = window.findWidget("TextureImage") as ImageBox;

            Button save = window.findWidget("SaveButton") as Button;
            save.MouseButtonClick += save_MouseButtonClick;

            Button reset = window.findWidget("ResetButton") as Button;
            reset.MouseButtonClick += reset_MouseButtonClick;
        }

        protected override void onShown(EventArgs args)
        {
            base.onShown(args);
            textureCombo.removeAllItems();
            foreach (String textureName in virtualTextureManager.TextureNames)
            {
                textureCombo.addItem(textureName);
            }
        }

        void textureCombo_EventComboAccept(Widget source, EventArgs e)
        {
            if (textureCombo.SelectedIndex != ComboBox.Invalid)
            {
                textureImage.setImageTexture(textureCombo.SelectedItemName);
            }
        }

        unsafe void save_MouseButtonClick(Widget source, EventArgs e)
        {
            try {
                if (textureCombo.SelectedIndex != ComboBox.Invalid)
                {
                    String selectedTexture = textureCombo.SelectedItemName;
                    String outputFolder = RuntimePlatformInfo.LocalUserDocumentsFolder;
                    using (var tex = TextureManager.getInstance().getByName(selectedTexture))
                    {
                        uint numMips = (uint)(tex.Value.NumMipmaps + 1);
                        uint width = tex.Value.Width;
                        uint height = tex.Value.Height;
                        for (uint mip = 0; mip < numMips; ++mip)
                        {
                            using (var buffer = tex.Value.getBuffer(0, mip))
                            {
                                using (var blitBitmap = new Image(width, height, 1, tex.Value.Format, 1, 0))
                                {
                                    using (var blitBitmapBox = blitBitmap.getPixelBox())
                                    {
                                        buffer.Value.blitToMemory(blitBitmapBox);
                                    }

                                    String fileName = String.Format("{0}_mip_{1}.png", selectedTexture, mip);
                                    fileName = Path.Combine(outputFolder, fileName);
                                    blitBitmap.save(fileName);
                                }
                                width >>= 1;
                                height >>= 1;
                            }
                        }
                        virtualTextureManager.saveIndirectionTexture(selectedTexture, outputFolder);
                        MessageBox.show(String.Format("Saved textures to {0}", outputFolder), "Texture Saved", MessageBoxStyle.Ok | MessageBoxStyle.IconInfo);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.show(String.Format("Error saving texture.\n{0} occured. Message: {1}", ex.GetType().Name, ex.Message), "Texture Save Error", MessageBoxStyle.Ok | MessageBoxStyle.IconError);
            }
        }

        void reset_MouseButtonClick(Widget source, EventArgs e)
        {
            virtualTextureManager.reset();
        }
    }
}
