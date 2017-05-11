using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPlatform.Win32
{
    class ProcessStartInfoShim : Engine.Shim.ProcessStartInfo
    {
        private ProcessStartInfo processStartInfo;

        public ProcessStartInfoShim(ProcessStartInfo processStartInfo)
        {
            this.processStartInfo = processStartInfo;
        }

        public string Arguments
        {
            get
            {
                return processStartInfo.Arguments;
            }
            set
            {
                processStartInfo.Arguments = value;
            }
        }

        public string Verb
        {
            get
            {
                return processStartInfo.Verb;
            }
            set
            {
                processStartInfo.Verb = value;
            }
        }

        public bool UseShellExecute
        {
            get
            {
                return processStartInfo.UseShellExecute;
            }
            set
            {
                processStartInfo.UseShellExecute = value;
            }
        }

        public ProcessStartInfo StartInfo
        {
            get
            {
                return processStartInfo;
            }
        }
    }
}