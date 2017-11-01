﻿using Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Engine.Shim;

namespace Anomalous.Shim
{
    public class FullNetFrameworkShim : INetFrameworkShim
    {
        public FullNetFrameworkShim()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(i => i.FullName == args.Name);
        }

        public string CurrentAssemblyName
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Name;
            }
        }

        public Assembly LoadFile(string path)
        {
            return Assembly.LoadFile(path);
        }

        public void PrintStackTrace()
        {
            var stackTrace = new System.Diagnostics.StackTrace(1, true);
            StringBuilder sb = new StringBuilder(512);
            sb.AppendLine("Stack Trace");
            sb.AppendLine("----------------------");
            foreach (var frame in stackTrace.GetFrames())
            {
                MethodBase method = frame.GetMethod();
                sb.AppendLine(String.Format("{0}::{1}", method.ReflectedType != null ? method.ReflectedType.Name : string.Empty, method.Name));
            }
            sb.Append("----------------------");
            Log.Debug(sb.ToString());
        }

        public void ParallelFor(int fromInclusive, int toExclusive, Action<int> body)
        {
            Parallel.For(fromInclusive, toExclusive, body);
        }

        public IEnumerable<Assembly> LoadedAssemblies
        {
            get
            {
                return AppDomain.CurrentDomain.GetAssemblies();
            }
        }
    }
}
