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
                    using (var tex = TextureManager.getInstance().getByName(selectedTexture))
                    {
                        int numMips = tex.Value.NumMipmaps + 1;
                        int width = (int)tex.Value.Width;
                        int height = (int)tex.Value.Height;
                        for (int mip = 0; mip < numMips; ++mip)
                        {
                            using (var buffer = tex.Value.getBuffer(0, (uint)mip))
                            {
                                using (var blitBitmap = new FreeImageAPI.FreeImageBitmap(width, height, FreeImageAPI.PixelFormat.Format32bppArgb))
                                {
                                    using (var blitBitmapBox = new PixelBox(0, 0, width, height, OgreDrawingUtility.getOgreFormat(blitBitmap.PixelFormat), blitBitmap.GetScanlinePointer(0).ToPointer()))
                                    {
                                        buffer.Value.blitToMemory(blitBitmapBox);
                                    }

                                    blitBitmap.RotateFlip(FreeImageAPI.RotateFlipType.RotateNoneFlipY);
                                    String fileName = String.Format("{0}_mip_{1}.png", selectedTexture, mip);
                                    fileName = Path.Combine(RuntimePlatformInfo.LocalUserDocumentsFolder, fileName);
                                    using (var stream = System.IO.File.Open(fileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
                                    {
                                        blitBitmap.Save(stream, FreeImageAPI.FREE_IMAGE_FORMAT.FIF_PNG);
                                    }
                                    MessageBox.show(String.Format("Saved texture to {0}", fileName), "Texture Saved", MessageBoxStyle.Ok | MessageBoxStyle.IconInfo);
                                }
                                width >>= 1;
                                height >>= 1;
                            }
                        }
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
