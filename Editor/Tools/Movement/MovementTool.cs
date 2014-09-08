using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Attributes;
using Engine.Renderer;
using Engine;
using Engine.Platform;

namespace Editor
{
    public class MovementTool : Tool
    {
        #region Static

        public static readonly ButtonEvent PickEvent;
        public static readonly ButtonEvent IncreaseToolSize;
        public static readonly ButtonEvent DecreaseToolSize;

        static MovementTool()
        {
            PickEvent = new ButtonEvent(EventLayers.Main);
            PickEvent.addButton(MouseButtonCode.MB_BUTTON0);
            DefaultEvents.registerDefaultEvent(PickEvent);

            IncreaseToolSize = new ButtonEvent(EventLayers.Main);
            IncreaseToolSize.addButton(KeyboardButtonCode.KC_EQUALS);
            DefaultEvents.registerDefaultEvent(IncreaseToolSize);

            DecreaseToolSize = new ButtonEvent(EventLayers.Main);
            DecreaseToolSize.addButton(KeyboardButtonCode.KC_MINUS);
            DefaultEvents.registerDefaultEvent(DecreaseToolSize);
        }

        const float LENGTH_DELTA = 1.0f;
        const float DOUBLE_AXIS_SCALE = 3.0f;

        #endregion Static

        #region Fields

        private DebugDrawingSurface axisSurface;
        private MoveController moveController; 
        private Axis xAxisBox;
        private Axis yAxisBox;
        private Axis zAxisBox;
        private Axis xzAxisBox;
        private Axis xyAxisBox;
        private Axis yzAxisBox;
        private Vector3 mouseOffset;
        private float currentLength = 10.0f;
        private String name;
        private bool enabled = true;
        private Vector3 savedOrigin = Vector3.Zero; //The origin of the tool when it was destroyed.

        #endregion Fields

        #region Constructors

        public MovementTool(String name, MoveController manager)
        {
            this.moveController = manager;
            this.name = name;
            xAxisBox = new Axis(Vector3.Right, currentLength, new Color(1.0f, 0.0f, 0.0f));
            yAxisBox = new Axis(Vector3.Up, currentLength, new Color(0.0f, 0.0f, 1.0f));
            zAxisBox = new Axis(Vector3.Backward, currentLength, new Color(0.0f, 1.0f, 0.0f));
            xzAxisBox = new Axis(Vector3.Right + Vector3.Backward, currentLength / DOUBLE_AXIS_SCALE, new Color(1.0f, 0.0f, 1.0f));
            xyAxisBox = new Axis(Vector3.Right + Vector3.Up, currentLength / DOUBLE_AXIS_SCALE, new Color(1.0f, 0.0f, 1.0f));
            yzAxisBox = new Axis(Vector3.Up + Vector3.Backward, currentLength / DOUBLE_AXIS_SCALE, new Color(1.0f, 0.0f, 1.0f));
        }

        #endregion Constructors

        #region Functions

        public void createSceneElement(SimSubScene subScene, PluginManager pluginManager)
        {
            axisSurface = pluginManager.RendererPlugin.createDebugDrawingSurface(name, subScene);
            if (axisSurface != null)
            {
                axisSurface.setDepthTesting(false);
                axisSurface.setVisible(enabled);
                axisSurface.moveOrigin(savedOrigin);
            }
        }

        public void destroySceneElement(SimSubScene subScene, PluginManager pluginManager)
        {
            if (axisSurface != null)
            {
                savedOrigin = axisSurface.getOrigin();
                pluginManager.RendererPlugin.destroyDebugDrawingSurface(axisSurface);
                axisSurface = null;
            }
        }

        public void update(EventLayer events)
        {
            if (axisSurface != null)
            {
                //Process the mouse
                Mouse mouse = events.Mouse;
                IntVector3 mouseLoc = mouse.AbsolutePosition;
                CameraMotionValidator validator = CameraResolver.getValidatorForLocation((int)mouseLoc.x, (int)mouseLoc.y);
                if (validator != null)
                {
                    validator.getLocalCoords(ref mouseLoc.x, ref mouseLoc.y);
                    processSelection(events, validator, mouseLoc);
                }

                //Check for resize
                if (IncreaseToolSize.FirstFrameUp)
                {
                    currentLength += LENGTH_DELTA;
                    resizeAxes();
                }
                else if (DecreaseToolSize.FirstFrameUp)
                {
                    if (currentLength - LENGTH_DELTA > 0)
                    {
                        currentLength -= LENGTH_DELTA;
                        resizeAxes();
                    }
                }
            }
        }

