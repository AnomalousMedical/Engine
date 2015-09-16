using Anomalous.OSPlatform;
using Anomalous.TextureCompiler;
using Engine;
using Engine.Threads;
using Logging;
using MyGUIPlugin;
using OgrePlugin;
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

        CheckButton dxt;
        CheckButton bc5;
        CheckButton etc2;
        CheckButton uncompressed;

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

            dxt = new CheckButton(window.findWidget("DXT") as Button);
            bc5 = new CheckButton(window.findWidget("BC5Normal") as Button);
            etc2 = new CheckButton(window.findWidget("ETC2") as Button);
            uncompressed = new CheckButton(window.findWidget("Uncompressed") as Button);

            uncompressed.Checked = true;
        }

        public String CurrentDest
        {
            get
            {
                return this.dest.OnlyText;
            }
            set
            {
                this.dest.OnlyText = value;
            }
        }

        public String CurrentSrc
        {
            get
            {
                return this.source.OnlyText;
            }
            set
            {
                this.source.OnlyText = value;
            }
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
                TextureCompilerInterface.CompileTextures(this.source.OnlyText, this.dest.OnlyText, pluginManager, ActiveCompileFormats);
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

        private OutputFormats ActiveCompileFormats
        {
            get
            {
                OutputFormats compile = OutputFormats.None;
                if(dxt.Checked)
                {
                    compile |= OutputFormats.BC3;
                }
                if(bc5.Checked)
                {
                    compile |= OutputFormats.BC5Normal;
                }
                if(etc2.Checked)
                {
                    compile |= OutputFormats.ETC2;
                }
                if(uncompressed.Checked)
                {
                    compile |= OutputFormats.Uncompressed;
                }
                return compile;
            }
        }
    }
}
