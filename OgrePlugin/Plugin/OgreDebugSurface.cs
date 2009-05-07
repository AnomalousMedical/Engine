using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using OgreWrapper;
using Engine.Renderer;

namespace OgrePlugin
{
    class OgreDebugSurface : DebugDrawingSurface
    {
        private const int NUM_LINES = 32;
        private const int NUM_LINES_IN_45 = NUM_LINES / 8 + 1;
        private const float PI = 3.14159265f;
        private const string MANUAL_OBJECT_RESERVED_NAME = "__DebugSurfaceManualObject";
        private const string NODE_RESERVED_NAME = "__DebugSurfaceNode";

        private bool renderedOnce = false;
        private ManualObject manualObject;
        private SceneNode sceneNode;
        OperationType opType;
        private Color color = Color.White;
        private float[] xAxisLines = new float[NUM_LINES_IN_45];
        private float[] yAxisLines = new float[NUM_LINES_IN_45];
        private SceneManager scene;

        public OgreDebugSurface(String name, SceneManager scene, DrawingType drawingType)
        {
            this.scene = scene;
            this.manualObject = scene.createManualObject(name + MANUAL_OBJECT_RESERVED_NAME);
            manualObject.setRenderQueueGroup(byte.MaxValue);
            this.sceneNode = scene.createSceneNode(name + NODE_RESERVED_NAME);
            sceneNode.attachObject(manualObject);
            scene.getRootSceneNode().addChild(sceneNode);
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
        }

        public void destroy()
        {
            scene.getRootSceneNode().removeChild(sceneNode);
            sceneNode.detachObject(manualObject);
            scene.destroyManualObject(manualObject);
            scene.destroySceneNode(sceneNode);
        }

        public void estimateNumVertices(uint numVertices)
        {
            manualObject.estimateVertexCount(numVertices);
        }

        public void begin()
        {
            if (renderedOnce)
            {
                manualObject.beginUpdate(0);
            }
            else
            {
                manualObject.begin("colorVertexNoDepth", opType);
                renderedOnce = true;
            }
        }

        public void end()
        {
            manualObject.end();
        }

