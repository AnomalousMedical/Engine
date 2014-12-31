﻿using Anomalous.OSPlatform;
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

        public override void Dispose()
        {
            controller.Dispose();
            base.Dispose();
        }

        public override bool OnInit()
        {
            controller = new OgreModelEditorController();
            controller.initialize(this);

            String[] commandLine = Environment.GetCommandLineArgs();
            if (commandLine.Length > 1)
            {
                String file = commandLine[1];
                if (File.Exists(file) && file.EndsWith(".mesh"))
                {
                    controller.openModel(file);
                }
            }

            return true;
        }

        public override int OnExit()
        {
            return 0;
        }

        public override void OnIdle()
        {
            controller.MainTimer.OnIdle();
        }

        public void saveCrashLog()
        {

        }
    }
}
