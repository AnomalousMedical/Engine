using System;
using Anomalous.GuiFramework;
using MyGUIPlugin;
using SoundPlugin;

namespace Anomalous.Minimus.Full.GUI
{
	public class TestWindow : MDIDialog
	{
		public TestWindow ()
            : base("Anomalous.Minimus.Full.GUI.TestWindow.TestWindow.layout")
		{
            Button button = window.findWidget("PlayTestSound") as Button;
            button.MouseButtonClick += button_MouseButtonClick;

            Button colorButton = window.findWidget("ColorButton") as Button;
            colorButton.MouseButtonClick += colorButton_MouseButtonClick;
		}

        void colorButton_MouseButtonClick(Widget source, EventArgs e)
        {
            ColorMenu.ShowColorMenu(this.Left, this.Top, (c) => { });
        }

        void button_MouseButtonClick(Widget source, EventArgs e)
        {
            SoundPlugin.SoundPluginInterface.Instance.SoundManager.streamPlayAndForgetSound(GetType().Assembly.GetManifestResourceStream("Anomalous.Minimus.Full.GUI.TestWindow.PiperVb.ogg"));
        }
	}
}

