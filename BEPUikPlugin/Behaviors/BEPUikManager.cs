using BEPUik;
using Engine;
using Engine.Attributes;
using Engine.ObjectManagement;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BEPUikPlugin.IKTest
{
    class BEPUikManager : Behavior
    {
        SimObject targetObj;

        [DoNotCopy]
        [DoNotSave]
        private IKSolver ikSolver = new IKSolver();

        [DoNotCopy]
        [DoNotSave]
        List<BEPUTestControl> controls = new List<BEPUTestControl>();

        [DoNotCopy]
        [DoNotSave]
        List<BEPUTestBone> testBones = new List<BEPUTestBone>();

        protected override void constructed()
        {
            base.constructed();
            ikSolver = new IKSolver();
        }

        protected override void destroy()
        {
            ikSolver.Dispose();
            base.destroy();
        }

        public void addBone(BEPUTestBone bone)
        {
            testBones.Add(bone);
        }

        public void removeBone(BEPUTestBone bone)
        {
            testBones.Remove(bone);
        }

        public void addController(BEPUTestControl control)
        {
            controls.Add(control);
        }

        public void removeController(BEPUTestControl control)
        {
            controls.Remove(control);
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            foreach (var control in controls)
            {
                control.syncPosition();
            }

            ikSolver.Solve(controls.Select(c => c.Control).ToList()); //Lots of garbage

            foreach (var bone in testBones)
            {
                bone.syncSimObject();
            }
        }
    }
}
