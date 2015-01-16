using Anomalous.OSPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomaly
{
    class AnomalyApp : App
    {
        private AnomalyController anomalyController;
        private String projectFileName;

        public AnomalyApp(String projectFileName)
        {
            this.projectFileName = projectFileName;
            anomalyController = new AnomalyController(this, new Solution(projectFileName));
        }

        public override bool OnInit()
        {
            return true;
        }

        public override int OnExit()
        {
            anomalyController.Dispose();
            return 0;
        }

        public override void OnIdle()
        {
            anomalyController.idle();
        }

        public AnomalyController AnomalyController
        {
            get
            {
                return anomalyController;
            }
        }
    }
}