        public void moveOrigin(Vector3 newOrigin)
        {
            sceneNode.setPosition(newOrigin);
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public void drawPoint(Vector3 p)
        {
            manualObject.position(p.x, p.y, p.z);
            manualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawPoint(ref Vector3 p)
        {
            manualObject.position(p.x, p.y, p.z);
            manualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawLine(Vector3 p1, Vector3 p2)
        {
            manualObject.position(p1.x, p1.y, p1.z);
            manualObject.color(color.r, color.g, color.b, color.a);
            manualObject.position(p2.x, p2.y, p2.z);
            manualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawLine(ref Vector3 p1, ref Vector3 p2)
        {
            manualObject.position(p1.x, p1.y, p1.z);
            manualObject.color(color.r, color.g, color.b, color.a);
            manualObject.position(p2.x, p2.y, p2.z);
            manualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            manualObject.position(p1.x, p1.y, p1.z);
            manualObject.color(color.r, color.g, color.b, color.a);
            manualObject.position(p2.x, p2.y, p2.z);
            manualObject.color(color.r, color.g, color.b, color.a);
            manualObject.position(p3.x, p3.y, p3.z);
            manualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawTriangle(ref Vector3 p1, ref Vector3 p2, ref Vector3 p3)
        {
            manualObject.position(p1.x, p1.y, p1.z);
            manualObject.color(color.r, color.g, color.b, color.a);
            manualObject.position(p2.x, p2.y, p2.z);
            manualObject.color(color.r, color.g, color.b, color.a);
            manualObject.position(p3.x, p3.y, p3.z);
            manualObject.color(color.r, color.g, color.b, color.a);
        }

        public void drawCircle(Vector3 origin, Vector3 xAxis, Vector3 yAxis, float radius)
        {
            drawCircle(ref origin, ref xAxis, ref yAxis, radius);
        }

        public void drawCircle(ref Vector3 origin, Vector3 xAxis, Vector3 yAxis, float radius)
        {
            drawCircle(ref origin, ref xAxis, ref yAxis, radius);
        }

        public unsafe void drawCircle(ref Vector3 origin, ref Vector3 xAxis, ref Vector3 yAxis, float radius)
        {
            //x means x in the first 45 degree octant
            //y means y in the first 45 degree octant
            //first octant is 0 to 45 degrees the remaining octants are defined counterclockwise
            //from there

            int i;
            float angleDelta = PI / 4.0f / (float)NUM_LINES_IN_45;
            float currentAngle = 0.0f;
            for (i = 0; i < NUM_LINES_IN_45; ++i)
            {
                xAxisLines[i] = ((float)Math.Cos(currentAngle)) * radius;
                yAxisLines[i] = ((float)Math.Sin(currentAngle)) * radius;
                currentAngle += angleDelta;
            }

            //first octant
            for (i = NUM_LINES_IN_45 - 1; i != 0; --i)
            {
                manualObject.position(xAxisLines[i] * xAxis.x + yAxisLines[i] * yAxis.x + origin.x,
                                 xAxisLines[i] * xAxis.y + yAxisLines[i] * yAxis.y + origin.y,
                                 xAxisLines[i] * xAxis.z + yAxisLines[i] * yAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
                manualObject.position(xAxisLines[i - 1] * xAxis.x + yAxisLines[i - 1] * yAxis.x + origin.x,
                                 xAxisLines[i - 1] * xAxis.y + yAxisLines[i - 1] * yAxis.y + origin.y,
                                 xAxisLines[i - 1] * xAxis.z + yAxisLines[i - 1] * yAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
            }

            //second octant
            for (i = NUM_LINES_IN_45 - 1; i != 0; --i)
            {
                manualObject.position(xAxisLines[i] * yAxis.x + yAxisLines[i] * xAxis.x + origin.x,
                                 xAxisLines[i] * yAxis.y + yAxisLines[i] * xAxis.y + origin.y,
                                 xAxisLines[i] * yAxis.z + yAxisLines[i] * xAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
                manualObject.position(xAxisLines[i - 1] * yAxis.x + yAxisLines[i - 1] * xAxis.x + origin.x,
                                 xAxisLines[i - 1] * yAxis.y + yAxisLines[i - 1] * xAxis.y + origin.y,
                                 xAxisLines[i - 1] * yAxis.z + yAxisLines[i - 1] * xAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
            }

            //third octant
            for (i = NUM_LINES_IN_45 - 1; i != 0; --i)
            {
                manualObject.position(xAxisLines[i] * -yAxis.x + yAxisLines[i] * xAxis.x + origin.x,
                                 xAxisLines[i] * -yAxis.y + yAxisLines[i] * xAxis.y + origin.y,
                                 xAxisLines[i] * -yAxis.z + yAxisLines[i] * xAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
                manualObject.position(xAxisLines[i - 1] * -yAxis.x + yAxisLines[i - 1] * xAxis.x + origin.x,
                                 xAxisLines[i - 1] * -yAxis.y + yAxisLines[i - 1] * xAxis.y + origin.y,
                                 xAxisLines[i - 1] * -yAxis.z + yAxisLines[i - 1] * xAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
            }

            //fourth octant
            for (i = NUM_LINES_IN_45 - 1; i != 0; --i)
            {
                manualObject.position(xAxisLines[i] * -xAxis.x + yAxisLines[i] * yAxis.x + origin.x,
                                 xAxisLines[i] * -xAxis.y + yAxisLines[i] * yAxis.y + origin.y,
                                 xAxisLines[i] * -xAxis.z + yAxisLines[i] * yAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
                manualObject.position(xAxisLines[i - 1] * -xAxis.x + yAxisLines[i - 1] * yAxis.x + origin.x,
                                 xAxisLines[i - 1] * -xAxis.y + yAxisLines[i - 1] * yAxis.y + origin.y,
                                 xAxisLines[i - 1] * -xAxis.z + yAxisLines[i - 1] * yAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
            }

            //fifth octant
            for (i = NUM_LINES_IN_45 - 1; i != 0; --i)
            {
                manualObject.position(xAxisLines[i] * -xAxis.x + yAxisLines[i] * -yAxis.x + origin.x,
                                 xAxisLines[i] * -xAxis.y + yAxisLines[i] * -yAxis.y + origin.y,
                                 xAxisLines[i] * -xAxis.z + yAxisLines[i] * -yAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
                manualObject.position(xAxisLines[i - 1] * -xAxis.x + yAxisLines[i - 1] * -yAxis.x + origin.x,
                                 xAxisLines[i - 1] * -xAxis.y + yAxisLines[i - 1] * -yAxis.y + origin.y,
                                 xAxisLines[i - 1] * -xAxis.z + yAxisLines[i - 1] * -yAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
            }

            //sixth octant
            for (i = NUM_LINES_IN_45 - 1; i != 0; --i)
            {
                manualObject.position(xAxisLines[i] * -yAxis.x + yAxisLines[i] * -xAxis.x + origin.x,
                                 xAxisLines[i] * -yAxis.y + yAxisLines[i] * -xAxis.y + origin.y,
                                 xAxisLines[i] * -yAxis.z + yAxisLines[i] * -xAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
                manualObject.position(xAxisLines[i - 1] * -yAxis.x + yAxisLines[i - 1] * -xAxis.x + origin.x,
                                 xAxisLines[i - 1] * -yAxis.y + yAxisLines[i - 1] * -xAxis.y + origin.y,
                                 xAxisLines[i - 1] * -yAxis.z + yAxisLines[i - 1] * -xAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
            }

            //seventh octant
            for (i = NUM_LINES_IN_45 - 1; i != 0; --i)
            {
                manualObject.position(xAxisLines[i] * yAxis.x + yAxisLines[i] * -xAxis.x + origin.x,
                                 xAxisLines[i] * yAxis.y + yAxisLines[i] * -xAxis.y + origin.y,
                                 xAxisLines[i] * yAxis.z + yAxisLines[i] * -xAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
                manualObject.position(xAxisLines[i - 1] * yAxis.x + yAxisLines[i - 1] * -xAxis.x + origin.x,
                                 xAxisLines[i - 1] * yAxis.y + yAxisLines[i - 1] * -xAxis.y + origin.y,
                                 xAxisLines[i - 1] * yAxis.z + yAxisLines[i - 1] * -xAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
            }

            //eigth octant
            for (i = NUM_LINES_IN_45 - 1; i != 0; --i)
            {
                manualObject.position(xAxisLines[i] * xAxis.x + yAxisLines[i] * -yAxis.x + origin.x,
                                 xAxisLines[i] * xAxis.y + yAxisLines[i] * -yAxis.y + origin.y,
                                 xAxisLines[i] * xAxis.z + yAxisLines[i] * -yAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
                manualObject.position(xAxisLines[i - 1] * xAxis.x + yAxisLines[i - 1] * -yAxis.x + origin.x,
                                 xAxisLines[i - 1] * xAxis.y + yAxisLines[i - 1] * -yAxis.y + origin.y,
                                 xAxisLines[i - 1] * xAxis.z + yAxisLines[i - 1] * -yAxis.z + origin.z);
                manualObject.color(color.r, color.g, color.b, color.a);
            }
        }
    }
}
