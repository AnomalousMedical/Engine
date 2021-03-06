﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Renderer;

namespace OgrePlugin
{
    class OgreDebugSurface : DebugDrawingSurface
    {
        private const int NUM_LINES = 32;
        private const int NUM_LINES_IN_45 = NUM_LINES / 8;
        private const int NUM_LINES_IN_45_PLUS_ONE = NUM_LINES_IN_45 + 1;
        private const float PI = 3.14159265f;
        private const string MANUAL_OBJECT_RESERVED_NAME = "__DebugSurfaceManualObject";
        private const string NODE_RESERVED_NAME = "__DebugSurfaceNode";

        private Dictionary<String, ManualObject> manualObjectMap = new Dictionary<string, ManualObject>();
        private ManualObject currentManualObject;
        private SceneNode sceneNode;
        private Color color = Color.White;
        private float[] xAxisLines = new float[NUM_LINES_IN_45_PLUS_ONE];
        private float[] yAxisLines = new float[NUM_LINES_IN_45_PLUS_ONE];
        private SceneManager scene;
        private bool visible;

        private String currentMaterial = "colorvertex";

        public OgreDebugSurface(String name, SceneManager scene)
        {
            this.scene = scene;
            this.sceneNode = scene.createSceneNode(name + NODE_RESERVED_NAME);
            scene.getRootSceneNode().addChild(sceneNode);
            visible = true;
        }

        public void destroy()
        {
            scene.getRootSceneNode().removeChild(sceneNode);
            foreach (ManualObject manualObject in manualObjectMap.Values)
            {
                sceneNode.detachObject(manualObject);
                scene.destroyManualObject(manualObject);
            }
            manualObjectMap.Clear();
            scene.destroySceneNode(sceneNode);
        }

        public void begin(String sectionName, DrawingType drawingType)
        {
            if (manualObjectMap.TryGetValue(sectionName, out currentManualObject))
            {
                if (currentManualObject.getNumSections() != 0)
                {
                    currentManualObject.beginUpdate(0);
                }
                else
                {
                    currentManualObject.begin(currentMaterial, getOpType(drawingType));
                }
            }
            else
            {
                currentManualObject = scene.createManualObject(sectionName + MANUAL_OBJECT_RESERVED_NAME);
                currentManualObject.setRenderQueueGroup((byte)RenderQueueGroupID.RENDER_QUEUE_OVERLAY);
                currentManualObject.setDynamic(true);
                sceneNode.attachObject(currentManualObject);
                currentManualObject.begin(currentMaterial, getOpType(drawingType));
                manualObjectMap.Add(sectionName, currentManualObject);
            }
        }

        public void end()
        {
            currentManualObject.end();
            currentManualObject = null;
        }

        public void destroySection(String sectionName)
        {
            ManualObject manualObject;
            if(manualObjectMap.TryGetValue(sectionName, out manualObject))
            {
                sceneNode.detachObject(manualObject);
                scene.destroyManualObject(manualObject);
                manualObjectMap.Remove(sectionName);
                if(manualObject == currentManualObject)
                {
                    currentManualObject = null;
                }
            }
        }

        /// <summary>
        /// Clear everything in this drawing surface.
        /// </summary>
        public void clearAll()
        {
            foreach (ManualObject manualObject in manualObjectMap.Values)
            {
                sceneNode.detachObject(manualObject);
                scene.destroyManualObject(manualObject);
            }
            manualObjectMap.Clear();
        }

        public void setDepthTesting(bool depthCheckEnabled)
        {
            if (depthCheckEnabled)
            {
                currentMaterial = "colorvertex";
            }
            else
            {
                currentMaterial = "colorVertexNoDepth";
            }
            foreach (ManualObject manualObject in manualObjectMap.Values)
            {
                if (manualObject.getNumSections() != 0)
                {
                    manualObject.setMaterialName(0, currentMaterial);
                }
            }
        }

        public bool isDepthTestingEnabled()
        {
            return currentMaterial == "colorvertex";
        }

        public void moveOrigin(Vector3 newOrigin)
        {
            sceneNode.setPosition(newOrigin);
        }
        
        /// <summary>
        /// Get the origin of this surface.
        /// </summary>
        /// <returns>The origin of the surface.</returns>
        public Vector3 getOrigin()
        {
            return sceneNode.getDerivedPosition();
        }

        /// <summary>
        /// Change the orientation of the surface.
        /// </summary>
        /// <param name="orientation">The orientation to set.</param>
        public void setOrientation(Quaternion orientation)
        {
            sceneNode.setOrientation(orientation);
        }

        /// <summary>
        /// Get the orientation of the surface.
        /// </summary>
        /// <returns>The orientation of the surface.</returns>
        public Quaternion getOrientation()
        {
            return sceneNode.getDerivedOrientation();
        }

