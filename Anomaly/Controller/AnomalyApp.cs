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
        }

        public override void Dispose()
        {
            anomalyController.Dispose();
            base.Dispose();
        }

        public override bool OnInit()
        {
            anomalyController = new AnomalyController(this);

            anomalyController.initialize(new Solution(projectFileName));
            anomalyController.buildScene();

            return true;
        }

        public override int OnExit()
        {
            return 0;
        }

        public override void OnIdle()
        {
            anomalyController.idle();
        }
    }
}
