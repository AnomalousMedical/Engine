using Anomalous.OSPlatform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreModelEditor
{
    class OgreModelEditorApp : App
    {
        private OgreModelEditorController controller;

        public override bool OnInit()
        {
            String defaultModel = null;

            String[] commandLine = Environment.GetCommandLineArgs();
            if (commandLine.Length > 1)
            {
                String file = commandLine[1];
                if (File.Exists(file) && file.EndsWith(".mesh"))
                {
                    defaultModel = file;
                }
            }

            controller = new OgreModelEditorController(this, defaultModel);

            return true;
        }

        public override void Dispose()
        {
            controller.Dispose();
            base.Dispose();
        }

        public override int OnExit()
        {
            return 0;
        }

        public override void OnIdle()
        {
            controller.idle();
        }

        public void saveCrashLog()
        {

        }
    }
}