        public void setEnabled(bool enabled)
        {
            if (axisSurface != null)
            {
                axisSurface.setVisible(enabled);
            }
            this.enabled = enabled;
        }

        public void setTranslation(Vector3 newTranslation)
        {
            if (axisSurface != null)
            {
                axisSurface.moveOrigin(newTranslation);
            }
        }

        private void resizeAxes()
        {
            xAxisBox.setLength(currentLength);
            yAxisBox.setLength(currentLength);
            zAxisBox.setLength(currentLength);
            xyAxisBox.setLength(currentLength / DOUBLE_AXIS_SCALE);
            xzAxisBox.setLength(currentLength / DOUBLE_AXIS_SCALE);
            yzAxisBox.setLength(currentLength / DOUBLE_AXIS_SCALE);
        }

        private void processSelection(EventLayer events, CameraMotionValidator validator, IntVector3 mouseLoc)
        {
            Vector3 trans = moveController.Translation;
            SceneView camera = validator.getCamera();
            Ray3 spaceRay = camera.getCameraToViewportRay(mouseLoc.x / validator.getMouseAreaWidth(), mouseLoc.y / validator.getMouseAreaHeight());
            float distance = (camera.Translation - moveController.Translation).length();
            Vector3 spacePoint = spaceRay.Direction * distance + spaceRay.Origin;
            if (PickEvent.FirstFrameDown)
            {
                mouseOffset = -(spacePoint - moveController.Translation);
            }
            else if (PickEvent.HeldDown)
            {
                spacePoint += -moveController.Translation + mouseOffset;

                Vector3 newPos = xAxisBox.translate(spacePoint)
                    + yAxisBox.translate(spacePoint)
                    + zAxisBox.translate(spacePoint)
                    + xzAxisBox.translate(spacePoint)
                    + xyAxisBox.translate(spacePoint)
                    + yzAxisBox.translate(spacePoint);

                newPos += moveController.Translation;
                moveController.setTranslation(ref newPos, this);
            }
            else
            {
                processAxis(ref spaceRay);
            }
        }

        private void processAxis(ref Ray3 spaceRay)
        {
            xzAxisBox.process(spaceRay, moveController.Translation);
            xyAxisBox.process(spaceRay, moveController.Translation);
            yzAxisBox.process(spaceRay, moveController.Translation);
            xAxisBox.process(spaceRay, moveController.Translation);
            yAxisBox.process(spaceRay, moveController.Translation);
            zAxisBox.process(spaceRay, moveController.Translation);

            if (xzAxisBox.isSelected())
            {
                xyAxisBox.clearSelection();
                yzAxisBox.clearSelection();
                xAxisBox.clearSelection();
                yAxisBox.clearSelection();
                zAxisBox.clearSelection();
            }
            else if (xyAxisBox.isSelected())
            {
                xzAxisBox.clearSelection();
                yzAxisBox.clearSelection();
                xAxisBox.clearSelection();
                yAxisBox.clearSelection();
                zAxisBox.clearSelection();
            }
            else if (yzAxisBox.isSelected())
            {
                xzAxisBox.clearSelection();
                xyAxisBox.clearSelection();
                xAxisBox.clearSelection();
                yAxisBox.clearSelection();
                zAxisBox.clearSelection();
            }

            drawAxis();
        }

        private void drawAxis()
        {
            axisSurface.begin("MoveTool", DrawingType.LineList);
            xAxisBox.drawLine(axisSurface);
            yAxisBox.drawLine(axisSurface);
            zAxisBox.drawLine(axisSurface);
            xyAxisBox.drawSquare(axisSurface);
            xzAxisBox.drawSquare(axisSurface);
            yzAxisBox.drawSquare(axisSurface);
            axisSurface.end();
        }

        #endregion Functions

        #region Properties

        public string Name
        {
            get
            {
                return name;
            }
        }

        #endregion Properties
    }
}
