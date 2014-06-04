using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Engine.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin.Behaviors
{
    /// <summary>
    /// This enum contains events that the tool behaviors listen for.
    /// </summary>
    enum ToolEvents
    {
        Pick,
        IncreaseToolSize,
        DecreaseToolSize,
    }

    class Grabber : Behavior, MovableObject
    {
        static Grabber()
        {
            MessageEvent pickEvent = new MessageEvent(ToolEvents.Pick);
            pickEvent.addButton(MouseButtonCode.MB_BUTTON0);
            DefaultEvents.registerDefaultEvent(pickEvent);

            MessageEvent increaseToolSize = new MessageEvent(ToolEvents.IncreaseToolSize);
            increaseToolSize.addButton(KeyboardButtonCode.KC_EQUALS);
            DefaultEvents.registerDefaultEvent(increaseToolSize);

            MessageEvent decreaseToolSize = new MessageEvent(ToolEvents.DecreaseToolSize);
            decreaseToolSize.addButton(KeyboardButtonCode.KC_MINUS);
            DefaultEvents.registerDefaultEvent(decreaseToolSize);
        }

        private DebugDrawingSurface drawingSurface = null;
        private List<MovableObjectTools> movableObjects = new List<MovableObjectTools>();
        private MovableObjectTools currentTools = null;
        private bool showMoveTools = false;
        private bool showRotateTools = false;
        private float toolSize = 1.0f;

        public Grabber()
        {
            
        }

        protected override void link()
        {
            base.link();
            SimSubScene subScene = Owner.SubScene;
            if (subScene != null)
            {
                drawingSurface = PluginManager.Instance.RendererPlugin.createDebugDrawingSurface(String.Format("{0}_{1}_DebugSurface", Owner.Name, Name), subScene);
                drawingSurface.setDepthTesting(false);
            }
            ShowMoveTools = true;
            this.addMovableObject(String.Format("{0}_{1}_MoveTool", Owner.Name, Name), this);
            this.setDrawingSurfaceVisible(true);
        }

        protected override void destroy()
        {
            base.destroy();
            this.removeMovableObject(this);
            if (drawingSurface != null)
            {
                PluginManager.Instance.RendererPlugin.destroyDebugDrawingSurface(drawingSurface);
                drawingSurface = null;
            }
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            //Process the mouse
            Mouse mouse = eventManager.Mouse;
            Vector3 mouseLoc = mouse.getAbsMouse();
            Ray3 spaceRay = new Ray3();
            Vector3 cameraPos = Vector3.Zero;
            CameraMotionValidator validator = CameraResolver.getValidatorForLocation((int)mouseLoc.x, (int)mouseLoc.y);
            if (validator != null)
            {
                validator.getLocalCoords(ref mouseLoc.x, ref mouseLoc.y);
                SceneView camera = validator.getCamera();
                spaceRay = camera.getCameraToViewportRay(mouseLoc.x / validator.getMouseAreaWidth(), mouseLoc.y / validator.getMouseAreaHeight());
                cameraPos = camera.Translation;              
            }
            //Check collisions and draw shapes
            if (!eventManager[ToolEvents.Pick].Down)
            {
                float closestDistance = float.MaxValue;
                MovableObjectTools closestTools = null;
                foreach (MovableObjectTools tools in movableObjects)
                {
                    if (tools.checkBoundingBoxCollision(ref spaceRay))
                    {
                        if (tools.processAxes(ref spaceRay))
                        {
                            float distance = (tools.Movable.ToolTranslation - cameraPos).length2();
                            if (distance < closestDistance)
                            {
                                //If we had a previous closer tool clear its selection.
                                if (closestTools != null)
                                {
                                    closestTools.clearSelection();
                                }
                                closestTools = tools;
                                closestDistance = distance;
                            }
                            //If this tool was not closer clear its selection.
                            else
                            {
                                tools.clearSelection();
                            }
                        }
                        else
                        {
                            tools.clearSelection();
                        }
                    }
                    else
                    {
                        tools.clearSelection();
                    }
                }
                if (eventManager[ToolEvents.Pick].FirstFrameDown)
                {
                    currentTools = closestTools;
                    if (closestTools != null)
                    {
                        closestTools.processSelection(eventManager, ref cameraPos, ref spaceRay);
                    }
                }
            }
            else
            {
                if (currentTools != null)
                {
                    currentTools.processSelection(eventManager, ref cameraPos, ref spaceRay);
                }
            }  
            foreach (MovableObjectTools tools in movableObjects)
            {
                tools.drawTools(drawingSurface);
            }
        }

        public void addMovableObject(String name, MovableObject movable)
        {
            MovableObjectTools tools = new MovableObjectTools(name, movable, toolSize);
            movableObjects.Add(tools);
            tools.MoveToolVisible = showMoveTools;
            tools.RotateToolVisible = showRotateTools;
        }

        public void removeMovableObject(MovableObject movable)
        {
            MovableObjectTools tools = null;
            foreach (MovableObjectTools currentTools in movableObjects)
            {
                if (currentTools.Movable == movable)
                {
                    tools = currentTools;
                    break;
                }
            }
            if (tools != null)
            {
                movableObjects.Remove(tools);
            }
        }

        public void setDrawingSurfaceVisible(bool visible)
        {
            if (drawingSurface != null)
            {
                drawingSurface.setVisible(visible);
            }
        }

        public void setActivePlanes(MovementAxis axes, MovementPlane planes)
        {
            foreach (MovableObjectTools currentTools in movableObjects)
            {
                currentTools.setActiveAxes(axes);
                currentTools.setActivePlanes(planes);
            }
        }

        public bool ShowMoveTools
        {
            get
            {
                return showMoveTools;
            }
            set
            {
                showMoveTools = value;
                foreach (MovableObjectTools tools in movableObjects)
                {
                    tools.MoveToolVisible = value;
                }
            }
        }

        public bool ShowRotateTools
        {
            get
            {
                return showRotateTools;
            }
            set
            {
                showRotateTools = value;
                foreach (MovableObjectTools tools in movableObjects)
                {
                    tools.RotateToolVisible = value;
                }
            }
        }

        public float ToolSize
        {
            get
            {
                return toolSize;
            }
            set
            {
                toolSize = value;
                foreach (MovableObjectTools tools in movableObjects)
                {
                    tools.setToolSize(toolSize);
                }
            }
        }

        public Vector3 ToolTranslation
        {
            get
            {
                return Owner.Translation;
            }
        }

        public void move(Vector3 offset)
        {
            updateTranslation(Owner.Translation + offset);
        }

        public Quaternion ToolRotation
        {
            get
            {
                return Owner.Rotation;
            }
        }

        public bool ShowTools
        {
            get
            {
                return true;
            }
        }

        public void rotate(ref Quaternion newRot)
        {
            updateRotation(ref newRot);
        }

        public void alertToolHighlightStatus(bool highlighted)
        {
            
        }
    }
}
