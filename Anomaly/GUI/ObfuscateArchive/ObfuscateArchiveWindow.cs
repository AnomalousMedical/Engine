using Anomalous.OSPlatform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomaly.GUI
{
    class ObfuscateArchiveWindow : Dialog
    {
        private const String Filter = "*.zip|*.zip|*.dat|*.dat";

        private NativeOSWindow parentWindow;

        private EditBox sourceTextBox;
        private EditBox destTextBox;

        public ObfuscateArchiveWindow(NativeOSWindow parentWindow)
            : base("Anomaly.GUI.ObfuscateArchive.ObfuscateArchiveWindow.layout")
        {
            this.parentWindow = parentWindow;

            sourceTextBox = window.findWidget("Source") as EditBox;
            destTextBox = window.findWidget("Destination") as EditBox;

            Button obfuscate = window.findWidget("Obfuscate") as Button;
            obfuscate.MouseButtonClick += obfuscate_MouseButtonClick;

            Button deobfuscate = window.findWidget("Deobfuscate") as Button;
            deobfuscate.MouseButtonClick += deobfuscate_MouseButtonClick;

            Button browseSource = window.findWidget("BrowseSource") as Button;
            browseSource.MouseButtonClick += browseSource_MouseButtonClick;

            Button browseDestination = window.findWidget("BrowseDestination") as Button;
            browseDestination.MouseButtonClick += browseDestination_MouseButtonClick;
        }

        void browseSource_MouseButtonClick(Widget source, EventArgs e)
        {
            FileOpenDialog open = new FileOpenDialog(parentWindow, wildcard: Filter, selectMultiple: false);
            open.showModal((result, path) =>
            {
                if (result == NativeDialogResult.OK)
                {
                    sourceTextBox.OnlyText = path.FirstOrDefault();
                    destTextBox.OnlyText = sourceTextBox.OnlyText.Replace(".zip", ".dat");
                }
            });
        }

        void browseDestination_MouseButtonClick(Widget source, EventArgs e)
        {
            FileOpenDialog open = new FileOpenDialog(parentWindow, wildcard: Filter, selectMultiple:false);
            open.showModal((result, path) =>
            {
                if (result == NativeDialogResult.OK)
                {
                    destTextBox.OnlyText = path.FirstOrDefault();
                }
            });
        }

        void obfuscate_MouseButtonClick(Widget source, EventArgs e)
        {
            if (sourceTextBox.OnlyText != null && destTextBox.OnlyText != null)
            {
                PublishController.obfuscateZipFile(sourceTextBox.OnlyText, destTextBox.OnlyText);
            }
        }

        void deobfuscate_MouseButtonClick(Widget source, EventArgs e)
        {
            if (sourceTextBox.OnlyText != null && destTextBox.OnlyText != null)
            {
                PublishController.deobfuscateZipFile(sourceTextBox.OnlyText, destTextBox.OnlyText);
            }
        }
    }
}
