using Anomaly;
using GameAppTest;
using System;

namespace GameAppTestNetCoreAnomaly
{
    static class Program
    {
        public static void Main()
        {
            AnomalyProgram.Run(new GameAppAnomaly(new Startup()));
        }
    }
}
