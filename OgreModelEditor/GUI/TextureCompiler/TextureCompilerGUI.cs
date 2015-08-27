using Anomalous.OSPlatform;
using Anomalous.TextureCompiler;
using Engine;
using Engine.Threads;
using Logging;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreModelEditor
{
    class TextureCompilerGUI : Dialog
    {
        PluginManager pluginManager;
        NativeOSWindow parent;

        EditBox source;
        EditBox dest;

        public TextureCompilerGUI(PluginManager pluginManager, NativeOSWindow parent)
            :base("OgreModelEditor.GUI.TextureCompiler.TextureCompilerGUI.layout")
        {
            this.parent = parent;
            this.pluginManager = pluginManager;

            Button browseSource = window.findWidget("SourceFileBrowser") as Button;
            browseSource.MouseButtonClick += BrowseSource_MouseButtonClick;

            Button browseDest = window.findWidget("DestFileBrowser") as Button;
            browseDest.MouseButtonClick += BrowseDest_MouseButtonClick;

            Button compileButton = window.findWidget("Compile") as Button;
            compileButton.MouseButtonClick += CompileButton_MouseButtonClick;

            Button cleanButton = window.findWidget("Clean") as Button;
            cleanButton.MouseButtonClick += CleanButton_MouseButtonClick;

            source = window.findWidget("SourceFolder") as EditBox;
            dest = window.findWidget("DestFolder") as EditBox;
        }

        public void setCurrentDest(String dest)
        {
            this.dest.OnlyText = dest;
        }

        protected override void onClosing(DialogCancelEventArgs args)
        {
            args.Cancel = !window.ClientWidget.Enabled;
            base.onClosing(args);
        }

        private void BrowseDest_MouseButtonClick(Widget source, EventArgs e)
        {
            DirDialog dirDialog = new DirDialog(parent, "Select a destination directory", "");
            dirDialog.showModal((result, path) =>
            {
                if (result == NativeDialogResult.OK)
                {
                    this.dest.OnlyText = path;
                }
            });
        }

        private void BrowseSource_MouseButtonClick(Widget source, EventArgs e)
        {
            DirDialog dirDialog = new DirDialog(parent, "Select a source directory", "");
            dirDialog.showModal((result, path) =>
            {
                if (result == NativeDialogResult.OK)
                {
                    this.source.OnlyText = path;
                }
            });
        }

        private void CompileButton_MouseButtonClick(Widget source, EventArgs e)
        {
            window.ClientWidget.Enabled = false;
            ThreadManager.RunInBackground(() =>
            {
                TextureCompilerInterface.CompileTextures(this.source.OnlyText, this.dest.OnlyText, pluginManager);
                ThreadManager.invoke(() => window.ClientWidget.Enabled = true);
            });
        }

        private void CleanButton_MouseButtonClick(Widget source, EventArgs e)
        {
            try
            {
                File.Delete(Path.Combine(this.source.OnlyText, TextureCompilerInterface.TextureHashFileName));
                Log.ImportantInfo("Cleaned {0}", this.source.OnlyText);
            }
            catch(Exception ex)
            {
                Log.Error("{0} deleting {1}. Reason: {2}", ex.GetType().Name, Path.Combine(this.source.OnlyText, TextureCompilerInterface.TextureHashFileName), ex.Message);
            }
        }
    }
}
