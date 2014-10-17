using BEPUik;
using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikScene : SimElementManager
    {
        private String name;
        private BEPUIkFactory factory;
        private UpdateTimer timer;
        private BEPUikSceneUpdater updater;
        private BEPUikSolver rootSolver;
        private Dictionary<String, BEPUikSolver> namedSolvers = new Dictionary<string, BEPUikSolver>();

        public BEPUikScene(BEPUikSceneDefinition definition, UpdateTimer timer)
        {
            this.timer = timer;
            this.name = definition.Name;
            factory = new BEPUIkFactory(this);
            updater = new BEPUikSceneUpdater(this);
            timer.addBackgroundUpdateListener("Rendering", updater);
            rootSolver = new BEPUikSolver(definition, "Root");
        }

        public void Dispose()
        {
            timer.removeBackgroundUpdateListener("Rendering", updater);
        }

        public SimElementFactory getFactory()
        {
            return factory;
        }

        internal BEPUIkFactory IkFactory
        {
            get
            {
                return factory;
            }
        }

        public Type getSimElementManagerType()
        {
            return typeof(BEPUikScene);
        }

        public string getName()
        {
            return name;
        }

        public SimElementManagerDefinition createDefinition()
        {
            return new BEPUikSceneDefinition(name)
                {
                    ActiveSetUseAutomass = rootSolver.IKSolver.ActiveSet.UseAutomass,
                    AutoscaleControlImpulses = rootSolver.IKSolver.AutoscaleControlImpulses,
                    AutoscaleControlMaximumForce = rootSolver.IKSolver.AutoscaleControlMaximumForce,
                    TimeStepDuration = rootSolver.IKSolver.TimeStepDuration,
                    ControlIterationCount = rootSolver.IKSolver.ControlIterationCount,
                    FixerIterationCount = rootSolver.IKSolver.FixerIterationCount,
                    VelocitySubiterationCount = rootSolver.IKSolver.VelocitySubiterationCount
                };
        }

        internal void addBone(BEPUikBone bone)
        {
            BEPUikSolver solver;
            if(!namedSolvers.TryGetValue(bone.SolverName, out solver))
            {
                solver = rootSolver;
            }
            solver.addBone(bone);
        }

        internal void removeBone(BEPUikBone bone)
        {
            BEPUikSolver solver;
            if (!namedSolvers.TryGetValue(bone.SolverName, out solver))
            {
                solver = rootSolver;
            }
            solver.removeBone(bone);
        }

        internal void addControl(BEPUikControl control)
        {
            BEPUikSolver solver;
            if (!namedSolvers.TryGetValue(control.Bone.SolverName, out solver))
            {
                solver = rootSolver;
            }
            solver.addControl(control);
        }

        internal void removeControl(BEPUikControl control)
        {
            BEPUikSolver solver;
            if (!namedSolvers.TryGetValue(control.Bone.SolverName, out solver))
            {
                solver = rootSolver;
            }
            solver.removeControl(control);
        }

        public void addExternalControl(ExternalControl control)
        {
            BEPUikSolver solver;
            if (!namedSolvers.TryGetValue(control.TargetBone.SolverName, out solver))
            {
                solver = rootSolver;
            }
            solver.addExternalControl(control);
        }

        public void removeExternalControl(ExternalControl control)
        {
            BEPUikSolver solver;
            if (control.CurrentSolverName == null || !namedSolvers.TryGetValue(control.CurrentSolverName, out solver))
            {
                solver = rootSolver;
            }
            solver.removeExternalControl(control);
        }

        //public void solveJoints(List<IKJoint> joints)
        //{
        //    solver.solveJoints(joints);
        //}

        //public List<IKJoint> findAllJointsFrom(BEPUikBone bone)
        //{
        //    DragControl control = new DragControl();
        //    control.TargetBone = bone.IkBone;
        //    List<Control> dummyList = new List<Control>();
        //    dummyList.Add(control);
        //    ActiveSet dummyActiveSet = new ActiveSet();
        //    dummyActiveSet.UpdateActiveSet(dummyList);
        //    return new List<IKJoint>(dummyActiveSet.Joints);
        //}

        internal void backgroundUpdate()
        {
            PerformanceMonitor.start("BEPU IK Background");
            rootSolver.update();
            PerformanceMonitor.stop("BEPU IK Background");
        }

        internal void sync()
        {
            rootSolver.sync();
        }

        internal void drawDebug(DebugDrawingSurface drawingSurface, DebugDrawMode drawMode)
        {
            drawingSurface.begin("BepuIkScene", Engine.Renderer.DrawingType.LineList);
            rootSolver.drawDebug(drawingSurface, drawMode);
            drawingSurface.end();
        }
    }
}