        public void setVisible(bool visible)
        {
            if (visible && !this.visible)
            {
                scene.getRootSceneNode().addChild(sceneNode);
                this.visible = true;
            }
            else if(this.visible && !visible)
            {
                scene.getRootSceneNode().removeChild(sceneNode);
                this.visible = false;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public void drawPoint(Vector3 p)
        {
            currentManualObject.position(p.x, p.y, p.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawPoint(ref Vector3 p)
        {
            currentManualObject.position(p.x, p.y, p.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawLine(Vector3 p1, Vector3 p2)
        {
            currentManualObject.position(p1.x, p1.y, p1.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
            currentManualObject.position(p2.x, p2.y, p2.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawLine(ref Vector3 p1, ref Vector3 p2)
        {
            currentManualObject.position(p1.x, p1.y, p1.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
            currentManualObject.position(p2.x, p2.y, p2.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            currentManualObject.position(p1.x, p1.y, p1.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
            currentManualObject.position(p2.x, p2.y, p2.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
            currentManualObject.position(p3.x, p3.y, p3.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawTriangle(ref Vector3 p1, ref Vector3 p2, ref Vector3 p3)
        {
            currentManualObject.position(p1.x, p1.y, p1.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
            currentManualObject.position(p2.x, p2.y, p2.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
            currentManualObject.position(p3.x, p3.y, p3.z);
            currentManualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawCircle(Vector3 origin, Vector3 xAxis, Vector3 yAxis, float radius)
        {
            //x means x in the first 45 degree octant
            //y means y in the first 45 degree octant
            //first octant is 0 to 45 degrees the remaining octants are defined counterclockwise
            //from there

            int i;
            float angleDelta = PI / 4.0f / (float)(NUM_LINES_IN_45);
            float currentAngle = 0.0f;
            for (i = 0; i < NUM_LINES_IN_45_PLUS_ONE; ++i)
            {
                xAxisLines[i] = ((float)Math.Cos(currentAngle)) * radius;
                yAxisLines[i] = ((float)Math.Sin(currentAngle)) * radius;
                currentAngle += angleDelta;
            }

            //first octant
            for (i = NUM_LINES_IN_45; i != 0; --i)
            {
                currentManualObject.position(xAxisLines[i] * xAxis.x + yAxisLines[i] * yAxis.x + origin.x,
                                 xAxisLines[i] * xAxis.y + yAxisLines[i] * yAxis.y + origin.y,
                                 xAxisLines[i] * xAxis.z + yAxisLines[i] * yAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
                currentManualObject.position(xAxisLines[i - 1] * xAxis.x + yAxisLines[i - 1] * yAxis.x + origin.x,
                                 xAxisLines[i - 1] * xAxis.y + yAxisLines[i - 1] * yAxis.y + origin.y,
                                 xAxisLines[i - 1] * xAxis.z + yAxisLines[i - 1] * yAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
            }

            //second octant
            for (i = NUM_LINES_IN_45; i != 0; --i)
            {
                currentManualObject.position(xAxisLines[i] * yAxis.x + yAxisLines[i] * xAxis.x + origin.x,
                                 xAxisLines[i] * yAxis.y + yAxisLines[i] * xAxis.y + origin.y,
                                 xAxisLines[i] * yAxis.z + yAxisLines[i] * xAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
                currentManualObject.position(xAxisLines[i - 1] * yAxis.x + yAxisLines[i - 1] * xAxis.x + origin.x,
                                 xAxisLines[i - 1] * yAxis.y + yAxisLines[i - 1] * xAxis.y + origin.y,
                                 xAxisLines[i - 1] * yAxis.z + yAxisLines[i - 1] * xAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
            }

            //third octant
            for (i = NUM_LINES_IN_45; i != 0; --i)
            {
                currentManualObject.position(xAxisLines[i] * -yAxis.x + yAxisLines[i] * xAxis.x + origin.x,
                                 xAxisLines[i] * -yAxis.y + yAxisLines[i] * xAxis.y + origin.y,
                                 xAxisLines[i] * -yAxis.z + yAxisLines[i] * xAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
                currentManualObject.position(xAxisLines[i - 1] * -yAxis.x + yAxisLines[i - 1] * xAxis.x + origin.x,
                                 xAxisLines[i - 1] * -yAxis.y + yAxisLines[i - 1] * xAxis.y + origin.y,
                                 xAxisLines[i - 1] * -yAxis.z + yAxisLines[i - 1] * xAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
            }

            //fourth octant
            for (i = NUM_LINES_IN_45; i != 0; --i)
            {
                currentManualObject.position(xAxisLines[i] * -xAxis.x + yAxisLines[i] * yAxis.x + origin.x,
                                 xAxisLines[i] * -xAxis.y + yAxisLines[i] * yAxis.y + origin.y,
                                 xAxisLines[i] * -xAxis.z + yAxisLines[i] * yAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
                currentManualObject.position(xAxisLines[i - 1] * -xAxis.x + yAxisLines[i - 1] * yAxis.x + origin.x,
                                 xAxisLines[i - 1] * -xAxis.y + yAxisLines[i - 1] * yAxis.y + origin.y,
                                 xAxisLines[i - 1] * -xAxis.z + yAxisLines[i - 1] * yAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
            }

            //fifth octant
            for (i = NUM_LINES_IN_45; i != 0; --i)
            {
                currentManualObject.position(xAxisLines[i] * -xAxis.x + yAxisLines[i] * -yAxis.x + origin.x,
                                 xAxisLines[i] * -xAxis.y + yAxisLines[i] * -yAxis.y + origin.y,
                                 xAxisLines[i] * -xAxis.z + yAxisLines[i] * -yAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
                currentManualObject.position(xAxisLines[i - 1] * -xAxis.x + yAxisLines[i - 1] * -yAxis.x + origin.x,
                                 xAxisLines[i - 1] * -xAxis.y + yAxisLines[i - 1] * -yAxis.y + origin.y,
                                 xAxisLines[i - 1] * -xAxis.z + yAxisLines[i - 1] * -yAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
            }

            //sixth octant
            for (i = NUM_LINES_IN_45; i != 0; --i)
            {
                currentManualObject.position(xAxisLines[i] * -yAxis.x + yAxisLines[i] * -xAxis.x + origin.x,
                                 xAxisLines[i] * -yAxis.y + yAxisLines[i] * -xAxis.y + origin.y,
                                 xAxisLines[i] * -yAxis.z + yAxisLines[i] * -xAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
                currentManualObject.position(xAxisLines[i - 1] * -yAxis.x + yAxisLines[i - 1] * -xAxis.x + origin.x,
                                 xAxisLines[i - 1] * -yAxis.y + yAxisLines[i - 1] * -xAxis.y + origin.y,
                                 xAxisLines[i - 1] * -yAxis.z + yAxisLines[i - 1] * -xAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
            }

            //seventh octant
            for (i = NUM_LINES_IN_45; i != 0; --i)
            {
                currentManualObject.position(xAxisLines[i] * yAxis.x + yAxisLines[i] * -xAxis.x + origin.x,
                                 xAxisLines[i] * yAxis.y + yAxisLines[i] * -xAxis.y + origin.y,
                                 xAxisLines[i] * yAxis.z + yAxisLines[i] * -xAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
                currentManualObject.position(xAxisLines[i - 1] * yAxis.x + yAxisLines[i - 1] * -xAxis.x + origin.x,
                                 xAxisLines[i - 1] * yAxis.y + yAxisLines[i - 1] * -xAxis.y + origin.y,
                                 xAxisLines[i - 1] * yAxis.z + yAxisLines[i - 1] * -xAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
            }

            //eigth octant
            for (i = NUM_LINES_IN_45; i != 0; --i)
            {
                currentManualObject.position(xAxisLines[i] * xAxis.x + yAxisLines[i] * -yAxis.x + origin.x,
                                 xAxisLines[i] * xAxis.y + yAxisLines[i] * -yAxis.y + origin.y,
                                 xAxisLines[i] * xAxis.z + yAxisLines[i] * -yAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
                currentManualObject.position(xAxisLines[i - 1] * xAxis.x + yAxisLines[i - 1] * -yAxis.x + origin.x,
                                 xAxisLines[i - 1] * xAxis.y + yAxisLines[i - 1] * -yAxis.y + origin.y,
                                 xAxisLines[i - 1] * xAxis.z + yAxisLines[i - 1] * -yAxis.z + origin.z);
                currentManualObject.color(color.r, color.g, color.b, color.a);
            }
        }

        private OperationType getOpType(DrawingType drawingType)
        {
            OperationType opType = OperationType.OT_LINE_LIST;
            switch (drawingType)
            {
                case DrawingType.LineList:
                    opType = OperationType.OT_LINE_LIST;
                    break;
                case DrawingType.LineStrip:
                    opType = OperationType.OT_LINE_STRIP;
                    break;
                case DrawingType.PointList:
                    opType = OperationType.OT_POINT_LIST;
                    break;
                case DrawingType.TriangleFan:
                    opType = OperationType.OT_TRIANGLE_FAN;
                    break;
                case DrawingType.TriangleList:
                    opType = OperationType.OT_TRIANGLE_LIST;
                    break;
                case DrawingType.TriangleStrip:
                    opType = OperationType.OT_TRIANGLE_STRIP;
                    break;
            }
            return opType;
        }
    }
}
